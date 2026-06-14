# Plan 001: Fix Critical Data Bugs and Broken API Surface

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan
> in `plans/README.md`.
>
> **Drift check (run first)**:
> ```
> git diff --stat 8937b9b..HEAD -- src/Treinou.API/Controllers/ExerciseTypeController.cs src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs src/Treinou.API/Controllers/WorkoutController.cs src/Treinou.API/Controllers/StudentController.cs src/Treinou.API/Controllers/TeacherController.cs src/Treinou.API/Controllers/ExerciseController.cs src/Treinou.API/Controllers/WorkoutExerciseController.cs
> ```
> If any in-scope file changed since this plan was written, compare the
> "Current state" excerpts against the live code before proceeding; on a
> mismatch, treat it as a STOP condition.

## Status

- **Priority**: P1
- **Effort**: S
- **Risk**: LOW
- **Depends on**: none
- **Category**: bug
- **Planned at**: commit `8937b9b`, 2026-06-14

## Why this matters

Five distinct bugs make parts of the API either silently broken or actively dangerous:

1. `ExerciseTypeController.Update()` accepts `UpdateStudentInput` and dispatches to the Student use case — a PUT to `/api/exercisetype` **updates a Student record**, not an ExerciseType. This is data corruption.
2. `WorkoutExerciseRepository.GetByWorkoutId()` filters by the entity's own primary key instead of the `WorkoutId` foreign key — it always returns an empty list, silently breaking any feature that loads exercises for a workout.
3. `WorkoutExerciseRepository.Search()` accesses `x.Workout.Name` without an EF Core `.Include()` — a `NullReferenceException` is thrown on every non-empty search request.
4. `WorkoutController` has no PUT endpoint — the `UpdateWorkout` use case is fully implemented in the Application layer but unreachable via API.
5. All GET and DELETE route constraints are written `{id::guid}` (double colon) — the empty token before `guid` is silently discarded by ASP.NET Core's parser; single-colon `{id:guid}` is the standard and avoids future framework surprises.

## Current state

### File roles
- `src/Treinou.API/Controllers/ExerciseTypeController.cs` — ExerciseType CRUD controller; contains the wrong dispatch bug
- `src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs` — repository for WorkoutExercise; contains two bugs
- `src/Treinou.API/Controllers/WorkoutController.cs` — Workout CRUD controller; missing PUT
- `src/Treinou.API/Controllers/StudentController.cs`, `TeacherController.cs`, `ExerciseController.cs`, `WorkoutExerciseController.cs`, `ExerciseTypeController.cs`, `WorkoutController.cs` — all have `{id::guid}` double-colon

### Bug 1 — ExerciseTypeController.Update() (`ExerciseTypeController.cs:77-84`)

Current (wrong):
```csharp
// Lines 9-10 — wrong using statements imported
using Treinou.Application.UseCases.Student.Common;
using Treinou.Application.UseCases.Student.UpdateStudent;

// Lines 77-84
[HttpPut]
[ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<StudentModelOutput>))]
[ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
public async Task<IActionResult> Update([FromBody] UpdateStudentInput input, CancellationToken cancellationToken)
{
    var output = await _mediator.Send(input, cancellationToken);
    return Ok(new APIResponse<StudentModelOutput>(output));
}
```

Target correct types (already exist in the project):
- Input: `UpdateExerciseTypeInput` from `Treinou.Application.UseCases.ExerciseType.UpdateExerciseType`
- Output: `ExerciseTypeModelOutput` from `Treinou.Application.UseCases.ExerciseType.Common`

`UpdateExerciseTypeInput` definition (for reference):
```csharp
// src/Treinou.Application/UseCases/ExerciseType/UpdateExerciseType/UpdateExerciseTypeInput.cs
public class UpdateExerciseTypeInput : IRequest<ExerciseTypeModelOutput>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public UpdateExerciseTypeInput(Guid id, string name) { Id = id; Name = name; }
}
```

### Bug 2 — Wrong filter in `GetByWorkoutId` (`WorkoutExerciseRepository.cs:37`)

Current (wrong):
```csharp
public async Task<IEnumerable<WorkoutExercise>> GetByWorkoutId(Guid workoutId, CancellationToken cancellationToken)
{
    return await _workoutExercises
        .Include(we => we.Exercise)
        .Where(we => we.Id == workoutId)   // ← BUG: compares primary key to workoutId
        .OrderBy(we => we.Order)
        .ToListAsync(cancellationToken);
}
```

### Bug 3 — Missing Include in `Search` (`WorkoutExerciseRepository.cs:54-58`)

Current (wrong):
```csharp
var query = _workoutExercises.AsNoTracking().AsQueryable();
// ...
if (!string.IsNullOrWhiteSpace(searchInput.Search))
    query = query.Where(x => x.Workout.Name.Contains(searchInput.Search)); // ← NRE: Workout not loaded
```

