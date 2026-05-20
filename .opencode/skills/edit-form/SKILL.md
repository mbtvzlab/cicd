---
name: edit-form
description: Generate Create/Edit forms for entity CRUD operations in the CI/CD dashboard MVC application
compatibility: opencode
metadata:
  stack: aspnet-core
  layer: frontend
---

## What I do

- Generate Razor Views for creating and editing entity records
- Create shared partial views (`_EntityForm.cshtml`) for form fields reused between Create and Edit
- Create matching controller actions (GET for displaying the form, POST with `[ValidateAntiForgeryToken]` for submission)
- Use ASP.NET Core tag helpers (`asp-for`, `asp-action`, `asp-route-id`) for form binding and validation

## When to use me

Use this skill when you need new Create/Edit pages for an entity, or when you need to add form fields to an existing edit form.

## Conventions for this project

- Forms use `<form asp-action="Create" method="post">` or `<form asp-action="Edit" asp-route-id="@Model.Id" method="post">`
- Shared form fields live in a partial: `Views/Xxx/_XxxForm.cshtml`, rendered via `@await Html.PartialAsync("_XxxForm", Model)`
- Form inputs use inline styles matching the dark-theme CSS variables from `_Layout.cshtml`:
  - `background: var(--bg-tertiary); border: 1px solid var(--border); border-radius: 3px; color: var(--text-primary); padding: 6px 10px; font-size: 12px;`
- Validation errors shown with `<span asp-validation-for="Prop" style="color:var(--failed);font-size:11px;"></span>`
- Enum dropdowns use `asp-items="Html.GetEnumSelectList<EnumType>()"`
- Hidden fields for IDs: `<input type="hidden" asp-for="Id" />`
- Buttons: `.btn.btn-primary` for submit, `.btn.btn-secondary` for cancel
- Layout uses `.card > .card-body` to wrap forms
- For attribute-routed controllers:
  - GET Create: `[HttpGet("create")]`
  - POST Create: `[HttpPost("create")]`, `[ValidateAntiForgeryToken]`
  - GET Edit: `[HttpGet("{id:int}/edit")]`
  - POST Edit: `[HttpPost("{id:int}/edit")]`, `[ValidateAntiForgeryToken]`
- POST actions redirect to `nameof(Index)` on success