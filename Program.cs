using CiCd.Models;

// Seed users
var alice = new User
{
    Id = 1, Username = "alice", Email = "alice@example.com",
    PasswordHash = "hash1", CreatedAt = new DateTime(2024, 1, 10), Role = UserRole.Admin
};
var bob = new User
{
    Id = 2, Username = "bob", Email = "bob@example.com",
    PasswordHash = "hash2", CreatedAt = new DateTime(2024, 2, 14), Role = UserRole.Developer
};
var carol = new User
{
    Id = 3, Username = "carol", Email = "carol@example.com",
    PasswordHash = "hash3", CreatedAt = new DateTime(2024, 3, 5), Role = UserRole.Developer
};
var dave = new User
{
    Id = 4, Username = "dave", Email = "dave@example.com",
    PasswordHash = "hash4", CreatedAt = new DateTime(2024, 4, 20), Role = UserRole.Viewer
};

var users = new List<User> { alice, bob, carol, dave };

// example project 1: REST API
var projectApi = new Project
{
    Id = 1,
    Name = "REST API",
    Description = "Backend REST API service",
    RepositoryUrl = "https://github.com/example/rest-api",
    CreatedAt = new DateTime(2024, 1, 15),
    IsActive = true,
    Owner = alice,
};

projectApi.Members =
[
    new ProjectMember
    {
        Id = 1, User = alice, Project = projectApi,
        Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 1, 15)
    },
    new ProjectMember
    {
        Id = 2, User = bob, Project = projectApi,
        Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 1, 20)
    },
    new ProjectMember
    {
        Id = 3, User = dave, Project = projectApi,
        Role = MemberRole.Viewer, JoinedAt = new DateTime(2024, 2, 1)
    },
];

var logArtifact1 = new Artifact
{
    Id = 1, FileName = "build.log", BlobUrl = "https://s3.example.com/logs/1/build.log",
    SizeBytes = 4096, CreatedAt = new DateTime(2024, 5, 1, 10, 5, 0)
};
var logArtifact2 = new Artifact
{
    Id = 2, FileName = "test.log", BlobUrl = "https://s3.example.com/logs/1/test.log",
    SizeBytes = 8192, CreatedAt = new DateTime(2024, 5, 1, 10, 6, 0)
};
var logArtifact3 = new Artifact
{
    Id = 3, FileName = "deploy.log", BlobUrl = "https://s3.example.com/logs/1/deploy.log",
    SizeBytes = 1024, CreatedAt = new DateTime(2024, 5, 1, 10, 7, 0)
};

var pipelineCi = new Pipeline
{
    Id = 1,
    Name = "CI Pipeline",
    Description = "Build and test on every push",
    Branch = "main",
    IsEnabled = true,
    CreatedAt = new DateTime(2024, 1, 20),
    Project = projectApi,
};
pipelineCi.Steps =
[
    new PipelineStep { Id = 1, Name = "Build", Command = "dotnet build", Order = 1, TimeoutSeconds = 120, ContinueOnError = false, Pipeline = pipelineCi, LogArtifact = logArtifact1 },
    new PipelineStep { Id = 2, Name = "Test", Command = "dotnet test", Order = 2, TimeoutSeconds = 180, ContinueOnError = false, Pipeline = pipelineCi, LogArtifact = logArtifact2 },
    new PipelineStep { Id = 3, Name = "Deploy", Command = "./deploy.sh", Order = 3, TimeoutSeconds = 60, ContinueOnError = false, Pipeline = pipelineCi, LogArtifact = logArtifact3 },
];

var runArtifact1 = new Artifact { Id = 10, FileName = "api.zip", BlobUrl = "https://s3.example.com/artifacts/api.zip", SizeBytes = 512000, CreatedAt = new DateTime(2024, 5, 1, 10, 8, 0) };

pipelineCi.RunLogs =
[
    new RunLog
    {
        Id = 1, Pipeline = pipelineCi, TriggeredBy = bob,
        StartedAt = new DateTime(2024, 5, 1, 10, 0, 0), FinishedAt = new DateTime(2024, 5, 1, 10, 8, 0),
        Status = RunStatus.Success, TriggerType = TriggerType.Push,
        Artifacts = [runArtifact1],
    },
    new RunLog
    {
        Id = 2, Pipeline = pipelineCi, TriggeredBy = alice,
        StartedAt = new DateTime(2024, 5, 2, 9, 0, 0), FinishedAt = new DateTime(2024, 5, 2, 9, 3, 0),
        Status = RunStatus.Failed, TriggerType = TriggerType.PullRequest,
        Artifacts = [],
    },
];

projectApi.Pipelines = [pipelineCi];


// example project 2: Frontend
var projectFe = new Project
{
    Id = 2,
    Name = "Frontend",
    Description = "React/TypeScript frontend application",
    RepositoryUrl = "https://github.com/example/frontend",
    CreatedAt = new DateTime(2024, 2, 1),
    IsActive = true,
    Owner = carol,
};

