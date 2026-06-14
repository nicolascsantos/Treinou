# Plan 006: Establish Test Baseline — Domain Unit Tests and Integration Test Fixtures

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan
> in `plans/README.md`.
>
> **Drift check (run first)**:
> ```
> git diff --stat 8937b9b..HEAD -- src/Treinou.Domain/Entities/ src/Treinou.Domain/Validation/ src/Treinou.Domain/ValueObjects/ src/Treinou.Infraestructure/Repositories/StudentRepository.cs
> ```
> If any in-scope file changed since this plan was written (especially if Plans 001 or 002 have already landed), re-read the affected source files before writing tests for them.

## Status

- **Priority**: P2
- **Effort**: L
- **Risk**: LOW
- **Depends on**: none (can land independently; running Plans 001–005 first means tests immediately cover the fixed code)
- **Category**: tests
- **Planned at**: commit `8937b9b`, 2026-06-14

## Why this matters

The `tests/` directory is empty. There are zero tests anywhere in the codebase. This means:

- Every bug in Plans 001–005 went undetected because there was no automated check.
- Any future change to domain validation, entity behavior, or repository logic has no safety net.
- Refactoring the architecture (e.g., WorkoutExercise aggregate boundary) cannot be done confidently.

This plan establishes the minimum viable test baseline: a unit test project for the domain layer and an integration test project with EF Core InMemory fixtures for repositories. It does not aim for comprehensive coverage — it aims to make `dotnet test` pass and to cover the highest-risk paths.

## Current state

### Infrastructure already available

`src/Treinou.Infraestructure/Treinou.Infraestructure.csproj` already references:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="10.0.6" />
```
This package is the EF Core in-memory provider used for integration tests. It does not need to be added to the infrastructure project — but a test project must reference it (or reference the infrastructure project which pulls it transitively).

### Domain validation (`src/Treinou.Domain/Validation/DomainValidation.cs`)

```csharp
public static class DomainValidation
{
    public static void NotNull(object? target, string fieldName) { ... }
    public static void NotNullOrEmpty(string? target, string fieldName) { ... }
    public static void MinLength(string target, string fieldName, int minLength) { ... }
    public static void MaxLength(string target, string fieldName, int maxLength) { ... }
}
```

### Key domain entities to cover first

- `Exercise` (src/Treinou.Domain/Entities/Exercise.cs) — has the `isActive` bug fixed in Plan 002
- `Workout` (src/Treinou.Domain/Entities/Workout.cs) — has the `Validate()` in `Update()` added in Plan 002
- `WorkoutExercise` (src/Treinou.Domain/Entities/WorkoutExercise.cs) — has the `Validate()` in `Update()` added in Plan 002
- `Student` (src/Treinou.Domain/Entities/Student.cs) — core aggregate with name/email/CPF validation
- `Teacher` (src/Treinou.Domain/Entities/Teacher.cs) — has the `createdAt` bug fixed in Plan 002

### Project naming convention

Existing projects: `Treinou.API`, `Treinou.Application`, `Treinou.Domain`, `Treinou.Infraestructure`. Test projects follow the pattern `<Project>.Tests` (e.g., `Treinou.Domain.Tests`).

### Build command for the whole solution

```bash
dotnet build Treinou.slnx
```

### Solution file format

The solution uses `.slnx` (XML solution format from .NET 10). To add projects to the solution:
```bash
dotnet sln Treinou.slnx add tests/Treinou.Domain.Tests/Treinou.Domain.Tests.csproj
dotnet sln Treinou.slnx add tests/Treinou.Integration.Tests/Treinou.Integration.Tests.csproj
```

## Commands you will need

| Purpose            | Command                                                              | Expected on success               |
|--------------------|----------------------------------------------------------------------|-----------------------------------|
| Create project     | `dotnet new xunit -o tests/Treinou.Domain.Tests`                    | exit 0, project created           |
| Add to solution    | `dotnet sln Treinou.slnx add tests/Treinou.Domain.Tests/...`        | exit 0                            |
| Build              | `dotnet build Treinou.slnx`                                         | exit 0                            |
| Run tests          | `dotnet test Treinou.slnx`                                          | all tests pass                    |
| Run specific       | `dotnet test tests/Treinou.Domain.Tests/`                           | all tests pass                    |

## Scope

**In scope** (new files to create):
- `tests/Treinou.Domain.Tests/Treinou.Domain.Tests.csproj`
- `tests/Treinou.Domain.Tests/Validation/DomainValidationTests.cs`
- `tests/Treinou.Domain.Tests/Entities/ExerciseTests.cs`
- `tests/Treinou.Domain.Tests/Entities/WorkoutTests.cs`
- `tests/Treinou.Domain.Tests/Entities/WorkoutExerciseTests.cs`
- `tests/Treinou.Domain.Tests/Entities/StudentTests.cs`
- `tests/Treinou.Integration.Tests/Treinou.Integration.Tests.csproj`
- `tests/Treinou.Integration.Tests/Fixtures/DbContextFixture.cs`
- `tests/Treinou.Integration.Tests/Repositories/StudentRepositoryTests.cs`

**Out of scope** (do NOT touch):
- Any source file in `src/` — this plan only creates test files
- API integration tests (WebApplicationFactory) — deferred to a later plan; authentication makes them harder to set up

## Git workflow

- Branch: `advisor/006-test-baseline`
- Commit after each step; message style: `test: <description>`
- Do NOT push or open a PR unless instructed

## Steps

### Step 1: Create the domain unit test project

From the repo root:

```bash
dotnet new xunit -o tests/Treinou.Domain.Tests -f net10.0
dotnet sln Treinou.slnx add tests/Treinou.Domain.Tests/Treinou.Domain.Tests.csproj
```

Edit `tests/Treinou.Domain.Tests/Treinou.Domain.Tests.csproj` to add the project reference to `Treinou.Domain`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.3">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\src\Treinou.Domain\Treinou.Domain.csproj" />
  </ItemGroup>
</Project>
```

