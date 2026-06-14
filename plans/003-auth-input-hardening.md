# Plan 003: Harden Auth Inputs and Fix Error Response Disclosures

> **Executor instructions**: Follow this plan step by step. Run every
> verification command and confirm the expected result before moving to the
> next step. If anything in the "STOP conditions" section occurs, stop and
> report — do not improvise. When done, update the status row for this plan
> in `plans/README.md`.
>
> **Drift check (run first)**:
> ```
> git diff --stat 8937b9b..HEAD -- src/Treinou.Application/UseCases/Auth/RegisterUserInput.cs src/Treinou.Application/UseCases/Auth/RegisterUser.cs src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudentInput.cs src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudent.cs src/Treinou.API/Filters/APIGlobalExceptionFilter.cs
> ```
> If any in-scope file changed since this plan was written, compare the
> "Current state" excerpts against the live code before proceeding; on a
> mismatch, treat it as a STOP condition.

## Status

- **Priority**: P1
- **Effort**: S
- **Risk**: LOW
- **Depends on**: none
- **Category**: security
- **Planned at**: commit `8937b9b`, 2026-06-14

## Why this matters

Four security issues in auth and error handling:

1. **Privilege escalation via UserType**: `RegisterUserInput` exposes `UserType` as a JSON-deserializable field. Any caller can POST `{"email": "...", "password": "...", "user_type": "Teacher"}` and self-assign the Teacher role. All newly registered users should default to `Student`; elevation to Teacher must go through a separate admin flow.
2. **Account-link hijacking via UserId**: `UpdateStudentInput` exposes `UserId` as a client-supplied field. `UpdateStudent` handler assigns it directly: `studentToBeUpdated.UserId = request.UserId`. Any caller can point a Student record at a different identity account — or null it out — without any authorization check.
3. **Identity error details leaked to API clients**: `APIGlobalExceptionFilter` includes `exception.InnerException?.Message` in the response body for `AuthenticationException`. `RegisterUser` wraps Identity framework errors (e.g., "Passwords must have at least one non alphanumeric character") in an `AuthenticationException` inner exception. This leaks password policy details and can aid credential attacks.
4. **Misleading 404 error title**: `APIGlobalExceptionFilter` maps `NotFoundException` with title `"One or more validation errors occurred."` — this is the validation error title, not a not-found title. Clients that parse the `title` field to determine UX behavior will misinterpret a 404 as a validation failure.

## Current state

### `src/Treinou.Application/UseCases/Auth/RegisterUserInput.cs`

```csharp
public class RegisterUserInput : IRequest<RegisterUserOutput>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserType UserType { get; set; }   // ← client-supplied, remove this

    public RegisterUserInput(string email, string password, UserType userType)
    {
        Email = email;
        Password = password;
        UserType = userType;
    }
}
```

### `src/Treinou.Application/UseCases/Auth/RegisterUser.cs`

```csharp
public async Task<RegisterUserOutput> Handle(RegisterUserInput request, CancellationToken cancellationToken)
{
    var user = new ApplicationUser
    {
        UserName = request.Email,
        Email = request.Email,
        UserType = request.UserType   // ← will become hardcoded default
    };

    var result = await _userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded)
        throw new AuthenticationException(
            "Registration failed.",
            new Exception(string.Join("; ", result.Errors.Select(e => e.Description)))
        );

    return new RegisterUserOutput(user.Id, user.Email, user.UserType);
}
```

### `src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudentInput.cs` (line 22)

```csharp
public class UpdateStudentInput : IRequest<StudentModelOutput>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string CPF { get; set; }
    public string PhoneNumber { get; set; }
    public double Weight { get; set; }
    public double Height { get; set; }
    public string? UserId { get; set; }   // ← client-supplied, remove this

    public UpdateStudentInput(Guid id, string name, string email, string cpf,
        string phoneNumber, double weight, double height, string? userId = null)
    {
        // ...
        UserId = userId;   // ← remove
    }
}
```

