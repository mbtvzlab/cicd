using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;
using CiCd.ViewModels;

namespace CiCd.Controllers;

[Authorize(Roles = "Admin,Developer,Viewer")]
public class ProjectController : Controller
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;

    public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
    }

    public IActionResult Index(string? search)
    {
        var projects = string.IsNullOrEmpty(search)
            ? _projectRepository.GetAll()
            : _projectRepository.Search(search);
        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return Json(projects.Select(p => new {
                p.Id, p.Name, p.Description, p.IsActive,
                Owner = p.Owner?.Username,
                Members = p.Members?.Count ?? 0,
                CreatedAt = p.CreatedAt.ToString("yyyy-MM-dd")
            }));
        return View(projects);
    }

    public IActionResult Details(int id)
    {
        var project = _projectRepository.GetById(id);
        if (project == null) return NotFound();
        return View(project);
    }

    public IActionResult Create()
    {
        ViewBag.Users = _userRepository.GetAll();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Developer")]
    public IActionResult Create(ProjectCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Users = _userRepository.GetAll();
            return View(model);
        }
        var project = new Project
        {
            Name = model.Name,
            Description = model.Description,
            RepositoryUrl = model.RepositoryUrl,
            IsActive = model.IsActive,
            OwnerId = model.OwnerId,
            CreatedAt = DateTime.UtcNow
        };
        _projectRepository.Add(project);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var project = _projectRepository.GetById(id);
        if (project == null) return NotFound();
        ViewBag.Users = _userRepository.GetAll();
        var model = new ProjectEditModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            RepositoryUrl = project.RepositoryUrl,
            IsActive = project.IsActive,
            OwnerId = project.OwnerId
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<IActionResult> Edit(int id, ProjectEditModel model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.Users = _userRepository.GetAll();
            return View(model);
        }
        var project = _projectRepository.GetById(id);
        if (project == null) return NotFound();
        project.Name = model.Name;
        project.Description = model.Description;
        project.RepositoryUrl = model.RepositoryUrl;
        project.IsActive = model.IsActive;
        project.OwnerId = model.OwnerId;
        _projectRepository.Update(project);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        _projectRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("api/users/search")]
    public IActionResult SearchUsers(string q)
    {
        var users = _userRepository.Search(q ?? "");
        return Json(users.Select(u => new { u.Id, u.Username, u.Email }));
    }
}