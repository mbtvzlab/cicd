# Semantic Database Model

## Tables

### Users
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| Username | varchar(100) | NOT NULL, UNIQUE |
| Email | varchar(200) | NOT NULL, UNIQUE |
| PasswordHash | text | NOT NULL |
| CreatedAt | timestamp | NOT NULL |
| Role | enum (Admin, Developer, Viewer) | NOT NULL |

### Projects
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| Name | varchar(200) | NOT NULL |
| Description | text | |
| RepositoryUrl | varchar(500) | NOT NULL |
| CreatedAt | timestamp | NOT NULL |
| IsActive | boolean | NOT NULL |
| OwnerId | int | FK → Users.Id (RESTRICT) |

### ProjectMembers
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| JoinedAt | timestamp | NOT NULL |
| Role | enum (Owner, Contributor, Viewer) | NOT NULL |
| UserId | int | FK → Users.Id (CASCADE) |
| ProjectId | int | FK → Projects.Id (CASCADE) |

### Pipelines
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| Name | varchar(200) | NOT NULL |
| Description | text | |
| Branch | varchar(100) | NOT NULL |
| IsEnabled | boolean | NOT NULL |
| CreatedAt | timestamp | NOT NULL |
| ProjectId | int | FK → Projects.Id (CASCADE) |

### PipelineSteps
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| Name | varchar(200) | NOT NULL |
| Command | text | NOT NULL |
| Order | int | NOT NULL |
| TimeoutSeconds | int | NOT NULL |
| ContinueOnError | boolean | NOT NULL |
| PipelineId | int | FK → Pipelines.Id (CASCADE) |
| LogArtifactId | int | FK → Artifacts.Id (SET NULL, nullable) |

### RunLogs
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| StartedAt | timestamp | NOT NULL |
| FinishedAt | timestamp | nullable |
| Status | enum (Pending, Running, Success, Failed, Cancelled) | NOT NULL |
| TriggerType | enum (Manual, Push, Schedule, PullRequest) | NOT NULL |
| PipelineId | int | FK → Pipelines.Id (CASCADE) |
| TriggeredByUserId | int | FK → Users.Id (RESTRICT) |

### StepRuns
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| ExecutionOrder | int | NOT NULL |
| StartedAt | timestamp | nullable |
| FinishedAt | timestamp | nullable |
| Status | enum (Pending, Running, Success, Failed, Cancelled, Skipped) | NOT NULL |
| ExitCode | int | nullable |
| ConsoleOutput | text | NOT NULL |
| ErrorMessage | text | nullable |
| StepId | int | FK → PipelineSteps.Id (CASCADE) |
| RunLogId | int | FK → RunLogs.Id (CASCADE) |
| LogArtifactId | int | FK → Artifacts.Id (SET NULL, nullable) |

### Artifacts
| Column | Type | Constraints |
|--------|------|-------------|
| Id | int | PK |
| FileName | varchar(255) | NOT NULL |
| BlobUrl | text | NOT NULL |
| SizeBytes | bigint | NOT NULL |
| CreatedAt | timestamp | NOT NULL |
| RunLogId | int | FK → RunLogs.Id (CASCADE, nullable) |

## Relationships

```
Users (1) ──< ProjectMembers >── (1) Projects
Users (1) ──────────────────────> (1) Projects  (Owner via OwnerId)

Projects (1) ──< Pipelines
Pipelines (1) ──< PipelineSteps
Pipelines (1) ──< RunLogs

RunLogs (1) ──< StepRuns
RunLogs (1) ──< Artifacts

PipelineSteps (1) ──> (0..1) Artifacts  (LogArtifact)
StepRuns (1) ──> (0..1) Artifacts  (LogArtifact)

StepRuns >── (1) PipelineSteps
```

- User → ProjectMember: one-to-many (a user has many memberships)
- Project → ProjectMember: one-to-many (a project has many members)
- Project → Pipeline: one-to-many (cascade delete)
- Pipeline → PipelineStep: one-to-many (cascade delete)
- Pipeline → RunLog: one-to-many (cascade delete)
- RunLog → StepRun: one-to-many (cascade delete)
- RunLog → Artifact: one-to-many (cascade delete, nullable FK — step-log artifacts have null RunLogId)
- PipelineStep → Artifact: one-to-one, optional (LogArtifactId nullable, SET NULL on delete)
- StepRun → Artifact: one-to-one, optional (LogArtifactId nullable, SET NULL on delete)
- StepRun → PipelineStep: many-to-one (cascade delete)
