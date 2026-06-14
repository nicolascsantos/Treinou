# Plan 004: Require Authentication on All Resource Endpoints

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan
> in `plans/README.md`.
>
> **Drift check (run first)**:
> ```
> git diff --stat 8937b9b..HEAD -- src/Treinou.API/Controllers/StudentController.cs src/Treinou.API/Controllers/TeacherController.cs src/Treinou.API/Controllers/WorkoutController.cs src/Treinou.API/Controllers/WorkoutExerciseController.cs src/Treinou.API/Controllers/ExerciseController.cs src/Treinou.API/Controllers/ExerciseTypeController.cs src/Treinou.API/Program.cs
> ```
> If any in-scope file changed since this plan was written, compare the
> "Current state" excerpts against the live code before proceeding; on a
> mismatch, treat it as a STOP condition.

## Status

- **Priority**: P1
- **Effort**: M
- **Risk**: LOW
- **Depends on**: none (can land independently; Plan 003 is complementary but not required)
- **Category**: security
- **Planned at**: commit `8937b9b`, 2026-06-14

## Why this matters

All six resource controllers (Student, Teacher, Workout, WorkoutExercise, Exercise, ExerciseType) are publicly accessible without any authentication. `UseAuthentication()` and `UseAuthorization()` are registered in `Program.cs` and the ASP.NET Core Identity pipeline is configured via `MapIdentityApi<ApplicationUser>()`, but no endpoint requires a valid token or session. Any anonymous HTTP client can enumerate, create, modify, and delete all data in the system.

This plan adds `[Authorize]` at the controller class level to all six resource controllers, which is the minimum viable fix. It does **not** add ownership checks or role-based access (those belong in a future RBAC plan once identity is linked to domain entities). After this plan lands, callers must be authenticated to use any resource endpoint; the Identity API endpoints (`/login`, `/register`) remain public as intended.

## Current state

### `src/Treinou.API/Program.cs` — middleware registration (lines 26–29)

```csharp
app.UseAuthentication();   // ← present
app.UseAuthorization();    // ← present
app.MapControllers();
```

Authentication and authorization middleware are correctly placed before `MapControllers()`. No change needed here.

### All 6 resource controllers — no `[Authorize]`

Each controller currently looks like:
```csharp
[Route("api/[controller]")]
[ApiController]                 // ← only attribute; no [Authorize]
public class StudentController : ControllerBase
{
    // ... all endpoints open to anonymous callers
}
```

Files and their current class-level attributes:
- `src/Treinou.API/Controllers/StudentController.cs:14-17` — `[Route]`, `[ApiController]`, no Authorize
- `src/Treinou.API/Controllers/TeacherController.cs` — same
- `src/Treinou.API/Controllers/WorkoutController.cs:13-16` — same
- `src/Treinou.API/Controllers/WorkoutExerciseController.cs` — same
- `src/Treinou.API/Controllers/ExerciseController.cs` — same
- `src/Treinou.API/Controllers/ExerciseTypeController.cs:13-17` — same

### `src/Treinou.API/Controllers/AuthController.cs`

The `AuthController` must **not** get `[Authorize]` — its `Register` endpoint must remain public. Do not touch this file.

### How ASP.NET Core Identity authentication works in this project

`Program.cs` calls `AddIdentityApiEndpoints<ApplicationUser>()` and `MapIdentityApi<ApplicationUser>()`. This generates cookie-based or bearer token endpoints at `/login`, `/register`, etc. After a successful `/login`, the client receives a bearer token (or cookie). That token must be sent in the `Authorization: Bearer <token>` header (or cookie) on subsequent requests.

`[Authorize]` with no policy argument requires only that the request is authenticated (any valid logged-in user). It does not check roles. This is the correct minimum for this plan.

### Repo conventions

Adding an attribute follows the pattern `[AttributeName]` on the class declaration. All controllers already use `[Route(...)]` and `[ApiController]` as class-level attributes. Add `[Authorize]` as a third class-level attribute. Add `using Microsoft.AspNetCore.Authorization;` to each file.

## Commands you will need

| Purpose       | Command                                                                       | Expected on success |
|---------------|-------------------------------------------------------------------------------|---------------------|
| Build         | `dotnet build Treinou.slnx`                                                   | exit 0, no errors   |
| Grep verify   | `grep -rn "\[Authorize\]" src/Treinou.API/Controllers/`                       | 6 matches (one per resource controller) |
| Confirm Auth  | `grep -n "Authorize" src/Treinou.API/Controllers/AuthController.cs`           | no output           |

## Scope

