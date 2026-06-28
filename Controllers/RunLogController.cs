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

    public RunLogController(IRunLogRepository runLogRepository)
    {
        _runLogRepository = runLogRepository;
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
        return RedirectToAction(nameof(Details), new { id = newRun.Id });
    }
}