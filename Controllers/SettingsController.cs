using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CiCd.Data;
using CiCd.Models;
using CiCd.ViewModels;

namespace CiCd.Controllers;

[Authorize]
[Route("settings")]
public class SettingsController : Controller
{
    private readonly IUserRepository _userRepository;

    public SettingsController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Challenge();

        var user = _userRepository.GetById(userId.Value);
        if (user == null) return NotFound();

        var model = new SettingsViewModel
        {
            Id = user.Id,
            Username = user.Username,
            DisplayName = user.DisplayName,
            Email = user.Email
        };

        return View(model);
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SettingsViewModel model)
    {
        var userId = GetCurrentUserId();
        if (userId == null || userId.Value != model.Id)
        {
            return Forbid();
        }

        // Only validate password fields if the user is changing their password.
        if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword))
        {
            if (string.IsNullOrEmpty(model.CurrentPassword))
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "Current password is required to set a new password.");
            }
        }

        // Clear password-related errors when not changing password so ModelState remains valid.
        if (string.IsNullOrEmpty(model.NewPassword))
        {
            ModelState.Remove(nameof(model.CurrentPassword));
            ModelState.Remove(nameof(model.NewPassword));
            ModelState.Remove(nameof(model.ConfirmPassword));
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _userRepository.GetById(model.Id);
        if (user == null) return NotFound();

        if (!string.IsNullOrEmpty(model.NewPassword))
        {
            if (string.IsNullOrEmpty(model.CurrentPassword) ||
                !BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "Current password is incorrect.");
                return View(model);
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        }

        user.DisplayName = model.DisplayName;
        user.Email = model.Email;
        _userRepository.Update(user);

        await RefreshAuthCookieAsync(user);

        TempData["SuccessMessage"] = "Settings saved successfully.";
        return RedirectToAction(nameof(Index));
    }

    private int? GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(value, out var id))
        {
            return id;
        }
        return null;
    }

    private async Task RefreshAuthCookieAsync(User user)
    {
        var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!authResult.Succeeded) return;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.GivenName, user.DisplayName ?? user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            authResult.Properties);
    }
}
