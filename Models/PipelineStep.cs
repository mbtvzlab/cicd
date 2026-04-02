namespace CiCd.Models;

public class PipelineStep
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Command { get; set; } = "";
    public int Order { get; set; }
    public int TimeoutSeconds { get; set; }
    public bool ContinueOnError { get; set; }

    public Pipeline Pipeline { get; set; } = null!;

    // Log output (stdout/stderr) produced when this step runs
    public Artifact? LogArtifact { get; set; }
}