### Bug 4 — WorkoutController missing PUT (`WorkoutController.cs`)

The controller has Get, Post, Delete, and Search — no HttpPut. The use case
`UpdateWorkoutInput` (`src/Treinou.Application/UseCases/Workout/UpdateWorkout/UpdateWorkoutInput.cs`) and handler exist:
```csharp
public class UpdateWorkoutInput : IRequest<WorkoutModelOutput>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid TeacherId { get; private set; }
    public Guid StudentId { get; private set; }
    // ...
}
```

Pattern to follow: `StudentController.Update` (lines 75-82 of `StudentController.cs`):
```csharp
[HttpPut]
[ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<StudentModelOutput>))]
[ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
public async Task<IActionResult> Update([FromBody] UpdateStudentInput input, CancellationToken cancellationToken)
{
    var output = await _mediator.Send(input, cancellationToken);
    return Ok(new APIResponse<StudentModelOutput>(output));
}
```

### Bug 5 — Route constraint syntax (`{id::guid}` in all controllers)

All GET and DELETE route attributes use double colon. Example from `ExerciseTypeController.cs:26`:
```csharp
[HttpGet("{id::guid}")]   // ← should be {id:guid}
[HttpDelete("{id::guid}")] // ← should be {id:guid}
```

Affected files: `StudentController.cs`, `TeacherController.cs`, `WorkoutController.cs`,
`WorkoutExerciseController.cs`, `ExerciseController.cs`, `ExerciseTypeController.cs` — each has two route attributes to fix.

## Commands you will need

| Purpose     | Command                                   | Expected on success           |
|-------------|-------------------------------------------|-------------------------------|
| Build       | `dotnet build Treinou.slnx`               | exit 0, no errors             |
| Grep verify | `grep -rn "{id::guid}" src/Treinou.API/Controllers/` | no output |
| Grep verify | `grep -n "UpdateStudentInput" src/Treinou.API/Controllers/ExerciseTypeController.cs` | no output |
| Grep verify | `grep -n "we.Id == workoutId" src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs` | no output |

## Scope

**In scope** (only these files):
- `src/Treinou.API/Controllers/ExerciseTypeController.cs`
- `src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs`
- `src/Treinou.API/Controllers/WorkoutController.cs`
- `src/Treinou.API/Controllers/StudentController.cs` (route constraints only)
- `src/Treinou.API/Controllers/TeacherController.cs` (route constraints only)
- `src/Treinou.API/Controllers/ExerciseController.cs` (route constraints only)
- `src/Treinou.API/Controllers/WorkoutExerciseController.cs` (route constraints only)

**Out of scope** (do NOT touch):
- `UpdateExerciseTypeInput.cs`, `UpdateWorkoutInput.cs`, and all use case implementations — they are correct
- `WorkoutExerciseController.cs` Update method — a separate correctness fix, not in this plan
- Any domain entity

## Git workflow

- Branch: `advisor/001-critical-data-bugs`
- Commit after each step; message style: `fix: <short description>` (e.g., `fix: ExerciseTypeController Update dispatches correct use case`)
- Do NOT push or open a PR unless instructed

## Steps

### Step 1: Fix `ExerciseTypeController.Update()` input and output types

In `src/Treinou.API/Controllers/ExerciseTypeController.cs`:

1. Remove the two wrong using statements:
   ```csharp
   using Treinou.Application.UseCases.Student.Common;         // remove
   using Treinou.Application.UseCases.Student.UpdateStudent;  // remove
   ```
2. Add the correct using statements (the other ExerciseType usings already present can be verified — `UpdateExerciseType` namespace needed):
   ```csharp
   using Treinou.Application.UseCases.ExerciseType.UpdateExerciseType;
   ```
   (`ExerciseTypeModelOutput` is already imported via `Treinou.Application.UseCases.ExerciseType.Common` at the top.)
3. Change the `Update` method signature and body:
   ```csharp
   [HttpPut]
   [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<ExerciseTypeModelOutput>))]
   [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
   public async Task<IActionResult> Update([FromBody] UpdateExerciseTypeInput input, CancellationToken cancellationToken)
   {
       var output = await _mediator.Send(input, cancellationToken);
       return Ok(new APIResponse<ExerciseTypeModelOutput>(output));
   }
   ```

**Verify**: `dotnet build Treinou.slnx` → exit 0  
**Verify**: `grep -n "UpdateStudentInput" src/Treinou.API/Controllers/ExerciseTypeController.cs` → no output

### Step 2: Fix `WorkoutExerciseRepository.GetByWorkoutId()` filter

In `src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs`, line 37:

Change:
```csharp
.Where(we => we.Id == workoutId)
```
To:
```csharp
.Where(we => we.WorkoutId == workoutId)
```

