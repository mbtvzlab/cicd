# Sitemap

## Conventional Routes (Program.cs MapControllerRoute)

| URL Pattern | Controller | Action | View |
|-------------|-----------|--------|------|
| `/` | Home | Index | `Views/Home/Index.cshtml` |
| `/Home/Privacy` | Home | Privacy | `Views/Home/Privacy.cshtml` |
| `/Home/Error` | Home | Error | `Views/Shared/Error.cshtml` |
| `/Project` | Project | Index | `Views/Project/Index.cshtml` |
| `/Project/Details/{id}` | Project | Details | `Views/Project/Details.cshtml` |

## Custom Conventional Routes (Program.cs)

| URL Pattern | Controller | Action | View |
|-------------|-----------|--------|------|
| `/projekti` | Project | Index | `Views/Project/Index.cshtml` |
| `/projekti/{id}/detalji` | Project | Details | `Views/Project/Details.cshtml` |

## Attribute-Routed Controllers

### PipelineController (`[Route("pipelines")]`)

| URL Pattern | HTTP Method | Action | View |
|-------------|------------|--------|------|
| `/pipelines` | GET | Index | `Views/Pipeline/Index.cshtml` |
| `/pipelines/{id}` | GET | Details | `Views/Pipeline/Details.cshtml` |
| `/pipelines/{id}/toggle` | POST | Toggle | Redirect to Details |

### RunLogController (`[Route("runs")]`)

| URL Pattern | HTTP Method | Action | View |
|-------------|------------|--------|------|
| `/runs` | GET | Index | `Views/RunLog/Index.cshtml` |
| `/runs/{id}` | GET | Details | `Views/RunLog/Details.cshtml` |
| `/runs/{id}/retrigger` | POST | Retrigger | Redirect to new run Details |

### UserController (`[Route("users")]`)

| URL Pattern | HTTP Method | Action | View |
|-------------|------------|--------|------|
| `/users` | GET | Index | `Views/User/Index.cshtml` |
| `/users/create` | GET | Create | `Views/User/Create.cshtml` |
| `/users/create` | POST | Create | Redirect to Index |
| `/users/{id}/edit` | GET | Edit | `Views/User/Edit.cshtml` |
| `/users/{id}/edit` | POST | Edit | Redirect to Index |
| `/users/{id}/delete` | POST | Delete | Redirect to Index |

### ProjectController (mixed — conventional + attribute)

| URL Pattern | HTTP Method | Action | View |
|-------------|------------|--------|------|
| `/project/by-slug/{slug}` | GET | BySlug | `Views/Project/Details.cshtml` |

## All URLs Summary

| # | URL | Type |
|---|-----|------|
| 1 | `/` | Conventional |
| 2 | `/Home/Privacy` | Conventional |
| 3 | `/projekti` | Custom conventional |
| 4 | `/projekti/{id}/detalji` | Custom conventional |
| 5 | `/project/by-slug/{slug}` | Attribute route |
| 6 | `/pipelines` | Attribute route (controller-level) |
| 7 | `/pipelines/{id}` | Attribute route (controller+action) |
| 8 | `/pipelines/{id}/toggle` | Attribute route (controller+action, POST) |
| 9 | `/runs` | Attribute route (controller-level) |
| 10 | `/runs/{id}` | Attribute route (controller+action) |
| 11 | `/runs/{id}/retrigger` | Attribute route (controller+action, POST) |
| 12 | `/users` | Attribute route (controller-level) |
| 13 | `/users/create` | Attribute route (controller+action) |
| 14 | `/users/{id}/edit` | Attribute route (controller+action) |
| 15 | `/users/{id}/delete` | Attribute route (controller+action, POST) |
