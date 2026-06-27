using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class RunLog
{
    [Key]
    public int Id { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public RunStatus Status { get; set; }
    public TriggerType TriggerType { get; set; }

    [ForeignKey("Pipeline")]
    public int PipelineId { get; set; }
    public virtual Pipeline Pipeline { get; set; } = null!;

    [ForeignKey("TriggeredBy")]
    public int TriggeredByUserId { get; set; }
    public virtual User TriggeredBy { get; set; } = null!;

    public virtual ICollection<Artifact> Artifacts { get; set; } = new List<Artifact>();

    public virtual ICollection<StepRun> StepRuns { get; set; } = new List<StepRun>();

    public string LogOutput { get; set; } = "";

    [NotMapped]
    public TimeSpan? Duration => FinishedAt.HasValue ? FinishedAt - StartedAt : null;
}
