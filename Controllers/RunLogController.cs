using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;
using CiCd.ViewModels;

namespace CiCd.Controllers;

[Route("runs")]
[Authorize(Roles = "Admin,Developer,Viewer")]
public class RunLogController : Controller
{
    private readonly IRunLogRepository _runLogRepository;
    private readonly IPipelineRepository _pipelineRepository;
    private readonly IUserRepository _userRepository;

    public RunLogController(IRunLogRepository runLogRepository, IPipelineRepository pipelineRepository, IUserRepository userRepository)
    {
        _runLogRepository = runLogRepository;
        _pipelineRepository = pipelineRepository;
        _userRepository = userRepository;
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

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var runLog = _runLogRepository.GetById(id);
        if (runLog == null) return NotFound();
        ViewBag.Pipelines = _pipelineRepository.GetAll();
        ViewBag.Users = _userRepository.GetAll();
        var model = new RunLogEditModel
        {
            Id = runLog.Id,
            Status = runLog.Status,
            PipelineId = runLog.PipelineId,
            TriggeredByUserId = runLog.TriggeredByUserId,
            StartedAt = runLog.StartedAt,
            FinishedAt = runLog.FinishedAt,
            TriggerType = runLog.TriggerType
        };
        return View(model);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Developer")]
    public IActionResult Edit(int id, RunLogEditModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Pipelines = _pipelineRepository.GetAll();
            ViewBag.Users = _userRepository.GetAll();
            return View(model);
        }
        var runLog = _runLogRepository.GetById(id);
        if (runLog == null) return NotFound();
        runLog.Status = model.Status;
        runLog.PipelineId = model.PipelineId;
        runLog.TriggeredByUserId = model.TriggeredByUserId;
        runLog.StartedAt = model.StartedAt;
        runLog.FinishedAt = model.FinishedAt;
        runLog.TriggerType = model.TriggerType;
        _runLogRepository.Update(runLog);
        return RedirectToAction(nameof(Details), new { id });
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