namespace CiCd.Models;

public class Pipeline
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Branch { get; set; } = "main";
    public bool IsEnabled { get; set; }
    public DateTime CreatedAt { get; set; }

    public Project Project { get; set; } = null!;
    public List<PipelineStep> Steps { get; set; } = [];
    public List<RunLog> RunLogs { get; set; } = [];
}
