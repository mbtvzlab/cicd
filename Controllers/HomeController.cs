using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;
using CiCd.ViewModels;

namespace CiCd.Controllers;

public class HomeController : Controller
{
    private readonly IProjectRepository _projectRepository;
    private readonly IPipelineRepository _pipelineRepository;
    private readonly IRunLogRepository _runLogRepository;

    public HomeController(
        IProjectRepository projectRepository,
        IPipelineRepository pipelineRepository,
        IRunLogRepository runLogRepository)
    {
        _projectRepository = projectRepository;
        _pipelineRepository = pipelineRepository;
        _runLogRepository = runLogRepository;
    }

    public IActionResult Index()
    {
        var projects = _projectRepository.GetAll();
        var pipelines = _pipelineRepository.GetAll();
        var runLogs = _runLogRepository.GetAll();

        var now = DateTime.UtcNow;
        var last24h = now.AddHours(-24);

        var viewModel = new DashboardViewModel
        {
            TotalRuns = runLogs.Count,
            Passed24h = runLogs.Count(r => r.Status == RunStatus.Success && r.StartedAt >= last24h),
            Failed24h = runLogs.Count(r => r.Status == RunStatus.Failed && r.StartedAt >= last24h),
            RunningNow = runLogs.Count(r => r.Status == RunStatus.Running),
            RecentRuns = runLogs.OrderByDescending(r => r.StartedAt).ToList(),
            TotalProjects = projects.Count,
            ActiveProjects = projects.Count(p => p.IsActive),
            InactiveProjects = projects.Count(p => !p.IsActive),
            TotalPipelines = pipelines.Count,
            EnabledPipelines = pipelines.Count(p => p.IsEnabled),
            DisabledPipelines = pipelines.Count(p => !p.IsEnabled),
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
