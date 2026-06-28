using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;
using CiCd.ViewModels;

namespace CiCd.Controllers;

[Route("pipelines")]
[Authorize(Roles = "Admin,Developer,Viewer")]
public class PipelineController : Controller
{
    private readonly IPipelineRepository _pipelineRepository;
    private readonly IProjectRepository _projectRepository;

    public PipelineController(IPipelineRepository pipelineRepository, IProjectRepository projectRepository)
    {
        _pipelineRepository = pipelineRepository;
        _projectRepository = projectRepository;
    }

    [HttpGet("")]
    public IActionResult Index(string? search)
    {
        var pipelines = string.IsNullOrEmpty(search)
            ? _pipelineRepository.GetAll()
            : _pipelineRepository.Search(search);
        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return Json(pipelines.Select(p => new {
                p.Id, p.Name, p.Branch, p.IsEnabled,
                Project = p.Project?.Name,
                Steps = p.Steps?.Count ?? 0,
                CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd")
            }));
        return View(pipelines);
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        var pipeline = _pipelineRepository.GetById(id);
        if (pipeline == null) return NotFound();
        return View(pipeline);
    }

    [HttpGet("{id:int}/steps")]
    public IActionResult Steps(int id)
    {
        var pipeline = _pipelineRepository.GetById(id);
        if (pipeline == null) return NotFound();
        return View(pipeline);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.Projects = _projectRepository.GetAll();
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Developer")]
    public IActionResult Create(PipelineCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Projects = _projectRepository.GetAll();
            return View(model);
        }
        var pipeline = new Pipeline
        {
            Name = model.Name,
            Description = model.Description,
            Branch = model.Branch,
            IsEnabled = model.IsEnabled,
            ProjectId = model.ProjectId,
            CreatedAt = DateTime.UtcNow
        };
        _pipelineRepository.Add(pipeline);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var pipeline = _pipelineRepository.GetById(id);
        if (pipeline == null) return NotFound();
        ViewBag.Projects = _projectRepository.GetAll();
        var model = new PipelineEditModel
        {
            Id = pipeline.Id,
            Name = pipeline.Name,
            Description = pipeline.Description,
            Branch = pipeline.Branch,
            IsEnabled = pipeline.IsEnabled,
            ProjectId = pipeline.ProjectId
        };
        return View(model);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Developer")]
    public IActionResult Edit(int id, PipelineEditModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Projects = _projectRepository.GetAll();
            return View(model);
        }
        var pipeline = _pipelineRepository.GetById(id);
        if (pipeline == null) return NotFound();
        pipeline.Name = model.Name;
        pipeline.Description = model.Description;
        pipeline.Branch = model.Branch;
        pipeline.IsEnabled = model.IsEnabled;
        pipeline.ProjectId = model.ProjectId;
        _pipelineRepository.Update(pipeline);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _pipelineRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("api/pipelines/search")]
    public IActionResult SearchPipelines(string q)
    {
        var pipelines = _pipelineRepository.Search(q ?? "");
        return Json(pipelines.Select(p => new { p.Id, p.Name, p.Branch, Project = p.Project?.Name }));
    }

    [HttpGet("api/projects/search")]
    public IActionResult SearchProjects(string q)
    {
        var projects = _projectRepository.Search(q ?? "");
        return Json(projects.Select(p => new { p.Id, p.Name }));
    }
}