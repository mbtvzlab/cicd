using System.ComponentModel.DataAnnotations;

namespace CiCd.ViewModels;

public class ProjectEditModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [MaxLength(1000)]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Repository URL is required")]
    [MaxLength(500)]
    public string RepositoryUrl { get; set; } = "";

    public bool IsActive { get; set; }

    [Required(ErrorMessage = "Owner is required")]
    public int OwnerId { get; set; }
}