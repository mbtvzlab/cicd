---
name: list-page
description: Generate a list/index page for any entity in the CI/CD dashboard MVC application
compatibility: opencode
metadata:
  stack: aspnet-core
  layer: frontend
---

## What I do

- Generate a Razor View that displays a table of entity records
- Create a matching controller Index action that loads data via the repository pattern
- Include breadcrumbs, table with columns, status badges, and action links

## When to use me

Use this skill when you need a new list/index page for an entity that does not yet have one, or when you need to update an existing list page.

## Conventions for this project

- Views follow the dark-theme CSS variable system defined in `Views/Shared/_Layout.cshtml`
- Use `.breadcrumb`, `.breadcrumb-separator`, `.breadcrumb-current` for navigation breadcrumbs
- Use `.section-header` with `<h2>` and an optional action button (`.btn.btn-primary`)
- Use `.table-container > table` structure for data tables
- Use `.badge` variants (`.badge-success`, `.badge-failed`, `.badge-running`, `.badge-cancelled`) for status indicators
- Use `.text-secondary` and `.text-muted` for secondary/muted text
- Controller actions use the repository pattern: inject `IXxxRepository`, call `GetAll()`, pass to `View()`
- For attribute-routed controllers, use `[HttpGet("")]` on the Index action
- Navigation links go in the sidebar (`_Layout.cshtml`) using `@Url.Action("Index", "Controller")`
- Active nav link detection: `@(ViewContext.RouteData.Values["Controller"]?.ToString() == "Xxx" ? "active" : "")`
- Models are in `Models/`, repositories in `Data/`, views in `Views/Xxx/`