using CiCd.Models;

namespace CiCd.ViewModels;

public class DashboardViewModel
{
    public int TotalRuns { get; set; }
    public int Passed24h { get; set; }
    public int Failed24h { get; set; }
    public int RunningNow { get; set; }
    public List<RunLog> RecentRuns { get; set; } = [];
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int InactiveProjects { get; set; }
    public int TotalPipelines { get; set; }
    public int EnabledPipelines { get; set; }
    public int DisabledPipelines { get; set; }
}