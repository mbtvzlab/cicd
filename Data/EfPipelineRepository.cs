using CiCd.Models;
using Microsoft.EntityFrameworkCore;

namespace CiCd.Data;

public class EfPipelineRepository : IPipelineRepository
{
    private readonly CiCdDbContext _context;
    private readonly ILogger<EfPipelineRepository> _logger;

    public EfPipelineRepository(CiCdDbContext context, ILogger<EfPipelineRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public List<Pipeline> GetAll()
    {
        return _context.Pipelines
            .Include(p => p.Project)
                .ThenInclude(pr => pr.Owner)
            .Include(p => p.Steps)
            .Include(p => p.RunLogs)
                .ThenInclude(r => r.TriggeredBy)
            .Include(p => p.RunLogs)
                .ThenInclude(r => r.Artifacts)
            .Where(p => p.DeletedAt == null)
            .ToList();
    }

    public Pipeline? GetById(int id)
    {
        return _context.Pipelines
            .Include(p => p.Project)
                .ThenInclude(pr => pr.Owner)
            .Include(p => p.Steps)
            .Include(p => p.RunLogs)
                .ThenInclude(r => r.TriggeredBy)
            .Include(p => p.RunLogs)
                .ThenInclude(r => r.Artifacts)
            .FirstOrDefault(p => p.Id == id && p.DeletedAt == null);
    }

    public List<Pipeline> GetByProjectId(int projectId)
    {
        return _context.Pipelines
            .Include(p => p.Project)
            .Include(p => p.Steps)
            .Include(p => p.RunLogs)
                .ThenInclude(r => r.TriggeredBy)
            .Where(p => p.ProjectId == projectId)
            .ToList();
    }

    public Pipeline Add(Pipeline pipeline)
    {
        try
        {
            _context.Pipelines.Add(pipeline);
            _context.SaveChanges();
            return pipeline;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add pipeline '{Name}' for project {ProjectId}", pipeline.Name, pipeline.ProjectId);
            throw;
        }
    }

    public void Update(Pipeline pipeline)
    {
        try
        {
            _context.Pipelines.Update(pipeline);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update pipeline id={Id}", pipeline.Id);
            throw;
        }
    }

    public void Delete(int id)
    {
        try
        {
            var pipeline = _context.Pipelines.Find(id);
            if (pipeline != null)
            {
                pipeline.DeletedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete pipeline id={Id}", id);
            throw;
        }
    }

    public List<Pipeline> Search(string query)
    {
        return _context.Pipelines
            .Include(p => p.Project)
                .ThenInclude(pr => pr.Owner)
            .Include(p => p.Steps)
            .Where(p => p.DeletedAt == null &&
                (p.Name.Contains(query) || p.Branch.Contains(query)))
            .ToList();
    }
}