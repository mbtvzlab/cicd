---
name: ef
description: Design Entity Framework models, DbContext configuration, repository patterns, and data access layer for ASP.NET Core MVC projects
compatibility: opencode
metadata:
  stack: aspnet-core
  layer: data
---

## What I do

- Design EF Core entity models with navigation properties and appropriate data annotations
- Configure DbContext with Fluent API for relationships, constraints, and indexes
- Implement repository pattern for decoupled data access
- Set up migrations and seed data via `OnModelCreating`
- Optimize queries with eager loading (`.Include()`/`.ThenInclude()`), `AsNoTracking`, and projection

## When to use me

Use this skill when you are building or modifying the data access layer—adding entities, configuring relationships, writing repositories, or optimizing database queries.

## Conventions for this project

- Entity models live in `Models/`
- DbContext lives in `Data/CiCdDbContext.cs`
- Repository interfaces live in `Data/` (e.g. `IProjectRepository`, `IUserRepository`)
- EF repository implementations live in `Data/` (e.g. `EfProjectRepository`, `EfUserRepository`)
- Register repositories as **scoped** in `Program.cs` via `builder.Services.AddScoped<IFooRepository, EfFooRepository>()`
- Register `CiCdDbContext` with `UseNpgsql` in `Program.cs`
- Entity models use `[Key]`, `[Required]`, `[ForeignKey]`, `[NotMapped]`, `[MaxLength]` data annotations
- Navigation properties use `virtual` keyword and `ICollection<T>` for collections
- Explicit foreign key properties (e.g. `OwnerId`, `ProjectId`) on all navigation properties
- Computed properties (like `Duration`) are marked `[NotMapped]`
- PostgreSQL connection string in `appsettings.json` under `"ConnectionStrings": { "CiCdDbContext": "..." }`
- Seed data in `OnModelCreating` using `HasData()` for stable migrations
- Run `dotnet ef migrations add <Name>` and `dotnet ef database update` to apply schema changes