**Verify**: `grep -n "we.Id == workoutId" src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs` → no output  
**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 3: Fix `WorkoutExerciseRepository.Search()` — add Include for Workout

In `src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs`, line 54:

Change:
```csharp
var query = _workoutExercises.AsNoTracking().AsQueryable();
```
To:
```csharp
var query = _workoutExercises.AsNoTracking().Include(x => x.Workout).AsQueryable();
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 4: Add PUT endpoint to `WorkoutController`

In `src/Treinou.API/Controllers/WorkoutController.cs`:

1. Add the missing using for UpdateWorkout:
   ```csharp
   using Treinou.Application.UseCases.Workout.UpdateWorkout;
   ```
2. Add the following method after the `Delete` method (around line 49), before the `Search` method:
   ```csharp
   [HttpPut]
   [ProducesResponseType(200, StatusCode = StatusCodes.Status200OK, Type = typeof(APIResponse<WorkoutModelOutput>))]
   [ProducesResponseType(404, StatusCode = StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
   public async Task<IActionResult> Update([FromBody] UpdateWorkoutInput input, CancellationToken cancellationToken)
   {
       var output = await _mediator.Send(input, cancellationToken);
       return Ok(new APIResponse<WorkoutModelOutput>(output));
   }
   ```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 5: Fix double-colon route constraints in all 6 controllers

In each of the following files, replace every occurrence of `{id::guid}` with `{id:guid}`:
- `src/Treinou.API/Controllers/StudentController.cs`
- `src/Treinou.API/Controllers/TeacherController.cs`
- `src/Treinou.API/Controllers/WorkoutController.cs`
- `src/Treinou.API/Controllers/WorkoutExerciseController.cs`
- `src/Treinou.API/Controllers/ExerciseController.cs`
- `src/Treinou.API/Controllers/ExerciseTypeController.cs`

Each file has exactly two occurrences (one `HttpGet`, one `HttpDelete`). Use find-and-replace in your editor, or run:
```
# Verify before fixing by counting occurrences
grep -rn "{id::guid}" src/Treinou.API/Controllers/
```

After replacing in all files:

**Verify**: `grep -rn "{id::guid}" src/Treinou.API/Controllers/` → no output  
**Verify**: `dotnet build Treinou.slnx` → exit 0

## Test plan

No test project exists yet (Plan 006 creates it). For now, manual smoke tests:

1. Run the API: `dotnet run --project src/Treinou.API/Treinou.API.csproj`
2. Open Swagger at `https://localhost:<port>/swagger`
3. Verify PUT `/api/exercisetype` appears in Swagger with the correct schema (not StudentInput)
4. Verify PUT `/api/workout` appears in Swagger
5. Verify GET `/api/student/{id}` routing works for a valid GUID

When Plan 006 lands, add these regression tests:
- `ExerciseTypeController_Update_ShouldDispatchUpdateExerciseTypeInput`
- `WorkoutExerciseRepository_GetByWorkoutId_ShouldReturnExercisesForWorkout`
- `WorkoutExerciseRepository_Search_ShouldNotThrowWhenSearchTermProvided`

## Done criteria

- [ ] `dotnet build Treinou.slnx` exits 0
- [ ] `grep -n "UpdateStudentInput" src/Treinou.API/Controllers/ExerciseTypeController.cs` returns no output
- [ ] `grep -n "StudentModelOutput" src/Treinou.API/Controllers/ExerciseTypeController.cs` returns no output
- [ ] `grep -n "we.Id == workoutId" src/Treinou.Infraestructure/Repositories/WorkoutExerciseRepository.cs` returns no output
- [ ] `grep -rn "{id::guid}" src/Treinou.API/Controllers/` returns no output
- [ ] `WorkoutController.cs` contains `[HttpPut]` (verify with `grep -n "HttpPut" src/Treinou.API/Controllers/WorkoutController.cs`)
- [ ] No files outside the in-scope list are modified (`git status`)
- [ ] `plans/README.md` status row updated to DONE

## STOP conditions

Stop and report back (do not improvise) if:

- The code at the locations in "Current state" doesn't match the excerpts (codebase has drifted since this plan was written — run the drift check at the top).
- `dotnet build` fails after a step and you cannot resolve the error in one attempt.
- `UpdateExerciseTypeInput` or `ExerciseTypeModelOutput` do not exist at the expected namespace (the plan assumes they exist — verify before step 1).
- A step's verification fails twice after a reasonable fix attempt.

## Maintenance notes

- When a new resource controller is added to the project, always use `{id:guid}` (single colon) in route attributes.
- `ExerciseTypeController.cs` was created by copy-pasting from `StudentController.cs` without updating types — when adding future controllers, verify all type references match the entity being served.
- Plan 004 adds `[Authorize]` to all controllers; when it lands, check that this plan's changes are still correct (they will be — `[Authorize]` is a class-level attribute).
