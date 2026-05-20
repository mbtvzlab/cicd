using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class StepRun
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Step")]
    public int StepId { get; set; }
    public virtual PipelineStep Step { get; set; } = null!;

    [ForeignKey("RunLog")]
    public int RunLogId { get; set; }
    public virtual RunLog RunLog { get; set; } = null!;

    public int ExecutionOrder { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public StepRunStatus Status { get; set; }
    public int? ExitCode { get; set; }

    public string ConsoleOutput { get; set; } = "";

    [NotMapped]
    public TimeSpan? Duration => StartedAt.HasValue && FinishedAt.HasValue
        ? FinishedAt.Value - StartedAt.Value
        : (StartedAt.HasValue ? DateTime.UtcNow - StartedAt.Value : null);

    public string? ErrorMessage { get; set; }

    [ForeignKey("LogArtifact")]
    public int? LogArtifactId { get; set; }
    public virtual Artifact? LogArtifact { get; set; }
}
