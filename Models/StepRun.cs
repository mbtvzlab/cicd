namespace CiCd.Models;

/// <summary>
/// Represents the execution of a single pipeline step within a run.
/// Contains the actual output logs and execution status for that step.
/// </summary>
public class StepRun
{
    public int Id { get; set; }
    
    /// <summary>
    /// The pipeline step definition that was executed
    /// </summary>
    public PipelineStep Step { get; set; } = null!;
    
    /// <summary>
    /// The parent run log this step run belongs to
    /// </summary>
    public RunLog RunLog { get; set; } = null!;
    
    /// <summary>
    /// Execution order within this run
    /// </summary>
    public int ExecutionOrder { get; set; }
    
    /// <summary>
    /// When the step started executing
    /// </summary>
    public DateTime? StartedAt { get; set; }
    
    /// <summary>
    /// When the step finished executing
    /// </summary>
    public DateTime? FinishedAt { get; set; }
    
    /// <summary>
    /// Current status of the step execution
    /// </summary>
    public StepRunStatus Status { get; set; }
    
    /// <summary>
    /// Exit code from the command execution (null if not finished)
    /// </summary>
    public int? ExitCode { get; set; }
    
    /// <summary>
    /// Full console output (stdout + stderr) from the step
    /// </summary>
    public string ConsoleOutput { get; set; } = "";
    
    /// <summary>
    /// Duration of the step execution
    /// </summary>
    public TimeSpan? Duration => StartedAt.HasValue && FinishedAt.HasValue 
        ? FinishedAt.Value - StartedAt.Value 
        : (StartedAt.HasValue ? DateTime.UtcNow - StartedAt.Value : null);
    
    /// <summary>
    /// Error message if the step failed
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// Artifact containing the step log file
    /// </summary>
    public Artifact? LogArtifact { get; set; }
}
