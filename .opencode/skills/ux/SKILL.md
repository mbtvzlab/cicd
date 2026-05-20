---
name: ux
description: Design and implement consistent, accessible UI patterns for ASP.NET Core MVC views—layout, components, styling, and interaction
compatibility: opencode
metadata:
  stack: aspnet-core
  layer: frontend
---

## What I do

- Build Razor Views and partials that follow the project's design system
- Apply the existing CSS variable theme (dark mode, accent colors, spacing, typography)
- Create reusable UI components: stat cards, badge indicators, data tables, info grids
- Ensure responsive layouts with mobile-first breakpoints
- Improve accessibility: semantic HTML, ARIA attributes, keyboard navigation, color contrast

## When to use me

Use this skill when you are creating or updating views, adjusting the layout, adding UI components, or improving the user experience of existing pages.

## Conventions for this project

- Layout is defined in `Views/Shared/_Layout.cshtml` with a sidebar + main-content structure
- CSS variables are declared in `:root` inside `_Layout.cshtml` (e.g. `--bg-primary`, `--accent-orange`, `--success`, `--failed`)
- Active nav detection uses `ViewContext.RouteData.Values["Controller"]`
- Styling is inline in `_Layout.cshtml` — keep global styles there, page-specific styles in the view
- Status badges use `.badge-success`, `.badge-failed`, `.badge-running`, `.badge-cancelled` with `.badge-dot`
- Stat cards use `.stat-card > .stat-value + .stat-label`
- Data tables use `.table-container > table` pattern
- Detail pages use `.details-grid` for two-column layouts and `.info-row > .info-label + .info-value` for key-value pairs
- Breadcrumbs use `.breadcrumb > a + .breadcrumb-separator + .breadcrumb-current`
- Responsive breakpoint at 1024px collapses grids
