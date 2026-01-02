# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Treinou is a workout management system built with .NET 10.0 following Domain-Driven Design (DDD) principles and Clean Architecture. The application manages relationships between Teachers (personal trainers with CREF certification) and Students, including workout programs and exercises.

## Solution Structure

The solution uses a layered architecture with clear separation of concerns:

- **Treinou.Domain**: Core domain logic, entities, value objects, and repository interfaces
- **Treinou.Infrastructure**: Data access implementation using Entity Framework Core (EF Core 10.0) with SQL Server support
- **Treinou.Application**: Application services implementing Use Cases with MediatR pattern and Adapter pattern for DTO conversions
- **Treinou.API**: ASP.NET Core Web API with Swagger/OpenAPI support
- **Treinou.Console**: Console application entry point
- **tests/**: Test projects (currently empty)

## Build and Run Commands

```bash
# Build the entire solution
dotnet build Treinou.slnx

# Build specific project
dotnet build src/Treinou.API/Treinou.API.csproj

# Run the API (recommended entry point)
dotnet run --project src/Treinou.API/Treinou.API.csproj
# Swagger UI available at: https://localhost:<port>/swagger

# Run the console application
dotnet run --project src/Treinou.Console/Treinou.Console.csproj

# Restore NuGet packages
dotnet restore
```

## Domain Architecture

### DDD Building Blocks (src/Treinou.Domain/SeedWork)

The project implements DDD tactical patterns:

- **Entity**: Base class for entities with `Guid` identity (auto-generated in constructor)
- **AggregateRoot**: Marker class extending Entity, defines transactional consistency boundaries
- **ValueObject**: Abstract base class with equality comparison and operator overloading
- **IRepository**: Marker interface for repositories
- **IGenericRepository<T>**: Generic repository interface with CRUD operations (Insert, Get, Update, Delete) and query ordering
- **ISearchableRepository<T>**: Repository interface for paginated search with SearchInput/SearchOutput pattern
- **IUnitOfWork**: Pattern for managing transactions with `Commit` and `Rollback` methods (implemented in infrastructure layer)

### Aggregate Roots

**Teacher** (src/Treinou.Domain/Entities/Teacher.cs)
- Aggregate root for personal trainers
- Has collection navigation to Students and Workouts
- Contains ValueObjects: Email, CPF, CREF (professional certification), PhoneNumber
- Self-validates using DomainValidation helper
- Includes `Update()` method for modifications

**Student** (src/Treinou.Domain/Entities/Student.cs)
- Aggregate root for students/clients
- Foreign key relationship to Teacher (TeacherId)
- Contains ValueObjects: Email, CPF, PhoneNumber
- Properties: Weight, Height, IsActive status
- Methods: `Update()`, `Activate()`, `Deactivate()`, and `Validate()`

**Workout** (src/Treinou.Domain/Entities/Workout.cs)
- Aggregate root for workout programs
- Foreign keys to both Teacher and Student
- Contains private List of WorkoutExercise (encapsulated collection exposed as IReadOnlyCollection)
- Methods: `Activate()`, `Deactivate()`, `Update()`, `RemoveExercise()`

**Exercise** (src/Treinou.Domain/Entities/Exercise.cs)
- Aggregate root for individual exercises
- Foreign key relationship to ExerciseType
- Properties: Name, ImageUrl (optional), IsActive, CreatedAt
- Methods: `Activate()`, `Deactivate()`, `Update()`
- Self-validates using DomainValidation helper

**ExerciseType** (src/Treinou.Domain/Entities/ExerciseType.cs)
- Aggregate root for exercise categories/types
- Has collection navigation to Exercises
- Properties: Name
- Methods: `Update()`, `Validate()`

**WorkoutExercise** (src/Treinou.Domain/Entities/WorkoutExercise.cs)
- Aggregate root representing the association between Workout and Exercise with training parameters
- Foreign keys to both Exercise and Workout
- Properties: Order, NumberOfSets, NumberOfRepetitions, Rest (TimeSpan), Notes
- Methods: `Update()`, `Validate()`
- Validates that Order, NumberOfSets, NumberOfRepetitions, and Rest are greater than zero

### Value Objects (src/Treinou.Domain/ValueObjects)

All value objects inherit from ValueObject base class and implement:
- `Equals(ValueObject? other)` for value-based equality
- `GetCustomHashCode()` for hashing

Current value objects:
- **Email**: Contains Address property
- **CPF**: Brazilian taxpayer ID, contains Number property
- **CREF**: Physical education professional certification
- **PhoneNumber**: Contains Number property

### Validation

Domain validation is centralized in `src/Treinou.Domain/Validation/DomainValidation.cs`. Entities call `Validate()` in constructors and mutation methods, throwing `EntityValidationException` on failure.

### Exceptions

- **EntityValidationException** (src/Treinou.Domain/Exceptions/EntityValidationException.cs): Domain validation failures
- **NotFoundException** (src/Treinou.Domain/Exceptions/NotFoundException.cs): Entity not found in repository queries

## Infrastructure Layer

### Database Context (src/Treinou.Infraestructure/TreinouDbContext.cs)

- Inherits from EF Core `DbContext`
- DbSets: Students, Teachers
- Applies entity configurations via `IEntityTypeConfiguration<T>`

### Entity Configurations (src/Treinou.Infraestructure/Configurations)

EF Core configurations use Fluent API:

- **StudentConfiguration**: Maps value objects using `OwnsOne()` pattern, flattening Email, CPF, and PhoneNumber into single columns
- **TeacherConfiguration**: Similar pattern for Teacher value objects
- All configurations define primary keys, required fields, max lengths, and navigation properties

### Repositories (src/Treinou.Infraestructure/Repositories)

Repository implementations inject `TreinouDbContext` and implement both `IGenericRepository<T>` and `ISearchableRepository<T>`:
- `Get(Guid id)`: Returns entity or throws NotFoundException
- `Insert(T aggregate)`: Adds to DbSet
- `Update(T aggregate)`: Marks as modified
- `Delete(T aggregate)`: Removes from DbSet
- `Search(SearchInput input)`: Returns paginated results with SearchOutput pattern

Available repositories: StudentRepository, TeacherRepository, WorkoutRepository, WorkoutExerciseRepository, ExerciseRepository, ExerciseTypeRepository

Note: Repositories don't call `SaveChanges()` directly - this follows Unit of Work pattern.

### Database Providers

The infrastructure project references:
- Microsoft.EntityFrameworkCore.SqlServer (production)
- Microsoft.EntityFrameworkCore.InMemory (testing)

## Application Layer

### Use Cases Pattern (src/Treinou.Application/UseCases)

The application layer implements the Use Case pattern with MediatR for CQRS-style operations. Each use case follows this structure:

- **Interface**: `I{Operation}{Entity}` (e.g., `ICreateStudent`, `IGetStudent`)
- **Implementation**: `{Operation}{Entity}` implementing `IRequestHandler<TInput, TOutput>`
- **Input**: `{Operation}{Entity}Input` as MediatR request
- **Output**: `{Entity}ModelOutput` as MediatR response

Use cases organized by entity:
- **Student**: Create, Get, Update, Delete, List (paginated)
- **Teacher**: Create, Get, Update, Delete
- **Workout**: Create, Get, Update, Delete
- **WorkoutExercise**: Create, Get, Update, Delete
- **Exercise**: Create, Get, Update, Delete, List (paginated)
- **ExerciseType**: Create, Get, Update, Delete, List (paginated)

### Adapter Pattern (src/Treinou.Application/Adapters)

Adapters convert between domain entities and DTOs:
- `ToEntity()`: Converts Input DTO to Domain Entity
- `ToOutput()`: Converts Domain Entity to Output DTO

Available adapters: StudentAdapter, TeacherAdapter, WorkoutAdapter, ExerciseAdapter

### Common Patterns

- **PaginatedListInput/Output**: Base classes for paginated queries with page, per_page, search, and sort parameters
- All use cases inject required repositories and `IUnitOfWork`
- Use cases call `await _unitOfWork.Commit(cancellationToken)` after repository operations

## API Layer

### Controllers (src/Treinou.API/Controllers)

ASP.NET Core Web API controllers use MediatR to dispatch requests to use cases:
- Inject `IMediator` via constructor
- Use standard RESTful conventions (GET, POST, PUT, DELETE)
- Return `APIResponse<T>` wrapper for successful responses
- Controllers: StudentController, TeacherController (others TBD)

### Exception Handling (src/Treinou.API/Filters/APIGlobalExceptionFilter.cs)

Global exception filter maps domain exceptions to HTTP status codes:
- `EntityValidationException` → 422 Unprocessable Entity
- `NotFoundException` → 404 Not Found
- Other exceptions → 422 Unprocessable Entity
- Stack traces included in Development environment only

### API Configuration

- **JSON Serialization**: Uses snake_case via custom `JsonSnakeCasePolicy`
- **Swagger/OpenAPI**: Configured with Swashbuckle
- **Dependency Injection**: UseCasesConfiguration registers all MediatR handlers and repositories
- **Database**: ConnectionsConfiguration sets up DbContext with connection string from appsettings.json

### API Response Format

All successful responses wrapped in `APIResponse<T>`:
```csharp
{
  "data": { /* entity data */ }
}
```

Error responses use standard ProblemDetails with `Type`, `Title`, `Status`, and `Detail` properties.

## Development Notes

### Entity Framework Core Migrations

When modifying entities or configurations:

```bash
# Add migration
dotnet ef migrations add MigrationName --project src/Treinou.Infraestructure --startup-project src/Treinou.API