### `src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudent.cs` (line 30)

```csharp
studentToBeUpdated.UserId = request.UserId;   // ← remove this line
```

### `src/Treinou.API/Filters/APIGlobalExceptionFilter.cs` (lines 49–55 and 33–39)

```csharp
// AuthenticationException handler — leaks inner message (lines 49-55):
else if (exception is AuthenticationException)
{
    details.Title = "An unexpected error occured.";
    details.Status = StatusCodes.Status422UnprocessableEntity;
    details.Type = "UnexpectedError";
    details.Detail = $"Message: {exception.Message};InnerException: {exception.InnerException?.Message}";
    //                                                ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
    //                                                leaks Identity error details to client
}

// NotFoundException handler — wrong title (lines 33-39):
else if (exception is NotFoundException)
{
    var ex = exception as NotFoundException;
    details.Title = "One or more validation errors occurred.";   // ← wrong
    details.Status = StatusCodes.Status404NotFound;
    details.Detail = ex!.Message;
    details.Type = "NotFound";
}
```

### `src/Treinou.Domain/Enums/UserType.cs` — `UserType` enum

This enum defines the available user types (e.g., `Teacher`, `Student`). The default new-user role is `Student`. Check the exact enum value name before hardcoding.

## Commands you will need

| Purpose  | Command                         | Expected on success |
|----------|---------------------------------|---------------------|
| Build    | `dotnet build Treinou.slnx`     | exit 0, no errors   |

## Scope

**In scope** (only these files):
- `src/Treinou.Application/UseCases/Auth/RegisterUserInput.cs`
- `src/Treinou.Application/UseCases/Auth/RegisterUser.cs`
- `src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudentInput.cs`
- `src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudent.cs`
- `src/Treinou.API/Filters/APIGlobalExceptionFilter.cs`

**Out of scope** (do NOT touch):
- `RegisterUserOutput.cs` — no change needed; it can keep returning UserType in the response
- `ApplicationUser.cs` — no change
- `AuthController.cs` — no change; the fix is in the input model
- Any other exception handler branches in `APIGlobalExceptionFilter.cs` beyond the two mentioned

## Git workflow

- Branch: `advisor/003-auth-input-hardening`
- Commit after each logical group; message style: `fix: <description>`
- Do NOT push or open a PR unless instructed

## Steps

### Step 1: Remove `UserType` from `RegisterUserInput`

In `src/Treinou.Application/UseCases/Auth/RegisterUserInput.cs`:

1. Remove the `[JsonConverter]` import if it becomes unused: `using System.Text.Json.Serialization;`
2. Remove the `UserType` property and its constructor parameter.
3. Remove the `using Treinou.Domain.Enums;` import if no longer needed.

The resulting class should look like:
```csharp
public class RegisterUserInput : IRequest<RegisterUserOutput>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public RegisterUserInput(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
```

**Verify**: `dotnet build Treinou.slnx` → expect a compilation error in `RegisterUser.cs` (references `request.UserType`) — this is expected and will be fixed in Step 2.

### Step 2: Default `UserType` to `Student` in `RegisterUser` handler

In `src/Treinou.Application/UseCases/Auth/RegisterUser.cs`:

1. Add `using Treinou.Domain.Enums;` if not already present.
2. Change `UserType = request.UserType` to `UserType = UserType.Student`.

The `ApplicationUser` initialization should look like:
```csharp
var user = new ApplicationUser
{
    UserName = request.Email,
    Email = request.Email,
    UserType = UserType.Student   // ← hardcoded default; teacher elevation requires a separate admin flow
};
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 3: Remove `UserId` from `UpdateStudentInput`

In `src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudentInput.cs`:

1. Remove the `public string? UserId { get; set; }` property.
2. Remove `string? userId = null` from the constructor parameters.
3. Remove `UserId = userId;` from the constructor body.

**Verify**: `dotnet build Treinou.slnx` → expect a compilation error in `UpdateStudent.cs` — expected, fixed in Step 4.

### Step 4: Remove `UserId` assignment from `UpdateStudent` handler

In `src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudent.cs`, remove line 30:
```csharp
studentToBeUpdated.UserId = request.UserId;   // ← delete this line
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 5: Fix `AuthenticationException` detail leak in exception filter

