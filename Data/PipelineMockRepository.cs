using CiCd.Models;

namespace CiCd.Data;

public class PipelineMockRepository
{
    private readonly List<Pipeline> _pipelines;

    public PipelineMockRepository()
    {
        var alice = new User { Id = 1, Username = "alice", Email = "alice@example.com", PasswordHash = "hash1", CreatedAt = new DateTime(2024, 1, 10), Role = UserRole.Admin };
        var bob = new User { Id = 2, Username = "bob", Email = "bob@example.com", PasswordHash = "hash2", CreatedAt = new DateTime(2024, 2, 14), Role = UserRole.Developer };
        var carol = new User { Id = 3, Username = "carol", Email = "carol@example.com", PasswordHash = "hash3", CreatedAt = new DateTime(2024, 3, 5), Role = UserRole.Developer };

        var projectApi = new Project { Id = 1, Name = "REST API", IsActive = true };
        var projectFe = new Project { Id = 2, Name = "Frontend", IsActive = true };
        var projectInfra = new Project { Id = 3, Name = "Infrastructure", IsActive = false };

        var logArtifact1 = new Artifact { Id = 1, FileName = "build.log", BlobUrl = "https://s3.example.com/logs/1/build.log", SizeBytes = 4096, CreatedAt = new DateTime(2024, 5, 1, 10, 5, 0) };
        var logArtifact2 = new Artifact { Id = 2, FileName = "test.log", BlobUrl = "https://s3.example.com/logs/1/test.log", SizeBytes = 8192, CreatedAt = new DateTime(2024, 5, 1, 10, 6, 0) };
        var logArtifact3 = new Artifact { Id = 3, FileName = "deploy.log", BlobUrl = "https://s3.example.com/logs/1/deploy.log", SizeBytes = 1024, CreatedAt = new DateTime(2024, 5, 1, 10, 7, 0) };

        var pipelineCi = new Pipeline
        {
            Id = 1,
            Name = "CI Pipeline",
            Description = "Build and test on every push",
            Branch = "main",
            IsEnabled = true,
            CreatedAt = new DateTime(2024, 1, 20),
            Project = projectApi,
            Steps =
            [
                new PipelineStep { Id = 1, Name = "Build", Command = "dotnet build", Order = 1, TimeoutSeconds = 120, ContinueOnError = false, LogArtifact = logArtifact1 },
                new PipelineStep { Id = 2, Name = "Test", Command = "dotnet test", Order = 2, TimeoutSeconds = 180, ContinueOnError = false, LogArtifact = logArtifact2 },
                new PipelineStep { Id = 3, Name = "Deploy", Command = "./deploy.sh", Order = 3, TimeoutSeconds = 60, ContinueOnError = false, LogArtifact = logArtifact3 },
            ],
            RunLogs =
            [
                new RunLog
                {
                    Id = 1, Pipeline = null!, TriggeredBy = bob,
                    StartedAt = new DateTime(2024, 5, 1, 10, 0, 0), FinishedAt = new DateTime(2024, 5, 1, 10, 8, 0),
                    Status = RunStatus.Success, TriggerType = TriggerType.Push,
                    Artifacts = [new Artifact { Id = 10, FileName = "api.zip", BlobUrl = "https://s3.example.com/artifacts/api.zip", SizeBytes = 512000, CreatedAt = new DateTime(2024, 5, 1, 10, 8, 0) }],
                },
                new RunLog
                {
                    Id = 2, Pipeline = null!, TriggeredBy = alice,
                    StartedAt = new DateTime(2024, 5, 2, 9, 0, 0), FinishedAt = new DateTime(2024, 5, 2, 9, 3, 0),
                    Status = RunStatus.Failed, TriggerType = TriggerType.PullRequest,
                    Artifacts = [],
                },
            ]
        };

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
            Steps =
            [
                new PipelineStep { Id = 4, Name = "Lint", Command = "npm run lint", Order = 1, TimeoutSeconds = 60, ContinueOnError = false, LogArtifact = logArtifact4 },
                new PipelineStep { Id = 5, Name = "Build", Command = "npm run build", Order = 2, TimeoutSeconds = 120, ContinueOnError = false, LogArtifact = logArtifact5 },
            ],
            RunLogs =
            [
                new RunLog
                {
                    Id = 3, Pipeline = null!, TriggeredBy = carol,
                    StartedAt = new DateTime(2024, 5, 3, 14, 0, 0), FinishedAt = new DateTime(2024, 5, 3, 14, 5, 0),
                    Status = RunStatus.Success, TriggerType = TriggerType.Push,
                    Artifacts = [new Artifact { Id = 11, FileName = "dist.zip", BlobUrl = "https://s3.example.com/artifacts/dist.zip", SizeBytes = 204800, CreatedAt = new DateTime(2024, 5, 3, 14, 5, 0) }],
                },
                new RunLog
                {
                    Id = 4, Pipeline = null!, TriggeredBy = bob,
                    StartedAt = new DateTime(2024, 5, 4, 9, 30, 0), FinishedAt = null,
                    Status = RunStatus.Running, TriggerType = TriggerType.Manual,
                    Artifacts = [],
                },
            ]
        };

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
            Steps =
            [
                new PipelineStep { Id = 6, Name = "Validate", Command = "terraform validate", Order = 1, TimeoutSeconds = 30, ContinueOnError = false, LogArtifact = logArtifact6 },
                new PipelineStep { Id = 7, Name = "Plan", Command = "terraform plan", Order = 2, TimeoutSeconds = 120, ContinueOnError = false, LogArtifact = logArtifact7 },
                new PipelineStep { Id = 8, Name = "Apply", Command = "terraform apply", Order = 3, TimeoutSeconds = 300, ContinueOnError = false, LogArtifact = logArtifact8 },
            ],
            RunLogs =
            [
                new RunLog
                {
                    Id = 5, Pipeline = null!, TriggeredBy = alice,
                    StartedAt = new DateTime(2024, 4, 10, 8, 0, 0), FinishedAt = new DateTime(2024, 4, 10, 8, 7, 0),
                    Status = RunStatus.Success, TriggerType = TriggerType.Manual,
                    Artifacts = [],
                },
                new RunLog
                {
                    Id = 6, Pipeline = null!, TriggeredBy = alice,
                    StartedAt = new DateTime(2024, 4, 15, 12, 0, 0), FinishedAt = new DateTime(2024, 4, 15, 12, 2, 0),
                    Status = RunStatus.Cancelled, TriggerType = TriggerType.Schedule,
                    Artifacts = [],
                },
            ]
        };

        _pipelines = [pipelineCi, pipelineFeCI, pipelineTf];
    }

    public List<Pipeline> GetAll() => _pipelines;

    public Pipeline? GetById(int id) => _pipelines.FirstOrDefault(p => p.Id == id);
}
