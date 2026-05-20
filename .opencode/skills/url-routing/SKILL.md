---
name: url-routing
description: Configure ASP.NET Core MVC routing—conventional routes, attribute routes, area routes, and endpoint mapping
compatibility: opencode
metadata:
  stack: aspnet-core
  layer: routing
---

## What I do

- Define conventional routes in `Program.cs` via `MapControllerRoute`
- Add attribute routes on controllers and actions with `[Route]`, `[HttpGet]`, `[HttpPost]`, etc.
- Set up area routing for modular application structure
- Configure route constraints, defaults, and optional parameters
- Ensure URL generation with `Url.Action`, `Url.RouteUrl`, and tag helpers works consistently

## When to use me

Use this skill when you are adding new controllers, changing URL patterns, setting up areas, or debugging routing issues.

## Conventions for this project

- The default conventional route is `{controller=Home}/{action=Index}/{id?}` defined in `Program.cs:350`
- Controllers follow the `XxxController` naming convention in the `CiCd.Controllers` namespace
- Index and Details actions use conventional routing (no attribute routes)
- Navigation links in `_Layout.cshtml` use `@Url.Action("Action", "Controller")`
- The `id` route parameter is optional and nullable on Details actions