Delete the default `UnitTest1.cs` file created by `dotnet new xunit`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 2: Write `DomainValidationTests.cs`

Create `tests/Treinou.Domain.Tests/Validation/DomainValidationTests.cs`:

```csharp
using Treinou.Domain.Exceptions;
using Treinou.Domain.Validation;

namespace Treinou.Domain.Tests.Validation;

public class DomainValidationTests
{
    [Fact]
    public void NotNull_ShouldThrow_WhenTargetIsNull()
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            DomainValidation.NotNull(null, "Field"));
        Assert.Contains("Field", ex.Message);
    }

    [Fact]
    public void NotNull_ShouldNotThrow_WhenTargetIsNotNull()
        => DomainValidation.NotNull("value", "Field");

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void NotNullOrEmpty_ShouldThrow_WhenTargetIsNullOrWhitespace(string? value)
    {
        Assert.Throws<EntityValidationException>(() =>
            DomainValidation.NotNullOrEmpty(value, "Field"));
    }

    [Fact]
    public void NotNullOrEmpty_ShouldNotThrow_WhenTargetHasValue()
        => DomainValidation.NotNullOrEmpty("valid", "Field");

    [Fact]
    public void MinLength_ShouldThrow_WhenTooShort()
    {
        Assert.Throws<EntityValidationException>(() =>
            DomainValidation.MinLength("ab", "Field", 3));
    }

    [Fact]
    public void MinLength_ShouldNotThrow_AtExactMinimum()
        => DomainValidation.MinLength("abc", "Field", 3);

    [Fact]
    public void MaxLength_ShouldThrow_WhenTooLong()
    {
        Assert.Throws<EntityValidationException>(() =>
            DomainValidation.MaxLength(new string('x', 201), "Field", 200));
    }

    [Fact]
    public void MaxLength_ShouldNotThrow_AtExactMaximum()
        => DomainValidation.MaxLength(new string('x', 200), "Field", 200);
}
```

**Verify**: `dotnet test tests/Treinou.Domain.Tests/` → all tests pass

### Step 3: Write `ExerciseTests.cs`

Create `tests/Treinou.Domain.Tests/Entities/ExerciseTests.cs`:

