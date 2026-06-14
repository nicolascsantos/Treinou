# Plan 005: Fix N+1 Query, Remove Unnecessary Eager Load, Align Repository Lifetimes

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan
> in `plans/README.md`.
>
> **Drift check (run first)**:
> ```
> git diff --stat 8937b9b..HEAD -- src/Treinou.Application/UseCases/Student/ListStudent/ListStudents.cs src/Treinou.Infraestructure/Repositories/StudentRepository.cs src/Treinou.Infraestructure/Repositories/TeacherRepository.cs src/Treinou.API/Configurations/UseCasesConfiguration.cs
> ```
> If any in-scope file changed since this plan was written, compare the
> "Current state" excerpts against the live code before proceeding; on a
> mismatch, treat it as a STOP condition.

## Status

- **Priority**: P2
- **Effort**: S
- **Risk**: LOW
- **Depends on**: none
- **Category**: perf
- **Planned at**: commit `8937b9b`, 2026-06-14

## Why this matters

Three related performance and correctness issues:

1. **N+1 query in `ListStudents`**: After calling `StudentRepository.Search()` (one SQL query), the handler loops through results and calls `_teacherRepository.Get(student.TeacherId)` once per student. Listing 50 students issues 51 queries. Listing 200 students issues 201 queries. Fix: eager-load Teacher in `StudentRepository.Search()` and remove the loop.

2. **`TeacherRepository.Search()` loads the `Students` collection unnecessarily**: The search query chains `.Include(x => x.Students)`, loading every Student for every Teacher in the result page. The list output model does not use students — this is a Cartesian product amplifying row count proportionally to students-per-teacher. Fix: remove the Include from Search; keep it in Get (where the detail view may use it).

3. **Repositories registered as `Transient` instead of `Scoped`**: All 6 repository registrations use `.AddTransient<>()`. EF Core `DbContext` is registered as `Scoped` (default for `AddDbContext`). Within a single HTTP request, DI will create a new repository instance per injection point, but each will resolve the same scoped `DbContext`. The current behavior is *accidentally* correct (shared DbContext), but the intent is wrong — `Transient` signals "new instance every time" which conflicts with the Unit of Work pattern. Fix: change to `AddScoped<>()` to make the intent match the behavior and ensure proper change-tracking semantics.

## Current state

### Bug 1 — N+1 in `ListStudents` (`src/Treinou.Application/UseCases/Student/ListStudent/ListStudents.cs:20-26`)

```csharp
public async Task<ListStudentsOutput> Handle(ListStudentsInput request, CancellationToken cancellationToken)
{
    var searchOutput = await _studentRepository.Search(request.ToSearchInput(), cancellationToken);

    foreach (var student in searchOutput.Items)   // ← N iterations
    {
        if (student.TeacherId != default)
        {
            var teacher = await _teacherRepository.Get(student.TeacherId, cancellationToken);  // ← N queries
            student.Teacher = teacher;
        }
    }

    var output = ListStudentsOutput.FromSearchOutput(searchOutput);
    return output;
}
```

### `StudentRepository.Search()` — does not include Teacher (`src/Treinou.Infraestructure/Repositories/StudentRepository.cs:44-55`)

```csharp
public async Task<SearchOutput<Student>> Search(SearchInput searchInput, CancellationToken cancellationToken)
{
    var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
    var query = _students.AsNoTracking();   // ← no .Include(x => x.Teacher)
    query = AddOrderToQuery(query, searchInput.OrderBy, searchInput.Order);
    // ...
}
```

### Bug 2 — TeacherRepository.Search() unnecessary Include (`src/Treinou.Infraestructure/Repositories/TeacherRepository.cs:57-68`)

```csharp
public async Task<SearchOutput<Teacher>> Search(SearchInput searchInput, CancellationToken cancellationToken)
{
    var toSkip = (searchInput.Page - 1) * searchInput.PerPage;
    var query = _teachers.AsNoTracking().Include(x => x.Students).AsQueryable();  // ← loads all students
    // ...
}
```

`TeacherRepository.Get()` also includes Students (line 26-28) — keep that, as the detail view may use it.

