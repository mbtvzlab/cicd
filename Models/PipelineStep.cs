using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class PipelineStep
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    public string Command { get; set; } = "";

    public int Order { get; set; }
    public int TimeoutSeconds { get; set; }
    public bool ContinueOnError { get; set; }

    [ForeignKey("Pipeline")]
    public int PipelineId { get; set; }
    public virtual Pipeline Pipeline { get; set; } = null!;

    [ForeignKey("LogArtifact")]
    public int? LogArtifactId { get; set; }
    public virtual Artifact? LogArtifact { get; set; }
}
