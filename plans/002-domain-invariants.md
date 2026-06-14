# Plan 002: Fix Domain Invariant Violations in Entity Constructors and Update Methods

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan
> in `plans/README.md`.
>
> **Drift check (run first)**:
> ```
> git diff --stat 8937b9b..HEAD -- src/Treinou.Domain/Entities/Exercise.cs src/Treinou.Domain/Entities/Teacher.cs src/Treinou.Domain/Entities/Workout.cs src/Treinou.Domain/Entities/WorkoutExercise.cs
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

Four domain invariant bugs allow invalid state to be created or updated without error:

1. `Exercise` constructor ignores the `isActive` parameter and always sets `IsActive = true`. Any caller passing `isActive: false` is silently overridden, making deactivated exercise creation impossible through the constructor.
2. `Teacher` constructor ignores the `createdAt` parameter and always sets `CreatedAt = DateTime.Now`. Historical imports, audit trails, and testability are all broken.
3. `Workout.Update()` does not call `Validate()` — a workout's name can be set to empty string and persisted without error. Every other entity's `Update()` method calls `Validate()`.
4. `WorkoutExercise.Update()` does not call `Validate()` — Order, NumberOfSets, NumberOfRepetitions, and Rest can be set to zero or negative values and persisted, violating the invariants enforced in the constructor.

All four fixes are one-line changes. None require schema or API changes.

## Current state

### `src/Treinou.Domain/Entities/Exercise.cs` (bug at line 29)

```csharp
public Exercise(
    string name,
    Guid exerciseTypeId,
    bool isActive = true,   // ← parameter accepted
    string? imageUrl = null
)
{
    Name = name;
    ExcerciseTypeId = exerciseTypeId;
    IsActive = true;         // ← BUG: parameter ignored, always true
    CreatedAt = DateTime.Now;
    ImageUrl = imageUrl;
    Validate();
}
```

### `src/Treinou.Domain/Entities/Teacher.cs` (bug at line 32)

```csharp
public Teacher(
    string name,
    Email email,
    CPF cpf,
    CREF cref,
    string description,
    PhoneNumber phoneNumber,
    DateTime birthDate,
    DateTime createdAt    // ← parameter accepted
)
{
    Name = name;
    // ... other assignments ...
    BirthDate = birthDate;
    CreatedAt = DateTime.Now;  // ← BUG: parameter ignored, always current time
    Validate();
}
```

### `src/Treinou.Domain/Entities/Workout.cs` (bug at lines 51–60)

```csharp
public void Update(
    string name,
    Guid teacherId,
    Guid studentId
)
{
    Name = name;
    TeacherId = teacherId;
    StudentId = studentId;
    // ← BUG: no Validate() call here
}
```

Contrast with `Activate()` (line 34) which does call `Validate()`:
```csharp
public void Activate()
{
    IsActive = true;
    Validate();   // ← correct pattern
}
```

`Validate()` on `Workout` (line 46):
```csharp
private void Validate()
{
    DomainValidation.NotNullOrEmpty(Name, nameof(Name));
}
```

### `src/Treinou.Domain/Entities/WorkoutExercise.cs` (bug at lines 67–80)

```csharp
public void Update(
    int order,
    int numberOfSets,
    int numberOfRepetitions,
    TimeSpan rest,
    string? notes = null
)
{
    Order = order;
    NumberOfSets = numberOfSets;
    NumberOfRepetitions = numberOfRepetitions;
    Rest = rest;
    Notes = notes ?? string.Empty;
    // ← BUG: no Validate() call here
}
```

`Validate()` on `WorkoutExercise` (line 50) enforces:
```csharp
public void Validate()
{
    DomainValidation.NotNull(ExerciseId, nameof(ExerciseId));
    if (Order <= 0) throw new EntityValidationException("Order should be greater than zero.");
    if (NumberOfSets <= 0) throw new EntityValidationException(...);
    if (NumberOfRepetitions <= 0) throw new EntityValidationException(...);
    if (Rest.TotalMilliseconds <= 0) throw new EntityValidationException(...);
}
```

### Repo conventions to match

- All entity validation is performed by calling `Validate()` on `this` from constructors and mutation methods — see `Student.Update()` (line 91), `Teacher.Update()` (line 90), `Exercise.Update()` (line 59).
- `DomainValidation` is a static helper in `src/Treinou.Domain/Validation/DomainValidation.cs`.
- `EntityValidationException` is the domain exception type in `src/Treinou.Domain/Exceptions/EntityValidationException.cs`.

## Commands you will need

| Purpose     | Command                         | Expected on success |
|-------------|---------------------------------|---------------------|
| Build       | `dotnet build Treinou.slnx`     | exit 0, no errors   |

## Scope

**In scope** (only these files):
- `src/Treinou.Domain/Entities/Exercise.cs`
- `src/Treinou.Domain/Entities/Teacher.cs`
- `src/Treinou.Domain/Entities/Workout.cs`
- `src/Treinou.Domain/Entities/WorkoutExercise.cs`

**Out of scope** (do NOT touch):
- `ExerciseType.cs` — its `Update()` already calls `Validate()` (verified)
- `Student.cs` — its `Update()` already calls `Validate()` (verified)
- Repository or use case files — no changes needed there
- EF Core configurations — no schema impact

## Git workflow

- Branch: `advisor/002-domain-invariants`
- One commit for all four fixes: `fix: honor entity constructor parameters and add missing Validate() calls`
- Do NOT push or open a PR unless instructed

## Steps

### Step 1: Fix `Exercise.IsActive` constructor bug

In `src/Treinou.Domain/Entities/Exercise.cs`, find the constructor body.

Change line 29:
```csharp
IsActive = true;
```
To:
```csharp
IsActive = isActive;
```

**Verify**: `dotnet build Treinou.slnx` → exit 0  
**Verify**: `grep -n "IsActive = true" src/Treinou.Domain/Entities/Exercise.cs` → no output (the hardcoded `true` should be gone; `Activate()` uses assignment not literal but double-check)

### Step 2: Fix `Teacher.CreatedAt` constructor bug

In `src/Treinou.Domain/Entities/Teacher.cs`, find the constructor body (around line 32).

Change:
```csharp
CreatedAt = DateTime.Now;
```
To:
```csharp
CreatedAt = createdAt;
```

**Verify**: `dotnet build Treinou.slnx` → exit 0  
**Verify**: `grep -n "DateTime.Now" src/Treinou.Domain/Entities/Teacher.cs` → no output

### Step 3: Add `Validate()` to `Workout.Update()`

In `src/Treinou.Domain/Entities/Workout.cs`, find the `Update()` method (lines 51–60).

Add `Validate();` as the last line of the method body, after `StudentId = studentId;`:
```csharp
public void Update(
    string name,
    Guid teacherId,
    Guid studentId
)
{
    Name = name;
    TeacherId = teacherId;
    StudentId = studentId;
    Validate();    // ← add this line
}
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 4: Add `Validate()` to `WorkoutExercise.Update()`

