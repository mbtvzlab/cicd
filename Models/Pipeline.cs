using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class Pipeline
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string Branch { get; set; } = "main";

    public bool IsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<PipelineStep> Steps { get; set; } = new List<PipelineStep>();
    public virtual ICollection<RunLog> RunLogs { get; set; } = new List<RunLog>();
}
