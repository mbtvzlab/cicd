using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;

namespace CiCd.Controllers;

[Route("users")]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
        user.PasswordHash = Guid.NewGuid().ToString();
        _userRepository.Add(user);
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
        _userRepository.Update(existing);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("{id:int}/delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
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