```csharp
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;

namespace Treinou.Domain.Tests.Entities;

public class ExerciseTests
{
    [Fact]
    public void Constructor_ShouldCreateActiveExercise_ByDefault()
    {
        var exercise = new Exercise("Push Up", Guid.NewGuid());
        Assert.True(exercise.IsActive);
    }

    [Fact]
    public void Constructor_ShouldRespectIsActiveFalse()
    {
        var exercise = new Exercise("Push Up", Guid.NewGuid(), isActive: false);
        Assert.False(exercise.IsActive);  // verifies Plan 002 fix
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        Assert.Throws<EntityValidationException>(() =>
            new Exercise("", Guid.NewGuid()));
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var exercise = new Exercise("Push Up", Guid.NewGuid());
        exercise.Deactivate();
        Assert.False(exercise.IsActive);
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrue()
    {
        var exercise = new Exercise("Push Up", Guid.NewGuid(), isActive: false);
        exercise.Activate();
        Assert.True(exercise.IsActive);
    }
}
```

**Verify**: `dotnet test tests/Treinou.Domain.Tests/` → all tests pass

### Step 4: Write `WorkoutTests.cs` and `WorkoutExerciseTests.cs`

Create `tests/Treinou.Domain.Tests/Entities/WorkoutTests.cs`:

```csharp
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;

namespace Treinou.Domain.Tests.Entities;

public class WorkoutTests
{
    [Fact]
    public void Update_ShouldThrow_WhenNameIsEmpty()
    {
        var workout = new Workout("Leg Day", Guid.NewGuid(), Guid.NewGuid());
        Assert.Throws<EntityValidationException>(() =>
            workout.Update("", Guid.NewGuid(), Guid.NewGuid()));  // verifies Plan 002 fix
    }

    [Fact]
    public void Update_ShouldSucceed_WithValidName()
    {
        var workout = new Workout("Leg Day", Guid.NewGuid(), Guid.NewGuid());
        var newTeacherId = Guid.NewGuid();
        var newStudentId = Guid.NewGuid();
        workout.Update("Upper Body", newTeacherId, newStudentId);
        Assert.Equal("Upper Body", workout.Name);
        Assert.Equal(newTeacherId, workout.TeacherId);
        Assert.Equal(newStudentId, workout.StudentId);
    }
}
```

Create `tests/Treinou.Domain.Tests/Entities/WorkoutExerciseTests.cs`:

```csharp
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;

namespace Treinou.Domain.Tests.Entities;

public class WorkoutExerciseTests
{
    private static WorkoutExercise ValidExercise() =>
        new WorkoutExercise(Guid.NewGuid(), Guid.NewGuid(), 1, 3, 10, TimeSpan.FromSeconds(60), "");

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldThrow_WhenOrderIsNotPositive(int order)
    {
        Assert.Throws<EntityValidationException>(() =>
            new WorkoutExercise(Guid.NewGuid(), Guid.NewGuid(), order, 3, 10, TimeSpan.FromSeconds(60), ""));
    }

    [Fact]
    public void Update_ShouldThrow_WhenOrderIsZero()
    {
        var we = ValidExercise();
        Assert.Throws<EntityValidationException>(() =>
            we.Update(order: 0, numberOfSets: 3, numberOfRepetitions: 10, rest: TimeSpan.FromSeconds(60)));  // verifies Plan 002 fix
    }

    [Fact]
    public void Update_ShouldThrow_WhenRestIsZero()
    {
        var we = ValidExercise();
        Assert.Throws<EntityValidationException>(() =>
            we.Update(order: 1, numberOfSets: 3, numberOfRepetitions: 10, rest: TimeSpan.Zero));
    }

    [Fact]
    public void Update_ShouldSucceed_WithValidValues()
    {
        var we = ValidExercise();
        we.Update(2, 4, 12, TimeSpan.FromSeconds(90), "updated notes");
        Assert.Equal(2, we.Order);
        Assert.Equal(4, we.NumberOfSets);
        Assert.Equal("updated notes", we.Notes);
    }
}
```

**Verify**: `dotnet test tests/Treinou.Domain.Tests/` → all tests pass

### Step 5: Write `StudentTests.cs`

Create `tests/Treinou.Domain.Tests/Entities/StudentTests.cs`:

