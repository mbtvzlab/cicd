using CiCd.Models;

namespace CiCd.Data;

public class ProjectMockRepository : IProjectRepository
{
    private readonly List<Project> _projects;

    public ProjectMockRepository()
    {
        var alice = new User { Id = 1, Username = "alice", Email = "alice@example.com", PasswordHash = "hash1", CreatedAt = new DateTime(2024, 1, 10), Role = UserRole.Admin };
        var bob = new User { Id = 2, Username = "bob", Email = "bob@example.com", PasswordHash = "hash2", CreatedAt = new DateTime(2024, 2, 14), Role = UserRole.Developer };
        var carol = new User { Id = 3, Username = "carol", Email = "carol@example.com", PasswordHash = "hash3", CreatedAt = new DateTime(2024, 3, 5), Role = UserRole.Developer };
        var dave = new User { Id = 4, Username = "dave", Email = "dave@example.com", PasswordHash = "hash4", CreatedAt = new DateTime(2024, 4, 20), Role = UserRole.Viewer };

        var projectApi = new Project
        {
            Id = 1,
            Name = "REST API",
            Description = "Backend REST API service",
            RepositoryUrl = "https://github.com/example/rest-api",
            CreatedAt = new DateTime(2024, 1, 15),
            IsActive = true,
            Owner = alice,
            Members =
            [
                new ProjectMember { Id = 1, User = alice, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 1, 15) },
                new ProjectMember { Id = 2, User = bob, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 1, 20) },
                new ProjectMember { Id = 3, User = dave, Role = MemberRole.Viewer, JoinedAt = new DateTime(2024, 2, 1) },
            ]
        };

        var projectFe = new Project
        {
            Id = 2,
            Name = "Frontend",
            Description = "React/TypeScript frontend application",
            RepositoryUrl = "https://github.com/example/frontend",
            CreatedAt = new DateTime(2024, 2, 1),
            IsActive = true,
            Owner = carol,
            Members =
            [
                new ProjectMember { Id = 4, User = carol, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 2, 1) },
                new ProjectMember { Id = 5, User = bob, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 2, 5) },
            ]
        };

        var projectInfra = new Project
        {
            Id = 3,
            Name = "Infrastructure",
            Description = "Terraform configs and deployment scripts",
            RepositoryUrl = "https://github.com/example/infra",
            CreatedAt = new DateTime(2024, 3, 1),
            IsActive = false,
            Owner = alice,
            Members =
            [
                new ProjectMember { Id = 6, User = alice, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 3, 1) },
                new ProjectMember { Id = 7, User = carol, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 3, 10) },
                new ProjectMember { Id = 8, User = dave, Role = MemberRole.Viewer, JoinedAt = new DateTime(2024, 3, 15) },
            ]
        };

        _projects = [projectApi, projectFe, projectInfra];
    }

    public List<Project> GetAll() => _projects;

    public Project? GetById(int id) => _projects.FirstOrDefault(p => p.Id == id);

    public Project Add(Project project)
    {
        project.Id = _projects.Count > 0 ? _projects.Max(p => p.Id) + 1 : 1;
        _projects.Add(project);
        return project;
    }

    public void Update(Project project)
    {
        var index = _projects.FindIndex(p => p.Id == project.Id);
        if (index >= 0) _projects[index] = project;
    }

    public void Delete(int id)
    {
        _projects.RemoveAll(p => p.Id == id);
    }

    public List<Project> Search(string query)
    {
        return _projects.Where(p => p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || p.Description.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
