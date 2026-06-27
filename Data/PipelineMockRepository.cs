using CiCd.Models;

namespace CiCd.Data;

public class PipelineMockRepository : IPipelineRepository
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
                new PipelineStep { Id = 1, Name = "Build", Command = "dotnet build", Order = 1, TimeoutSeconds = 120, ContinueOnError = false },
                new PipelineStep { Id = 2, Name = "Test", Command = "dotnet test", Order = 2, TimeoutSeconds = 180, ContinueOnError = false },
                new PipelineStep { Id = 3, Name = "Deploy", Command = "./deploy.sh", Order = 3, TimeoutSeconds = 60, ContinueOnError = false },
            ],
            RunLogs =
            [
                new RunLog
                {
                    Id = 1, Pipeline = null!, TriggeredBy = bob,
                    StartedAt = new DateTime(2024, 5, 1, 10, 0, 0), FinishedAt = new DateTime(2024, 5, 1, 10, 8, 0),
                    Status = RunStatus.Success, TriggerType = TriggerType.Push,
                    LogOutput = "Pipeline started.",
                    Artifacts = [new Artifact { Id = 10, FileName = "api.zip", BlobUrl = "https://s3.example.com/artifacts/api.zip", SizeBytes = 512000, CreatedAt = new DateTime(2024, 5, 1, 10, 8, 0) }],
                    StepRuns =
                    [
                        new StepRun { Id = 1, Step = new PipelineStep { Id = 1, Name = "Build" }, ExecutionOrder = 1, StartedAt = new DateTime(2024, 5, 1, 10, 0, 1), FinishedAt = new DateTime(2024, 5, 1, 10, 3, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Build started...\nBuild succeeded." },
                        new StepRun { Id = 2, Step = new PipelineStep { Id = 2, Name = "Test" }, ExecutionOrder = 2, StartedAt = new DateTime(2024, 5, 1, 10, 3, 1), FinishedAt = new DateTime(2024, 5, 1, 10, 7, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Test run started...\n42 tests passed." },
                        new StepRun { Id = 3, Step = new PipelineStep { Id = 3, Name = "Deploy" }, ExecutionOrder = 3, StartedAt = new DateTime(2024, 5, 1, 10, 7, 1), FinishedAt = new DateTime(2024, 5, 1, 10, 8, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Deploying...\nDeployment complete." }
                    ]
                },
                new RunLog
                {
                    Id = 2, Pipeline = null!, TriggeredBy = alice,
                    StartedAt = new DateTime(2024, 5, 2, 9, 0, 0), FinishedAt = new DateTime(2024, 5, 2, 9, 3, 0),
                    Status = RunStatus.Failed, TriggerType = TriggerType.PullRequest,
                    LogOutput = "Pipeline started.",
                    Artifacts = [],
                    StepRuns =
                    [
                        new StepRun { Id = 4, Step = new PipelineStep { Id = 1, Name = "Build" }, ExecutionOrder = 1, StartedAt = new DateTime(2024, 5, 2, 9, 0, 1), FinishedAt = new DateTime(2024, 5, 2, 9, 2, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Build started...\nBuild succeeded." },
                        new StepRun { Id = 5, Step = new PipelineStep { Id = 2, Name = "Test" }, ExecutionOrder = 2, StartedAt = new DateTime(2024, 5, 2, 9, 2, 1), FinishedAt = new DateTime(2024, 5, 2, 9, 3, 0), Status = StepRunStatus.Failed, ExitCode = 1, ErrorMessage = "Test project failed: assertion in PaymentServiceTests.ShouldChargeAmount.", LogOutput = "Test run started...\n3 tests failed." }
                    ]
                },
            ]
        };

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
                new PipelineStep { Id = 4, Name = "Lint", Command = "npm run lint", Order = 1, TimeoutSeconds = 60, ContinueOnError = false },
                new PipelineStep { Id = 5, Name = "Build", Command = "npm run build", Order = 2, TimeoutSeconds = 120, ContinueOnError = false },
            ],
            RunLogs =
            [
                new RunLog
                {
                    Id = 3, Pipeline = null!, TriggeredBy = carol,
                    StartedAt = new DateTime(2024, 5, 3, 14, 0, 0), FinishedAt = new DateTime(2024, 5, 3, 14, 5, 0),
                    Status = RunStatus.Success, TriggerType = TriggerType.Push,
                    LogOutput = "Pipeline started.",
                    Artifacts = [new Artifact { Id = 11, FileName = "dist.zip", BlobUrl = "https://s3.example.com/artifacts/dist.zip", SizeBytes = 204800, CreatedAt = new DateTime(2024, 5, 3, 14, 5, 0) }],
                    StepRuns =
                    [
                        new StepRun { Id = 6, Step = new PipelineStep { Id = 4, Name = "Lint" }, ExecutionOrder = 1, StartedAt = new DateTime(2024, 5, 3, 14, 0, 1), FinishedAt = new DateTime(2024, 5, 3, 14, 2, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Lint started...\nNo lint errors." },
                        new StepRun { Id = 7, Step = new PipelineStep { Id = 5, Name = "Build" }, ExecutionOrder = 2, StartedAt = new DateTime(2024, 5, 3, 14, 2, 1), FinishedAt = new DateTime(2024, 5, 3, 14, 5, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Build started...\nBundle created." }
                    ]
                },
                new RunLog
                {
                    Id = 4, Pipeline = null!, TriggeredBy = bob,
                    StartedAt = new DateTime(2024, 5, 4, 9, 30, 0), FinishedAt = null,
                    Status = RunStatus.Running, TriggerType = TriggerType.Manual,
                    LogOutput = "Pipeline started.",
                    Artifacts = [],
                    StepRuns =
                    [
                        new StepRun { Id = 8, Step = new PipelineStep { Id = 4, Name = "Lint" }, ExecutionOrder = 1, StartedAt = new DateTime(2024, 5, 4, 9, 30, 1), FinishedAt = null, Status = StepRunStatus.Running, ExitCode = null, LogOutput = "Lint started..." }
                    ]
                },
            ]
        };

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
                new PipelineStep { Id = 6, Name = "Validate", Command = "terraform validate", Order = 1, TimeoutSeconds = 30, ContinueOnError = false },
                new PipelineStep { Id = 7, Name = "Plan", Command = "terraform plan", Order = 2, TimeoutSeconds = 120, ContinueOnError = false },
                new PipelineStep { Id = 8, Name = "Apply", Command = "terraform apply", Order = 3, TimeoutSeconds = 300, ContinueOnError = false },
            ],
            RunLogs =
            [
                new RunLog
                {
                    Id = 5, Pipeline = null!, TriggeredBy = alice,
                    StartedAt = new DateTime(2024, 4, 10, 8, 0, 0), FinishedAt = new DateTime(2024, 4, 10, 8, 7, 0),
                    Status = RunStatus.Success, TriggerType = TriggerType.Manual,
                    LogOutput = "Pipeline started.",
                    Artifacts = [],
                    StepRuns =
                    [
                        new StepRun { Id = 9, Step = new PipelineStep { Id = 6, Name = "Validate" }, ExecutionOrder = 1, StartedAt = new DateTime(2024, 4, 10, 8, 0, 1), FinishedAt = new DateTime(2024, 4, 10, 8, 1, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Terraform validate passed." },
                        new StepRun { Id = 10, Step = new PipelineStep { Id = 7, Name = "Plan" }, ExecutionOrder = 2, StartedAt = new DateTime(2024, 4, 10, 8, 1, 1), FinishedAt = new DateTime(2024, 4, 10, 8, 4, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Plan: 2 to add, 0 to change, 0 to destroy." },
                        new StepRun { Id = 11, Step = new PipelineStep { Id = 8, Name = "Apply" }, ExecutionOrder = 3, StartedAt = new DateTime(2024, 4, 10, 8, 4, 1), FinishedAt = new DateTime(2024, 4, 10, 8, 7, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Apply complete. Resources created." }
                    ]
                },
                new RunLog
                {
                    Id = 6, Pipeline = null!, TriggeredBy = alice,
                    StartedAt = new DateTime(2024, 4, 15, 12, 0, 0), FinishedAt = new DateTime(2024, 4, 15, 12, 2, 0),
                    Status = RunStatus.Cancelled, TriggerType = TriggerType.Schedule,
                    LogOutput = "Pipeline started.",
                    Artifacts = [],
                    StepRuns =
                    [
                        new StepRun { Id = 12, Step = new PipelineStep { Id = 6, Name = "Validate" }, ExecutionOrder = 1, StartedAt = new DateTime(2024, 4, 15, 12, 0, 1), FinishedAt = new DateTime(2024, 4, 15, 12, 1, 0), Status = StepRunStatus.Success, ExitCode = 0, LogOutput = "Terraform validate passed." },
                        new StepRun { Id = 13, Step = new PipelineStep { Id = 7, Name = "Plan" }, ExecutionOrder = 2, StartedAt = new DateTime(2024, 4, 15, 12, 1, 1), FinishedAt = new DateTime(2024, 4, 15, 12, 2, 0), Status = StepRunStatus.Cancelled, ExitCode = null, LogOutput = "Plan in progress...\nRun cancelled by user." }
                    ]
                },
            ]
        };

        _pipelines = [pipelineCi, pipelineFeCI, pipelineTf];
    }

    public List<Pipeline> GetAll() => _pipelines;

    public Pipeline? GetById(int id) => _pipelines.FirstOrDefault(p => p.Id == id);

    public List<Pipeline> GetByProjectId(int projectId) => _pipelines.Where(p => p.Project.Id == projectId).ToList();

    public Pipeline Add(Pipeline pipeline)
    {
        pipeline.Id = _pipelines.Count > 0 ? _pipelines.Max(p => p.Id) + 1 : 1;
        _pipelines.Add(pipeline);
        return pipeline;
    }

    public void Update(Pipeline pipeline)
    {
        var index = _pipelines.FindIndex(p => p.Id == pipeline.Id);
        if (index >= 0) _pipelines[index] = pipeline;
    }

    public void Delete(int id)
    {
        _pipelines.RemoveAll(p => p.Id == id);
    }

    public List<Pipeline> Search(string query)
    {
        return _pipelines.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || p.Branch.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
