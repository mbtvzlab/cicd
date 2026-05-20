using CiCd.Models;
using Microsoft.EntityFrameworkCore;

namespace CiCd.Data;

public class EfRunLogRepository : IRunLogRepository
{
    private readonly CiCdDbContext _context;

    public EfRunLogRepository(CiCdDbContext context)
    {
        _context = context;
    }

    public List<RunLog> GetAll()
    {
        return _context.RunLogs
            .Include(r => r.Pipeline)
                .ThenInclude(p => p.Project)
                    .ThenInclude(pr => pr.Owner)
            .Include(r => r.TriggeredBy)
            .Include(r => r.Artifacts)
            .Include(r => r.StepRuns)
                .ThenInclude(sr => sr.Step)
            .Where(r => r.DeletedAt == null)
            .ToList();
    }

    public RunLog? GetById(int id)
    {
        return _context.RunLogs
            .Include(r => r.Pipeline)
                .ThenInclude(p => p.Project)
                    .ThenInclude(pr => pr.Owner)
            .Include(r => r.TriggeredBy)
            .Include(r => r.Artifacts)
            .Include(r => r.StepRuns)
                .ThenInclude(sr => sr.Step)
            .FirstOrDefault(r => r.Id == id && r.DeletedAt == null);
    }

    public List<RunLog> GetByPipelineId(int pipelineId)
    {
        return _context.RunLogs
            .Include(r => r.Pipeline)
            .Include(r => r.TriggeredBy)
            .Include(r => r.Artifacts)
            .Where(r => r.PipelineId == pipelineId)
            .ToList();
    }

    public RunLog Add(RunLog runLog)
    {
        _context.RunLogs.Add(runLog);
        _context.SaveChanges();
        return runLog;
    }

    public void Update(RunLog runLog)
    {
        _context.RunLogs.Update(runLog);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var runLog = _context.RunLogs.Find(id);
        if (runLog != null)
        {
            runLog.DeletedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    public List<RunLog> Search(string query)
    {
        return _context.RunLogs
            .Include(r => r.Pipeline)
                .ThenInclude(p => p.Project)
            .Include(r => r.TriggeredBy)
            .Where(r => r.DeletedAt == null &&
                (r.Pipeline.Name.Contains(query) ||
                 r.TriggeredBy.Username.Contains(query) ||
                 r.Status.ToString().Contains(query)))
            .ToList();
    }
}