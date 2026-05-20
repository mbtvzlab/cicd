using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class Project
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    public string Description { get; set; } = "";

    [Required]
    [MaxLength(500)]
    public string RepositoryUrl { get; set; } = "";

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsActive { get; set; }

    [ForeignKey("Owner")]
    public int OwnerId { get; set; }
    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    public virtual ICollection<Pipeline> Pipelines { get; set; } = new List<Pipeline>();
}