# Update database
dotnet ef database update --project src/Treinou.Infraestructure --startup-project src/Treinou.API

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Treinou.Infraestructure --startup-project src/Treinou.API
```

### Naming Conventions

- Entities and aggregates use PascalCase
- Private fields use underscore prefix (e.g., `_exercises` in Workout)
- Constants use UPPER_SNAKE_CASE (e.g., `NAME_MAX_LENGTH`)
- Database columns for value objects are flattened (e.g., Email.Address → "Email" column)

### Important Patterns

1. **Aggregate Invariants**: Entities validate themselves in constructors and update methods
2. **Encapsulated Collections**: Use private List with public IReadOnlyCollection (see Workout.Exercises)
3. **Value Object Ownership**: EF Core maps value objects as owned types, not separate tables
4. **Repository Abstraction**: Domain defines interfaces, infrastructure implements them
5. **Use Case Pattern**: Each operation is a separate use case class implementing MediatR's IRequestHandler
6. **Adapter Pattern**: Domain entities and DTOs are converted via dedicated Adapter classes
7. **Unit of Work**: All use cases explicitly commit transactions via IUnitOfWork.Commit()
8. **Global Exception Handling**: APIGlobalExceptionFilter centralizes exception-to-HTTP mapping

## Key Architectural Decisions

- **No public setters in most entities**: All state changes go through methods to maintain invariants (except ExerciseType which uses public setters)
- **Teacher as aggregate root**: Students reference Teacher, but Teacher owns the relationship
- **Value objects for domain concepts**: Email, CPF, CREF, PhoneNumber are not primitives
- **MediatR for decoupling**: Controllers don't directly depend on use case implementations
- **API as primary entry point**: Web API with Swagger is the main interface (Console app for legacy/testing)
- **Snake case API**: JSON responses use snake_case convention via custom policy
- **Searchable repositories**: All repositories support both direct access and paginated search