```csharp
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.ValueObjects;

namespace Treinou.Domain.Tests.Entities;

public class StudentTests
{
    private static Email ValidEmail() => new Email("test@example.com");
    private static CPF ValidCPF() => new CPF("12345678901");
    private static PhoneNumber ValidPhone() => new PhoneNumber("11987654321");

    [Fact]
    public void Constructor_ShouldCreateStudent_WithValidData()
    {
        var student = new Student("John Doe", ValidEmail(), ValidCPF(), ValidPhone(), 70.0, 1.75);
        Assert.Equal("John Doe", student.Name);
        Assert.True(student.IsActive);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsTooShort()
    {
        Assert.Throws<EntityValidationException>(() =>
            new Student("Jo", ValidEmail(), ValidCPF(), ValidPhone(), 70.0, 1.75));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        Assert.Throws<EntityValidationException>(() =>
            new Student("", ValidEmail(), ValidCPF(), ValidPhone(), 70.0, 1.75));
    }

    [Fact]
    public void Activate_ShouldSetIsActiveTrue()
    {
        var student = new Student("John Doe", ValidEmail(), ValidCPF(), ValidPhone(), 70.0, 1.75, isActive: false);
        student.Activate();
        Assert.True(student.IsActive);
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveFalse()
    {
        var student = new Student("John Doe", ValidEmail(), ValidCPF(), ValidPhone(), 70.0, 1.75);
        student.Deactivate();
        Assert.False(student.IsActive);
    }
}
```

**Verify**: `dotnet test tests/Treinou.Domain.Tests/` → all tests pass

### Step 6: Create the integration test project

From the repo root:

```bash
dotnet new xunit -o tests/Treinou.Integration.Tests -f net10.0
dotnet sln Treinou.slnx add tests/Treinou.Integration.Tests/Treinou.Integration.Tests.csproj
```

Edit `tests/Treinou.Integration.Tests/Treinou.Integration.Tests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.3">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\src\Treinou.Domain\Treinou.Domain.csproj" />
    <ProjectReference Include="..\..\src\Treinou.Infraestructure\Treinou.Infraestructure.csproj" />
  </ItemGroup>
</Project>
```

Delete the default `UnitTest1.cs`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 7: Create the InMemory DbContext fixture

Create `tests/Treinou.Integration.Tests/Fixtures/DbContextFixture.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using Treinou.Infraestructure;

namespace Treinou.Integration.Tests.Fixtures;

public class DbContextFixture : IDisposable
{
    public TreinouDbContext DbContext { get; }

    public DbContextFixture()
    {
        var options = new DbContextOptionsBuilder<TreinouDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        DbContext = new TreinouDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}
```

**STOP condition**: If `TreinouDbContext` does not have a constructor that accepts `DbContextOptions<TreinouDbContext>`, stop and report — do not guess at the constructor signature. Read `src/Treinou.Infraestructure/TreinouDbContext.cs` to confirm.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 8: Write `StudentRepositoryTests.cs`

Create `tests/Treinou.Integration.Tests/Repositories/StudentRepositoryTests.cs`:

