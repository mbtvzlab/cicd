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
- Implement repository and unit-of-work patterns for decoupled data access
- Set up migrations and seed data strategies
- Optimize queries with eager/lazy/explicit loading, AsNoTracking, and projection

## When to use me

Use this skill when you are building or modifying the data access layer—adding entities, configuring relationships, writing repositories, or optimizing database queries.

## Conventions for this project

- Entity models live in `Models/`
- Repository interfaces live in `Data/` (e.g. `IProjectRepository`, `IPipelineRepository`)
- Mock repositories live in `Data/` (e.g. `ProjectMockRepository`)
- Register repositories as singletons in `Program.cs` via `builder.Services.AddSingleton<IFooRepository, FooMockRepository>()`
- Navigation properties are initialized with `null!` for required references and `[]` for collections
- Default string values use `""` rather than nullable strings