In `src/Treinou.Domain/Entities/WorkoutExercise.cs`, find the `Update()` method (lines 67–80).

Add `Validate();` as the last line of the method body, after `Notes = notes ?? string.Empty;`:
```csharp
public void Update(
    int order,
    int numberOfSets,
    int numberOfRepetitions,
    TimeSpan rest,
    string? notes = null
)
{
    Order = order;
    NumberOfSets = numberOfSets;
    NumberOfRepetitions = numberOfRepetitions;
    Rest = rest;
    Notes = notes ?? string.Empty;
    Validate();    // ← add this line
}
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

## Test plan

No test project exists yet (Plan 006 creates it). When it lands, add the following to `tests/Treinou.Domain.Tests/Entities/ExerciseTests.cs`:
```csharp
[Fact]
public void Constructor_ShouldRespectIsActiveParameter()
{
    var exercise = new Exercise("Squat", Guid.NewGuid(), isActive: false);
    Assert.False(exercise.IsActive);
}
```

And for `WorkoutExercise`:
```csharp
[Fact]
public void Update_ShouldThrow_WhenOrderIsZero()
{
    var we = new WorkoutExercise(Guid.NewGuid(), Guid.NewGuid(), 1, 3, 10, TimeSpan.FromSeconds(60), "");
    Assert.Throws<EntityValidationException>(() =>
        we.Update(order: 0, numberOfSets: 3, numberOfRepetitions: 10, rest: TimeSpan.FromSeconds(60)));
}
```

## Done criteria

- [ ] `dotnet build Treinou.slnx` exits 0
- [ ] `grep -n "IsActive = true" src/Treinou.Domain/Entities/Exercise.cs` returns no output (the constructor no longer hardcodes `true`)
- [ ] `grep -n "DateTime.Now" src/Treinou.Domain/Entities/Teacher.cs` returns no output
- [ ] `grep -n "Validate" src/Treinou.Domain/Entities/Workout.cs` shows `Validate()` called in both `Activate`, `Deactivate`, and now `Update`
- [ ] `grep -n "Validate" src/Treinou.Domain/Entities/WorkoutExercise.cs` shows `Validate()` called in constructor and now `Update`
- [ ] No files outside the in-scope list are modified (`git status`)
- [ ] `plans/README.md` status row updated to DONE

## STOP conditions

Stop and report back (do not improvise) if:

- The code at the locations in "Current state" doesn't match the excerpts (drift check failed).
- `dotnet build` fails after any step and you cannot identify a clear resolution.
- You discover that callers of `Workout.Update()` or `WorkoutExercise.Update()` are intentionally passing values that would now fail validation (look for callers that pass empty strings or zero — if found, stop and report before changing the entity).

## Maintenance notes

- When adding new entities, the pattern is: constructor calls `Validate()`, every mutation method (`Update`, `Activate`, `Deactivate`) calls `Validate()` at the end.
- `Teacher.createdAt` parameter was intentionally left as a constructor parameter to allow historical imports. If the team decides creation timestamp should always be "now", remove the parameter and use `DateTime.UtcNow` directly — also see Plan 005's note on `DateTime.Now` vs `DateTime.UtcNow`.
