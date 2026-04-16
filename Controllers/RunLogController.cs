using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;

namespace CiCd.Controllers;

public class RunLogController : Controller
{
    private readonly RunLogMockRepository _runLogRepository;

    public RunLogController(RunLogMockRepository runLogRepository)
    {
        _runLogRepository = runLogRepository;
    }

    public IActionResult Index()
    {
        var runLogs = _runLogRepository.GetAll();
        return View(runLogs);
    }

    public IActionResult Details(int id)
    {
        var runLog = _runLogRepository.GetById(id);
        if (runLog == null)
            return NotFound();

        return View(runLog);
    }
}
