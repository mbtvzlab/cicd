using CiCd.Models;
using Microsoft.EntityFrameworkCore;

namespace CiCd.Data;

public class EfProjectRepository : IProjectRepository
{
    private readonly CiCdDbContext _context;
    private readonly ILogger<EfProjectRepository> _logger;

    public EfProjectRepository(CiCdDbContext context, ILogger<EfProjectRepository> logger)
    {
        _context = context;
        _logger = logger;
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
        try
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add project '{Name}'", project.Name);
            throw;
        }
    }

    public void Update(Project project)
    {
        try
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update project id={Id}", project.Id);
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var project = _context.Projects.Find(id);
            if (project != null)
            {
                project.DeletedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete project id={Id}", id);
            throw;
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