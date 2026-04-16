using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;

namespace CiCd.Controllers;

public class ProjectController : Controller
{
    private readonly ProjectMockRepository _projectRepository;

    public ProjectController(ProjectMockRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public IActionResult Index()
    {
        var projects = _projectRepository.GetAll();
        return View(projects);
    }

    public IActionResult Details(int id)
    {
        var project = _projectRepository.GetById(id);
        if (project == null)
            return NotFound();

        return View(project);
    }
}