projectFe.Members =
[
    new ProjectMember { Id = 4, User = carol, Project = projectFe, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 2, 1) },
    new ProjectMember { Id = 5, User = bob, Project = projectFe, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 2, 5) },
];

var logArtifact4 = new Artifact { Id = 4, FileName = "lint.log", BlobUrl = "https://s3.example.com/logs/2/lint.log", SizeBytes = 2048, CreatedAt = new DateTime(2024, 5, 3, 14, 1, 0) };
var logArtifact5 = new Artifact { Id = 5, FileName = "build.log", BlobUrl = "https://s3.example.com/logs/2/build.log", SizeBytes = 6144, CreatedAt = new DateTime(2024, 5, 3, 14, 3, 0) };

var pipelineFeCI = new Pipeline
{
    Id = 2,
    Name = "Frontend CI",
    Description = "Lint and build the frontend bundle",
    Branch = "main",
    IsEnabled = true,
    CreatedAt = new DateTime(2024, 2, 10),
    Project = projectFe,
};
pipelineFeCI.Steps =
[
    new PipelineStep { Id = 4, Name = "Lint", Command = "npm run lint", Order = 1, TimeoutSeconds = 60, ContinueOnError = false, Pipeline = pipelineFeCI, LogArtifact = logArtifact4 },
    new PipelineStep { Id = 5, Name = "Build", Command = "npm run build", Order = 2, TimeoutSeconds = 120, ContinueOnError = false, Pipeline = pipelineFeCI, LogArtifact = logArtifact5 },
];

var runArtifact2 = new Artifact { Id = 11, FileName = "dist.zip", BlobUrl = "https://s3.example.com/artifacts/dist.zip", SizeBytes = 204800, CreatedAt = new DateTime(2024, 5, 3, 14, 5, 0) };

pipelineFeCI.RunLogs =
[
    new RunLog
    {
        Id = 3, Pipeline = pipelineFeCI, TriggeredBy = carol,
        StartedAt = new DateTime(2024, 5, 3, 14, 0, 0), FinishedAt = new DateTime(2024, 5, 3, 14, 5, 0),
        Status = RunStatus.Success, TriggerType = TriggerType.Push,
        Artifacts = [runArtifact2],
    },
    new RunLog
    {
        Id = 4, Pipeline = pipelineFeCI, TriggeredBy = bob,
        StartedAt = new DateTime(2024, 5, 4, 9, 30, 0), FinishedAt = null,
        Status = RunStatus.Running, TriggerType = TriggerType.Manual,
        Artifacts = [],
    },
];

projectFe.Pipelines = [pipelineFeCI];

// example project 3: Infrastructure
var projectInfra = new Project
{
    Id = 3,
    Name = "Infrastructure",
    Description = "Terraform configs and deployment scripts",
    RepositoryUrl = "https://github.com/example/infra",
    CreatedAt = new DateTime(2024, 3, 1),
    IsActive = false,
    Owner = alice,
};

projectInfra.Members =
[
    new ProjectMember { Id = 6, User = alice, Project = projectInfra, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 3, 1) },
    new ProjectMember { Id = 7, User = carol, Project = projectInfra, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 3, 10) },
    new ProjectMember { Id = 8, User = dave, Project = projectInfra, Role = MemberRole.Viewer, JoinedAt = new DateTime(2024, 3, 15) },
];

var logArtifact6 = new Artifact { Id = 6, FileName = "validate.log", BlobUrl = "https://s3.example.com/logs/3/validate.log", SizeBytes = 512, CreatedAt = new DateTime(2024, 4, 10, 8, 1, 0) };
var logArtifact7 = new Artifact { Id = 7, FileName = "plan.log", BlobUrl = "https://s3.example.com/logs/3/plan.log", SizeBytes = 3072, CreatedAt = new DateTime(2024, 4, 10, 8, 3, 0) };
var logArtifact8 = new Artifact { Id = 8, FileName = "apply.log", BlobUrl = "https://s3.example.com/logs/3/apply.log", SizeBytes = 2048, CreatedAt = new DateTime(2024, 4, 10, 8, 6, 0) };

var pipelineTf = new Pipeline
{
    Id = 3,
    Name = "Terraform Apply",
    Description = "Validate, plan and apply infrastructure changes",
    Branch = "main",
    IsEnabled = false,
    CreatedAt = new DateTime(2024, 3, 5),
    Project = projectInfra,
};
pipelineTf.Steps =
[
    new PipelineStep { Id = 6, Name = "Validate", Command = "terraform validate", Order = 1, TimeoutSeconds = 30, ContinueOnError = false, Pipeline = pipelineTf, LogArtifact = logArtifact6 },
    new PipelineStep { Id = 7, Name = "Plan", Command = "terraform plan", Order = 2, TimeoutSeconds = 120, ContinueOnError = false, Pipeline = pipelineTf, LogArtifact = logArtifact7 },
    new PipelineStep { Id = 8, Name = "Apply", Command = "terraform apply", Order = 3, TimeoutSeconds = 300, ContinueOnError = false, Pipeline = pipelineTf, LogArtifact = logArtifact8 },
];