### Bug 3 — Transient repositories (`src/Treinou.API/Configurations/UseCasesConfiguration.cs:19-26`)

```csharp
private static IServiceCollection AddRepositories(this IServiceCollection services)
{
    services.AddTransient<IExerciseRepository, ExerciseRepository>();
    services.AddTransient<IExerciseTypeRepository, ExerciseTypeRepository>();
    services.AddTransient<IStudentRepository, StudentRepository>();
    services.AddTransient<ITeacherRepository, TeacherRepository>();
    services.AddTransient<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
    services.AddTransient<IWorkoutRepository, WorkoutRepository>();
    services.AddTransient<IUnitOfWork, UnitOfWork>();
    return services;
}
```

### Repo conventions to match

- Eager loading in Search uses `.Include()` before `.AsNoTracking()` or chained in the query — see `StudentRepository.Get()` (line 24-26): `.Include(x => x.Teacher).FirstOrDefaultAsync(...)`.
- Service lifetimes are configured in `UseCasesConfiguration.AddRepositories()`.

## Commands you will need

| Purpose     | Command                                                                              | Expected on success           |
|-------------|--------------------------------------------------------------------------------------|-------------------------------|
| Build       | `dotnet build Treinou.slnx`                                                          | exit 0, no errors             |
| Grep verify | `grep -n "AddTransient" src/Treinou.API/Configurations/UseCasesConfiguration.cs`    | no output (all changed to Scoped) |
| Grep verify | `grep -n "Include(x => x.Students)" src/Treinou.Infraestructure/Repositories/TeacherRepository.cs` | 1 match only (in Get(), not Search()) |

## Scope

**In scope** (only these files):
- `src/Treinou.Application/UseCases/Student/ListStudent/ListStudents.cs`
- `src/Treinou.Infraestructure/Repositories/StudentRepository.cs`
- `src/Treinou.Infraestructure/Repositories/TeacherRepository.cs`
- `src/Treinou.API/Configurations/UseCasesConfiguration.cs`

**Out of scope** (do NOT touch):
- `ListStudentsOutput.cs` — no change needed
- `StudentModelOutput.cs` — no change needed; Teacher will now be pre-loaded
- Other repository files (ExerciseRepository, WorkoutRepository, etc.) — the Transient→Scoped fix is batched in Step 3 for all 6, but no other logic changes
- `TeacherRepository.Get()` — keep its `.Include(x => x.Students)` intact

## Git workflow

- Branch: `advisor/005-performance-queries`
- One commit per step; message style: `perf: <description>`
- Do NOT push or open a PR unless instructed

## Steps

### Step 1: Eager-load Teacher in `StudentRepository.Search()`

In `src/Treinou.Infraestructure/Repositories/StudentRepository.cs`, find the `Search()` method.

Change:
```csharp
var query = _students.AsNoTracking();
```
To:
```csharp
var query = _students.AsNoTracking().Include(x => x.Teacher);
```

This ensures every student returned by Search already has the Teacher navigation property populated, matching the behavior of `StudentRepository.Get()`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 2: Remove the N+1 teacher-fetch loop from `ListStudents`

In `src/Treinou.Application/UseCases/Student/ListStudent/ListStudents.cs`:

1. Remove the entire `foreach` block (lines 20–27):
   ```csharp
   // DELETE this block:
   foreach (var student in searchOutput.Items)
   {
       if (student.TeacherId != default)
       {
           var teacher = await _teacherRepository.Get(student.TeacherId, cancellationToken);
           student.Teacher = teacher;
       }
   }
   ```
2. The `_teacherRepository` field and constructor parameter are now unused. Remove:
   - The `private readonly ITeacherRepository _teacherRepository;` field
   - The `ITeacherRepository teacherRepository` constructor parameter
   - The `_teacherRepository = teacherRepository` assignment in the constructor body

