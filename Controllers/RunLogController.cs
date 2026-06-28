using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;

namespace CiCd.Controllers;

[Route("runs")]
[Authorize(Roles = "Admin,Developer,Viewer")]
public class RunLogController : Controller
{
    private readonly IRunLogRepository _runLogRepository;
    private readonly ILogger<RunLogController> _logger;

    public RunLogController(IRunLogRepository runLogRepository, ILogger<RunLogController> logger)
    {
        _runLogRepository = runLogRepository;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string? search)
    {
        var runLogs = string.IsNullOrEmpty(search)
            ? _runLogRepository.GetAll()
            : _runLogRepository.Search(search);
        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return Json(runLogs.OrderByDescending(r => r.StartedAt).Select(r => new {
                r.Id, r.Status, r.TriggerType,
                Pipeline = r.Pipeline?.Name,
                TriggeredBy = r.TriggeredBy?.Username,
                StartedAt = r.StartedAt.ToString("yyyy-MM-dd HH:mm"),
                Duration = r.Duration?.TotalMinutes.ToString("F1") + " min"
            }));
        return View(runLogs.OrderByDescending(r => r.StartedAt).ToList());
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var runLog = _runLogRepository.GetById(id);
        if (runLog == null) return NotFound();
        return View(runLog);
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _logger.LogWarning("RunLog id={Id} deleted by {Username}", id, User.Identity?.Name);
        _runLogRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/retrigger")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Developer")]
    public IActionResult Retrigger(int id)
    {
        var runLog = _runLogRepository.GetById(id);
        if (runLog == null) return NotFound();
        var newRun = new RunLog
        {
            PipelineId = runLog.PipelineId,
            TriggeredByUserId = runLog.TriggeredByUserId,
            StartedAt = DateTime.UtcNow,
            Status = RunStatus.Pending,
            TriggerType = TriggerType.Manual
        };
        _runLogRepository.Add(newRun);
        _logger.LogInformation("Pipeline id={PipelineId} retriggered by {Username}, new run id={NewRunId}", runLog.PipelineId, User.Identity?.Name, newRun.Id);
        return RedirectToAction(nameof(Details), new { id = newRun.Id });
    }
}