pipelineTf.RunLogs =
[
    new RunLog
    {
        Id = 5, Pipeline = pipelineTf, TriggeredBy = alice,
        StartedAt = new DateTime(2024, 4, 10, 8, 0, 0), FinishedAt = new DateTime(2024, 4, 10, 8, 7, 0),
        Status = RunStatus.Success, TriggerType = TriggerType.Manual,
        Artifacts = [],
    },
    new RunLog
    {
        Id = 6, Pipeline = pipelineTf, TriggeredBy = alice,
        StartedAt = new DateTime(2024, 4, 15, 12, 0, 0), FinishedAt = new DateTime(2024, 4, 15, 12, 2, 0),
        Status = RunStatus.Cancelled, TriggerType = TriggerType.Schedule,
        Artifacts = [],
    },
];

projectInfra.Pipelines = [pipelineTf];

var projects = new List<Project> { projectApi, projectFe, projectInfra };
var pipelines = new List<Pipeline> { pipelineCi, pipelineFeCI, pipelineTf };
var runLogs = pipelines.SelectMany(p => p.RunLogs).ToList();

// ---------------------------------------------------------------------------
// LINQ queries
// ---------------------------------------------------------------------------

Console.WriteLine("=== 1. Active projects with their pipeline count ===");
var activeProjectPipelineCounts = projects
    .Where(p => p.IsActive)
    .Select(p => new { p.Name, PipelineCount = p.Pipelines.Count });

foreach (var item in activeProjectPipelineCounts)
    Console.WriteLine($"  {item.Name}: {item.PipelineCount} pipeline(s)");

Console.WriteLine("\n=== 2. All failed or cancelled runs, newest first ===");
var failedRuns = runLogs
    .Where(r => r.Status == RunStatus.Failed || r.Status == RunStatus.Cancelled)
    .OrderByDescending(r => r.StartedAt);

foreach (var r in failedRuns)
    Console.WriteLine($"  [{r.Status}] {r.Pipeline.Name} triggered by {r.TriggeredBy.Username} at {r.StartedAt}");

Console.WriteLine("\n=== 3. Users with the number of projects they are members of ===");
var userMembershipCounts = users
    .Select(u => new
    {
        u.Username,
        ProjectCount = projects.Count(p => p.Members.Any(m => m.User == u))
    })
    .OrderByDescending(x => x.ProjectCount);

foreach (var item in userMembershipCounts)
    Console.WriteLine($"  {item.Username}: {item.ProjectCount} project(s)");

Console.WriteLine("\n=== 4. Average run duration (in seconds) per pipeline ===");
var avgDurations = pipelines
    .Select(p => new
    {
        p.Name,
        AvgSeconds = p.RunLogs
            .Where(r => r.Duration.HasValue)
            .Select(r => r.Duration!.Value.TotalSeconds)
            .DefaultIfEmpty(0)
            .Average()
    });

foreach (var item in avgDurations)
    Console.WriteLine($"  {item.Name}: {item.AvgSeconds:F1}s avg");

Console.WriteLine("\n=== 5. Pipeline steps with a timeout under 60 seconds ===");
var shortTimeoutSteps = pipelines
    .SelectMany(p => p.Steps)
    .Where(s => s.TimeoutSeconds < 60)
    .Select(s => new { Pipeline = s.Pipeline.Name, Step = s.Name, s.TimeoutSeconds });

foreach (var s in shortTimeoutSteps)
    Console.WriteLine($"  [{s.Pipeline}] {s.Step} — timeout: {s.TimeoutSeconds}s");

Console.WriteLine("\n=== 6. Total artifact storage per project (bytes) ===");
var storagePerProject = projects
    .Select(p => new
    {
        p.Name,
        TotalBytes = p.Pipelines
            .SelectMany(pl => pl.RunLogs)
            .SelectMany(r => r.Artifacts)
            .Sum(a => a.SizeBytes)
    })
    .OrderByDescending(x => x.TotalBytes);

foreach (var item in storagePerProject)
    Console.WriteLine($"  {item.Name}: {item.TotalBytes:N0} bytes");

Console.WriteLine("\n=== 7. Most recent successful run per pipeline ===");
var latestSuccess = pipelines
    .Select(p => new
    {
        p.Name,
        LastSuccess = p.RunLogs
            .Where(r => r.Status == RunStatus.Success)
            .OrderByDescending(r => r.StartedAt)
            .FirstOrDefault()
    });

foreach (var item in latestSuccess)
{
    var when = item.LastSuccess?.StartedAt.ToString("yyyy-MM-dd HH:mm") ?? "never";
    Console.WriteLine($"  {item.Name}: {when}");
}

// ---------------------------------------------------------------------------
// Web host (MVC — not used yet)
// ---------------------------------------------------------------------------

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();

app.Run();
