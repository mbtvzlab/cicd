using CiCd.Models;
using Microsoft.EntityFrameworkCore;

namespace CiCd.Data;

public class EfProjectRepository : IProjectRepository
{
    private readonly CiCdDbContext _context;

    public EfProjectRepository(CiCdDbContext context)
    {
        _context = context;
    }

    public List<Project> GetAll()
    {
        return _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
                .ThenInclude(m => m.User)
            .Include(p => p.Pipelines)
                .ThenInclude(p => p.Steps)
            .Include(p => p.Pipelines)
                .ThenInclude(p => p.RunLogs)
                    .ThenInclude(r => r.TriggeredBy)
            .Include(p => p.Pipelines)
                .ThenInclude(p => p.RunLogs)
                    .ThenInclude(r => r.Artifacts)
            .Where(p => p.DeletedAt == null)
            .ToList();
    }

    public Project? GetById(int id)
    {
        return _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
                .ThenInclude(m => m.User)
            .Include(p => p.Pipelines)
                .ThenInclude(p => p.Steps)
            .Include(p => p.Pipelines)
                .ThenInclude(p => p.RunLogs)
                    .ThenInclude(r => r.TriggeredBy)
            .Include(p => p.Pipelines)
                .ThenInclude(p => p.RunLogs)
                    .ThenInclude(r => r.Artifacts)
            .FirstOrDefault(p => p.Id == id && p.DeletedAt == null);
    }

    public Project Add(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }

    public void Update(Project project)
    {
        _context.Projects.Update(project);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var project = _context.Projects.Find(id);
        if (project != null)
        {
            project.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    public List<Project> Search(string query)
    {
        return _context.Projects
            .Include(p => p.Owner)
            .Include(p => p.Members)
                .ThenInclude(m => m.User)
            .Where(p => p.DeletedAt == null &&
                (p.Name.Contains(query) || p.Description.Contains(query)))
            .ToList();
    }
}