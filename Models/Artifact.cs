using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class Artifact
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = "";

    [Required]
    public string BlobUrl { get; set; } = "";

    public long SizeBytes { get; set; }
    public DateTime CreatedAt { get; set; }

    [ForeignKey("RunLog")]
    public int RunLogId { get; set; }
    public virtual RunLog RunLog { get; set; } = null!;
}
