using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;

namespace CiCd.Controllers;

public class PipelineController : Controller
{
    private readonly IPipelineRepository _pipelineRepository;

    public PipelineController(IPipelineRepository pipelineRepository)
    {
        _pipelineRepository = pipelineRepository;
    }

    public IActionResult Index()
    {
        var pipelines = _pipelineRepository.GetAll();
        return View(pipelines);
    }

    public IActionResult Details(int id)
    {
        var pipeline = _pipelineRepository.GetById(id);
        if (pipeline == null)
            return NotFound();

        return View(pipeline);
    }
}
