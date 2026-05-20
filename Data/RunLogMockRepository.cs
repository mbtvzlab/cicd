using CiCd.Models;

namespace CiCd.Data;

public class RunLogMockRepository : IRunLogRepository
{
    private readonly List<RunLog> _runLogs;

    public RunLogMockRepository()
    {
        var alice = new User { Id = 1, Username = "alice", Email = "alice@example.com", PasswordHash = "hash1", CreatedAt = new DateTime(2024, 1, 10), Role = UserRole.Admin };
        var bob = new User { Id = 2, Username = "bob", Email = "bob@example.com", PasswordHash = "hash2", CreatedAt = new DateTime(2024, 2, 14), Role = UserRole.Developer };
        var carol = new User { Id = 3, Username = "carol", Email = "carol@example.com", PasswordHash = "hash3", CreatedAt = new DateTime(2024, 3, 5), Role = UserRole.Developer };

        var pipelineCi = new Pipeline { Id = 1, Name = "CI Pipeline" };
        var pipelineFeCI = new Pipeline { Id = 2, Name = "Frontend CI" };
        var pipelineTf = new Pipeline { Id = 3, Name = "Terraform Apply" };

        _runLogs =
        [
            new RunLog
            {
                Id = 1, Pipeline = pipelineCi, TriggeredBy = bob,
                StartedAt = new DateTime(2024, 5, 1, 10, 0, 0), FinishedAt = new DateTime(2024, 5, 1, 10, 8, 0),
                Status = RunStatus.Success, TriggerType = TriggerType.Push,
                Artifacts = [new Artifact { Id = 10, FileName = "api.zip", BlobUrl = "https://s3.example.com/artifacts/api.zip", SizeBytes = 512000, CreatedAt = new DateTime(2024, 5, 1, 10, 8, 0) }],
            },
            new RunLog
            {
                Id = 2, Pipeline = pipelineCi, TriggeredBy = alice,
                StartedAt = new DateTime(2024, 5, 2, 9, 0, 0), FinishedAt = new DateTime(2024, 5, 2, 9, 3, 0),
                Status = RunStatus.Failed, TriggerType = TriggerType.PullRequest,
                Artifacts = [],
            },
            new RunLog
            {
                Id = 3, Pipeline = pipelineFeCI, TriggeredBy = carol,
                StartedAt = new DateTime(2024, 5, 3, 14, 0, 0), FinishedAt = new DateTime(2024, 5, 3, 14, 5, 0),
                Status = RunStatus.Success, TriggerType = TriggerType.Push,
                Artifacts = [new Artifact { Id = 11, FileName = "dist.zip", BlobUrl = "https://s3.example.com/artifacts/dist.zip", SizeBytes = 204800, CreatedAt = new DateTime(2024, 5, 3, 14, 5, 0) }],
            },
            new RunLog
            {
                Id = 4, Pipeline = pipelineFeCI, TriggeredBy = bob,
                StartedAt = new DateTime(2024, 5, 4, 9, 30, 0), FinishedAt = null,
                Status = RunStatus.Running, TriggerType = TriggerType.Manual,
                Artifacts = [],
            },
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
    }

    public List<RunLog> GetAll() => _runLogs;

    public RunLog? GetById(int id) => _runLogs.FirstOrDefault(r => r.Id == id);

    public List<RunLog> GetByPipelineId(int pipelineId) => _runLogs.Where(r => r.Pipeline.Id == pipelineId).ToList();

    public RunLog Add(RunLog runLog)
    {
        runLog.Id = _runLogs.Count > 0 ? _runLogs.Max(r => r.Id) + 1 : 1;
        _runLogs.Add(runLog);
        return runLog;
    }

    public void Update(RunLog runLog)
    {
        var index = _runLogs.FindIndex(r => r.Id == runLog.Id);
        if (index >= 0) _runLogs[index] = runLog;
    }

    public void Delete(int id)
    {
        _runLogs.RemoveAll(r => r.Id == id);
    }
}