**In scope** (only these files — class-level attribute addition only):
- `src/Treinou.API/Controllers/StudentController.cs`
- `src/Treinou.API/Controllers/TeacherController.cs`
- `src/Treinou.API/Controllers/WorkoutController.cs`
- `src/Treinou.API/Controllers/WorkoutExerciseController.cs`
- `src/Treinou.API/Controllers/ExerciseController.cs`
- `src/Treinou.API/Controllers/ExerciseTypeController.cs`

**Out of scope** (do NOT touch):
- `AuthController.cs` — must remain public
- `Program.cs` — middleware order is already correct, no change needed
- Any use case, repository, or domain file
- Individual action methods — apply `[Authorize]` at the class level only; do not add method-level attributes

## Git workflow

- Branch: `advisor/004-authorize-endpoints`
- One commit: `feat: require authentication on all resource endpoints`
- Do NOT push or open a PR unless instructed

## Steps

### Step 1: Add `[Authorize]` to `StudentController`

In `src/Treinou.API/Controllers/StudentController.cs`:

1. Add `using Microsoft.AspNetCore.Authorization;` to the using block at the top.
2. Add `[Authorize]` above the class declaration, alongside the existing attributes:

```csharp
[Route("api/[controller]")]
[ApiController]
[Authorize]                    // ← add this
public class StudentController : ControllerBase
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 2: Add `[Authorize]` to `TeacherController`

Same pattern as Step 1 in `src/Treinou.API/Controllers/TeacherController.cs`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 3: Add `[Authorize]` to `WorkoutController`

Same pattern in `src/Treinou.API/Controllers/WorkoutController.cs`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 4: Add `[Authorize]` to `WorkoutExerciseController`

Same pattern in `src/Treinou.API/Controllers/WorkoutExerciseController.cs`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 5: Add `[Authorize]` to `ExerciseController`

Same pattern in `src/Treinou.API/Controllers/ExerciseController.cs`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 6: Add `[Authorize]` to `ExerciseTypeController`

Same pattern in `src/Treinou.API/Controllers/ExerciseTypeController.cs`.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 7: Verify all 6 controllers have `[Authorize]` and `AuthController` does not

```bash
grep -rn "\[Authorize\]" src/Treinou.API/Controllers/
```

Expected output: exactly 6 lines, one for each resource controller. `AuthController.cs` must not appear.

```bash
grep -n "Authorize" src/Treinou.API/Controllers/AuthController.cs
```

Expected: no output.

**Final build verify**: `dotnet build Treinou.slnx` → exit 0

## Test plan

No test project exists yet (Plan 006 creates it). When it lands, add integration tests:

- `StudentController_Get_ShouldReturn401_WhenNotAuthenticated()` — unauthenticated GET → 401
- `StudentController_Get_ShouldReturn200_WhenAuthenticated()` — authenticated GET with valid bearer → 200
- Repeat for at least one endpoint on each controller

These tests require a `WebApplicationFactory<Program>` setup and an Identity test user. Use the EF Core InMemory provider (already referenced in `Treinou.Infraestructure.csproj`).

## Done criteria

- [ ] `dotnet build Treinou.slnx` exits 0
- [ ] `grep -rn "\[Authorize\]" src/Treinou.API/Controllers/` returns exactly 6 lines
- [ ] `grep -n "Authorize" src/Treinou.API/Controllers/AuthController.cs` returns no output
- [ ] Manual smoke test: an unauthenticated GET to any resource endpoint returns 401 (run the app and test with curl or Swagger)
- [ ] No files outside the in-scope list are modified (`git status`)
- [ ] `plans/README.md` status row updated to DONE

## STOP conditions

Stop and report back (do not improvise) if:

- After adding `[Authorize]`, the app returns 403 instead of 401 for unauthenticated requests — this indicates a misconfigured default policy or a cookie-vs-bearer conflict; investigate before continuing.
- Any controller action already has a `[AllowAnonymous]` attribute (would be unusual but would interfere) — report before proceeding.
- The build fails because `Microsoft.AspNetCore.Authorization` is not available in a project — check package references and report.

## Maintenance notes

- This plan enforces authentication only (any logged-in user). Ownership checks (student can only see their own data) and role-based access (Teacher vs. Student) are the next step and belong in a separate RBAC plan.
- When adding new controllers in the future, always add `[Authorize]` at the class level by default; add `[AllowAnonymous]` only to explicitly public endpoints.
- `MapIdentityApi<ApplicationUser>()` in `Program.cs` maps the built-in Identity endpoints without `[Authorize]`; those stay public by design.
