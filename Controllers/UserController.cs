using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;

namespace CiCd.Controllers;

[Route("users")]
[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string? search)
    {
        var users = string.IsNullOrEmpty(search)
            ? _userRepository.GetAll()
            : _userRepository.Search(search);
        if (Request.Headers.XRequestedWith == "XMLHttpRequest")
            return Json(users.Select(u => new {
                u.Id, u.Username, u.Email,
                Role = u.Role.ToString(),
                CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd")
            }));
        return View(users);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(User user)
    {
        if (!ModelState.IsValid) return View(user);
        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = string.IsNullOrEmpty(user.Password)
            ? BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString())
            : BCrypt.Net.BCrypt.HashPassword(user.Password);
        _userRepository.Add(user);
        _logger.LogInformation("User '{Username}' (id={Id}) created", user.Username, user.Id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("{id:int}/edit")]
    public IActionResult Edit(int id)
    {
        var user = _userRepository.GetById(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost("{id:int}/edit")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, User user)
    {
        if (id != user.Id) return BadRequest();
        if (!ModelState.IsValid) return View(user);
        var existing = _userRepository.GetById(id);
        if (existing == null) return NotFound();
        existing.Username = user.Username;
        existing.Email = user.Email;
        existing.Role = user.Role;
        if (!string.IsNullOrEmpty(user.Password))
        {
            existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }
        _userRepository.Update(existing);
        _logger.LogInformation("User '{Username}' (id={Id}) updated", existing.Username, existing.Id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _logger.LogWarning("User id={Id} deleted by {Username}", id, User.Identity?.Name);
        _userRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("api/users/search")]
    public IActionResult SearchUsers(string q)
    {
        var users = _userRepository.Search(q ?? "");
        return Json(users.Select(u => new { u.Id, u.Username, u.Email }));
    }
}