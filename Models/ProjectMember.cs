using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiCd.Models;

public class ProjectMember
{
    [Key]
    public int Id { get; set; }
    public DateTime JoinedAt { get; set; }
    public MemberRole Role { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;
}
