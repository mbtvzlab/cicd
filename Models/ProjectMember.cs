namespace CiCd.Models;

public class ProjectMember
{
    public int Id { get; set; }
    public DateTime JoinedAt { get; set; }
    public MemberRole Role { get; set; }

    public User User { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
