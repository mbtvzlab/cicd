namespace CiCd.Models;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string RepositoryUrl { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public User Owner { get; set; } = null!;
    public List<ProjectMember> Members { get; set; } = [];
    public List<Pipeline> Pipelines { get; set; } = [];
}
