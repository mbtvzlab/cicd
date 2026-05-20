using System.ComponentModel.DataAnnotations;
using CiCd.Models;

namespace CiCd.ViewModels;

public class RunLogEditModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public RunStatus Status { get; set; }

    [Required(ErrorMessage = "Pipeline is required")]
    public int PipelineId { get; set; }

    [Required(ErrorMessage = "Triggered by is required")]
    public int TriggeredByUserId { get; set; }

    [Required(ErrorMessage = "Started at is required")]
    public DateTime StartedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    [Required(ErrorMessage = "Trigger type is required")]
    public TriggerType TriggerType { get; set; }
}