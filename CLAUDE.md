# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Treinou is a workout management system built with .NET 10.0 following Domain-Driven Design (DDD) principles and Clean Architecture. The application manages relationships between Teachers (personal trainers with CREF certification) and Students, including workout programs and exercises.

## Solution Structure

The solution uses a layered architecture with clear separation of concerns:

- **Treinou.Domain**: Core domain logic, entities, value objects, and repository interfaces
- **Treinou.Infrastructure**: Data access implementation using Entity Framework Core (EF Core 10.0) with SQL Server support
- **Treinou.Application**: Application services and business orchestration (currently minimal)
- **Treinou.Console**: Console application entry point
- **tests/**: Test projects (currently empty)

## Build and Run Commands

```bash
# Build the entire solution
dotnet build Treinou.slnx

# Build specific project
dotnet build src/Treinou.Console/Treinou.Console.csproj

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
- **IUnitOfWork**: Pattern for managing transactions with `Commit` and `Rollback` methods

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

Repository implementations inject `TreinouDbContext` and expose:
- `Get(Guid id)`: Returns entity or throws NotFoundException
- `Insert(T aggregate)`: Adds to DbSet
- `Update(T aggregate)`: Marks as modified
- `Delete(T aggregate)`: Removes from DbSet

Note: Repositories don't call `SaveChanges()` directly - this follows Unit of Work pattern (though UoW implementation is pending).

### Database Providers

The infrastructure project references:
- Microsoft.EntityFrameworkCore.SqlServer (production)
- Microsoft.EntityFrameworkCore.InMemory (testing)

## Development Notes

### Entity Framework Core Migrations

When modifying entities or configurations:

```bash
# Add migration
dotnet ef migrations add MigrationName --project src/Treinou.Infraestructure --startup-project src/Treinou.Console

# Update database
dotnet ef database update --project src/Treinou.Infraestructure --startup-project src/Treinou.Console

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Treinou.Infraestructure --startup-project src/Treinou.Console
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

### Recent Changes (Git History)

- Added Workout aggregate with Exercise relationships
- Implemented Student and Teacher repositories
- Created infrastructure project with EF Core setup
- Established Teacher-Student relationship

## Key Architectural Decisions

- **No public setters in Workout**: All state changes go through methods to maintain invariants
- **Teacher as aggregate root**: Students reference Teacher, but Teacher owns the relationship
- **Value objects for domain concepts**: Email, CPF, CREF, PhoneNumber are not primitives
- **Separate read/write in repositories**: Repositories provide typed access to aggregates only
