namespace CiCd.Models;

public class Artifact
{
    public int Id { get; set; }
    public string FileName { get; set; } = "";
    public string BlobUrl { get; set; } = "";
    public long SizeBytes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Null when the artifact is a step log (owned by PipelineStep, not a RunLog)
    public RunLog? RunLog { get; set; }
}
