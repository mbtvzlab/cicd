using System.ComponentModel.DataAnnotations;

namespace CiCd.ViewModels;

public class PipelineEditModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(200)]
    public string Name { get; set; } = "";

    [MaxLength(500)]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Branch is required")]
    [MaxLength(100)]
    public string Branch { get; set; } = "main";

    public bool IsEnabled { get; set; }

    [Required(ErrorMessage = "Project is required")]
    public int ProjectId { get; set; }
}