The resulting `Handle` method should look like:
```csharp
public async Task<ListStudentsOutput> Handle(ListStudentsInput request, CancellationToken cancellationToken)
{
    var searchOutput = await _studentRepository.Search(request.ToSearchInput(), cancellationToken);
    var output = ListStudentsOutput.FromSearchOutput(searchOutput);
    return output;
}
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 3: Remove unnecessary Students Include from `TeacherRepository.Search()`

In `src/Treinou.Infraestructure/Repositories/TeacherRepository.cs`, find `Search()` (line 57).

Change:
```csharp
var query = _teachers.AsNoTracking().Include(x => x.Students).AsQueryable();
```
To:
```csharp
var query = _teachers.AsNoTracking().AsQueryable();
```

Do NOT change `TeacherRepository.Get()` — it retains `.Include(x => x.Students)` for the detail view.

**Verify**: `grep -n "Include(x => x.Students)" src/Treinou.Infraestructure/Repositories/TeacherRepository.cs` → exactly 1 match (in `Get()`, not `Search()`)  
**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 4: Change all repository registrations from `Transient` to `Scoped`

In `src/Treinou.API/Configurations/UseCasesConfiguration.cs`, in `AddRepositories()`:

Change every `AddTransient` to `AddScoped`:
```csharp
services.AddScoped<IExerciseRepository, ExerciseRepository>();
services.AddScoped<IExerciseTypeRepository, ExerciseTypeRepository>();
services.AddScoped<IStudentRepository, StudentRepository>();
services.AddScoped<ITeacherRepository, TeacherRepository>();
services.AddScoped<IWorkoutExerciseRepository, WorkoutExerciseRepository>();
services.AddScoped<IWorkoutRepository, WorkoutRepository>();
services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**Verify**: `grep -n "AddTransient" src/Treinou.API/Configurations/UseCasesConfiguration.cs` → no output  
**Verify**: `dotnet build Treinou.slnx` → exit 0

## Test plan

No test project exists yet (Plan 006 creates it). When it lands, add:

- `StudentRepository_Search_ShouldIncludeTeacher()` — search returns students with Teacher populated, no null
- `ListStudents_ShouldIssueOneQuery_ForPageOf50Students()` — use EF Core query counter or logging to assert query count = 1 (requires DbContext diagnostics setup)
- `TeacherRepository_Search_ShouldNotLoadStudents()` — search result teachers have empty Students collection

## Done criteria

- [ ] `dotnet build Treinou.slnx` exits 0
- [ ] `grep -n "AddTransient" src/Treinou.API/Configurations/UseCasesConfiguration.cs` returns no output
- [ ] `grep -n "_teacherRepository" src/Treinou.Application/UseCases/Student/ListStudent/ListStudents.cs` returns no output
- [ ] `grep -n "Include(x => x.Teacher)" src/Treinou.Infraestructure/Repositories/StudentRepository.cs` returns 1 match (in Search)
- [ ] `grep -n "Include(x => x.Students)" src/Treinou.Infraestructure/Repositories/TeacherRepository.cs` returns 1 match (in Get only)
- [ ] No files outside the in-scope list are modified (`git status`)
- [ ] `plans/README.md` status row updated to DONE

## STOP conditions

Stop and report back (do not improvise) if:

- Removing the `_teacherRepository` from `ListStudents` causes a DI resolution error at runtime (would indicate another handler also injects it via `ListStudents` — unlikely but check)
- `StudentModelOutput.FromStudent()` throws when `student.Teacher` is null after this change (would indicate some students in the database have `TeacherId == default` and are not linked to a teacher — if so, make the Teacher null-safe in the output model before removing the loop)
- The build fails due to an EF Core expression tree limitation on the new Include chain — report the exact error

## Maintenance notes

- When adding new list use cases in the future, check whether the repository's `Search()` already eager-loads the necessary navigation properties. The N+1 pattern is easy to re-introduce.
- `TeacherRepository.Get()` still loads Students — if a teacher detail view is ever added that doesn't need students (e.g., a lightweight profile endpoint), create a separate `GetLightweight()` method rather than modifying `Get()`.
- The `Transient→Scoped` change means repositories are now tied to the HTTP request scope. Do not use them in singleton services or background tasks without a dedicated `IServiceScopeFactory` scope.