In `src/Treinou.API/Filters/APIGlobalExceptionFilter.cs`, find the `AuthenticationException` handler block (lines 49–55).

Change the `Detail` assignment from:
```csharp
details.Detail = $"Message: {exception.Message};InnerException: {exception.InnerException?.Message}";
```
To:
```csharp
details.Detail = exception.Message;
```

The `exception.Message` for `AuthenticationException` is the safe, generic string `"Registration failed."` set in `RegisterUser.cs`. The inner exception (which contains Identity error details) is no longer forwarded to the client. If server-side logging is configured, the inner exception will still be captured there.

**Verify**: `dotnet build Treinou.slnx` → exit 0

### Step 6: Fix `NotFoundException` title in exception filter

In `src/Treinou.API/Filters/APIGlobalExceptionFilter.cs`, find the `NotFoundException` handler block (lines 33–39).

Change:
```csharp
details.Title = "One or more validation errors occurred.";
```
To:
```csharp
details.Title = "Resource not found.";
```

**Verify**: `dotnet build Treinou.slnx` → exit 0

## Test plan

No test project exists yet (Plan 006 creates it). When it lands, add:

- `RegisterUser_ShouldDefaultUserTypeToStudent()` — register without specifying role; assert returned UserType is Student
- `UpdateStudent_ShouldNotModifyUserId()` — call UpdateStudent; assert student.UserId unchanged
- `APIGlobalExceptionFilter_NotFoundException_ShouldReturnResourceNotFoundTitle()`
- `APIGlobalExceptionFilter_AuthenticationException_ShouldNotLeakInnerMessage()`

## Done criteria

- [ ] `dotnet build Treinou.slnx` exits 0
- [ ] `grep -n "UserType" src/Treinou.Application/UseCases/Auth/RegisterUserInput.cs` returns no output
- [ ] `grep -n "UserId" src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudentInput.cs` returns no output
- [ ] `grep -n "request.UserId" src/Treinou.Application/UseCases/Student/UpdateStudent/UpdateStudent.cs` returns no output
- [ ] `grep -n "InnerException" src/Treinou.API/Filters/APIGlobalExceptionFilter.cs` returns no output
- [ ] `grep -n "validation errors" src/Treinou.API/Filters/APIGlobalExceptionFilter.cs` returns one match only (the `EntityValidationException` handler, not NotFoundException)
- [ ] No files outside the in-scope list are modified (`git status`)
- [ ] `plans/README.md` status row updated to DONE

## STOP conditions

Stop and report back (do not improvise) if:

- `UserType.Student` is not a valid enum value in `src/Treinou.Domain/Enums/UserType.cs` — read the file before Step 2 and confirm the exact member name.
- Removing `UserId` from `UpdateStudentInput` causes compilation errors in any file other than `UpdateStudent.cs` (would indicate other callers that need attention).
- The `AuthenticationException` inner exception in `RegisterUser.cs` contains the *only* useful error message (i.e., `exception.Message` is always empty) — if so, stop and report before removing it.

## Maintenance notes

- Teacher role assignment will be needed when Plan D1 (RBAC) is implemented. At that point, create a separate admin endpoint: `POST /api/admin/teachers` that creates an `ApplicationUser` with `UserType.Teacher` and requires an admin `[Authorize]` policy.
- The `AuthenticationException` pattern in `RegisterUser.cs` leaks error details into the inner exception by design — any future error thrown this way must use a generic outer message to stay safe after this plan lands.
- If more error titles need fixing in `APIGlobalExceptionFilter`, the pattern is: `EntityValidationException` → "One or more validation errors occurred.", `NotFoundException` → "Resource not found.", `UniqueConstraintException` → "Resource already exists."
