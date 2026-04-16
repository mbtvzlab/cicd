namespace CiCd.Models;

public class RunLog
{
    public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public RunStatus Status { get; set; }
    public TriggerType TriggerType { get; set; }

    public Pipeline Pipeline { get; set; } = null!;
    public User TriggeredBy { get; set; } = null!;

    // Build outputs, test reports, binaries, etc.
    public List<Artifact> Artifacts { get; set; } = [];
    
    /// <summary>
    /// Individual step executions for this run
    /// </summary>
    public List<StepRun> StepRuns { get; set; } = [];

    public TimeSpan? Duration => FinishedAt.HasValue ? FinishedAt - StartedAt : null;
}