```csharp
using Treinou.Domain.Entities;
using Treinou.Domain.Exceptions;
using Treinou.Domain.SeedWork.SearchableRepository;
using Treinou.Domain.ValueObjects;
using Treinou.Infraestructure;
using Treinou.Infraestructure.Repositories;
using Treinou.Integration.Tests.Fixtures;

namespace Treinou.Integration.Tests.Repositories;

public class StudentRepositoryTests : IDisposable
{
    private readonly DbContextFixture _fixture;
    private readonly StudentRepository _repository;
    private readonly TreinouDbContext _dbContext;

    public StudentRepositoryTests()
    {
        _fixture = new DbContextFixture();
        _dbContext = _fixture.DbContext;
        _repository = new StudentRepository(_dbContext);
    }

    public void Dispose() => _fixture.Dispose();

    private static Teacher CreateTeacher()
        => new Teacher(
            "Teacher Name",
            new Email("teacher@example.com"),
            new CPF("12345678901"),
            new CREF("123456-G/SP"),
            "This is a valid description.",
            new PhoneNumber("11987654321"),
            new DateTime(1985, 1, 1),
            DateTime.UtcNow);

    private static Student CreateStudent(string name = "Student Name")
        => new Student(
            name,
            new Email($"{name.Replace(" ", "")}@example.com"),
            new CPF("98765432100"),
            new PhoneNumber("11912345678"),
            70.0,
            1.75);

    [Fact]
    public async Task Insert_ShouldPersistStudent()
    {
        var teacher = CreateTeacher();
        _dbContext.Set<Teacher>().Add(teacher);
        await _dbContext.SaveChangesAsync();

        var student = CreateStudent();
        student.TeacherId = teacher.Id;

        await _repository.Insert(student, CancellationToken.None);
        await _dbContext.SaveChangesAsync();

        var retrieved = await _repository.Get(student.Id, CancellationToken.None);
        Assert.Equal(student.Name, retrieved.Name);
    }

    [Fact]
    public async Task Get_ShouldThrowNotFoundException_WhenStudentDoesNotExist()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _repository.Get(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task Search_ShouldReturnPaginatedResults()
    {
        var teacher = CreateTeacher();
        _dbContext.Set<Teacher>().Add(teacher);
        await _dbContext.SaveChangesAsync();

        for (int i = 1; i <= 5; i++)
        {
            var s = CreateStudent($"Student {i}");
            s.TeacherId = teacher.Id;
            _dbContext.Set<Student>().Add(s);
        }
        await _dbContext.SaveChangesAsync();

        var result = await _repository.Search(
            new SearchInput(1, 3, "", "name", SearchOrder.ASCENDING),
            CancellationToken.None);

        Assert.Equal(5, result.Total);
        Assert.Equal(3, result.Items.Count);
    }

    [Fact]
    public async Task Delete_ShouldRemoveStudent()
    {
        var teacher = CreateTeacher();
        _dbContext.Set<Teacher>().Add(teacher);
        await _dbContext.SaveChangesAsync();

        var student = CreateStudent();
        student.TeacherId = teacher.Id;
        _dbContext.Set<Student>().Add(student);
        await _dbContext.SaveChangesAsync();

        await _repository.Delete(student, CancellationToken.None);
        await _dbContext.SaveChangesAsync();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _repository.Get(student.Id, CancellationToken.None));
    }
}
```

**Verify**: `dotnet test tests/Treinou.Integration.Tests/` → all tests pass

## Done criteria

- [ ] `dotnet build Treinou.slnx` exits 0
- [ ] `dotnet test Treinou.slnx` exits 0 with all tests passing
- [ ] `dotnet test Treinou.slnx` reports at least 20 tests discovered
- [ ] `tests/Treinou.Domain.Tests/` and `tests/Treinou.Integration.Tests/` appear in solution: `dotnet sln Treinou.slnx list` shows both
- [ ] No files outside the `tests/` directory are modified (`git status`)
- [ ] `plans/README.md` status row updated to DONE

## STOP conditions

Stop and report back (do not improvise) if:

- `TreinouDbContext` does not have a constructor accepting `DbContextOptions<TreinouDbContext>` — read `src/Treinou.Infraestructure/TreinouDbContext.cs` before writing the fixture.
- `dotnet new xunit` fails (SDK version mismatch) — report the SDK version and the error.
- EF Core InMemory provider raises a query error for a specific search test (InMemory has limited LINQ translation) — comment out the failing test and report it as a STOP condition rather than changing the search query.
- Package versions suggested in this plan are not available — use `dotnet add package <name>` without a version and let NuGet resolve the latest compatible version, then report what was installed.
- CREF value object constructor signature differs from what the test assumes — read `src/Treinou.Domain/ValueObjects/CREF.cs` before writing tests that use it.

## Maintenance notes

- Each test class creates its own InMemory database (unique name via `Guid.NewGuid()`) — tests are isolated and can run in parallel.
- The EF Core InMemory provider does not enforce referential integrity the same way SQL Server does. Tests that rely on cascade delete behavior may behave differently from production. Where in doubt, add a note in the test.
- When Plans 001–005 land, add regression tests that specifically cover the bugs they fixed (the test plan sections in each plan identify exactly what to add).
- This plan covers the Domain and Infrastructure layers only. API integration tests (requiring `WebApplicationFactory<Program>` and a test identity user) are deferred — they are harder to set up and lower leverage than domain tests.
