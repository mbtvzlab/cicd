using System.ComponentModel.DataAnnotations;

namespace CiCd.ViewModels;

public class SettingsViewModel
{
    public int Id { get; set; }

    public string Username { get; set; } = "";

    [Display(Name = "Display name")]
    [MaxLength(200, ErrorMessage = "Display name cannot exceed 200 characters.")]
    public string? DisplayName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    public string Email { get; set; } = "";

    [Display(Name = "Current password")]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [Display(Name = "New password")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string? NewPassword { get; set; }

    [Display(Name = "Confirm new password")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match.")]
    public string? ConfirmPassword { get; set; }
}
