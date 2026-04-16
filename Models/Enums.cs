namespace CiCd.Models;

public enum UserRole
{
    Admin,
    Developer,
    Viewer
}

public enum MemberRole
{
    Owner,
    Contributor,
    Viewer
}

public enum RunStatus
{
    Pending,
    Running,
    Success,
    Failed,
    Cancelled
}

public enum TriggerType
{
    Manual,
    Push,
    Schedule,
    PullRequest
}

public enum StepRunStatus
{
    Pending,
    Running,
    Success,
    Failed,
    Cancelled,
    Skipped
}
