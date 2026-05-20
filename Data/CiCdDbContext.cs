using Microsoft.EntityFrameworkCore;
using CiCd.Models;

namespace CiCd.Data;

public class CiCdDbContext : DbContext
{
    public CiCdDbContext(DbContextOptions<CiCdDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
    public DbSet<Pipeline> Pipelines { get; set; } = null!;
    public DbSet<PipelineStep> PipelineSteps { get; set; } = null!;
    public DbSet<RunLog> RunLogs { get; set; } = null!;
    public DbSet<StepRun> StepRuns { get; set; } = null!;
    public DbSet<Artifact> Artifacts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasOne(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Members)
                .WithOne(m => m.Project)
                .HasForeignKey(m => m.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Pipelines)
                .WithOne(p => p.Project)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Pipeline>(entity =>
        {
            entity.HasMany(e => e.Steps)
                .WithOne(s => s.Pipeline)
                .HasForeignKey(s => s.PipelineId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.RunLogs)
                .WithOne(r => r.Pipeline)
                .HasForeignKey(r => r.PipelineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PipelineStep>(entity =>
        {
            entity.HasOne(e => e.LogArtifact)
                .WithOne()
                .HasForeignKey<PipelineStep>(e => e.LogArtifactId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<RunLog>(entity =>
        {
            entity.HasOne(e => e.TriggeredBy)
                .WithMany()
                .HasForeignKey(e => e.TriggeredByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Artifacts)
                .WithOne(a => a.RunLog)
                .HasForeignKey(a => a.RunLogId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.StepRuns)
                .WithOne(s => s.RunLog)
                .HasForeignKey(s => s.RunLogId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StepRun>(entity =>
        {
            entity.HasOne(e => e.Step)
                .WithMany()
                .HasForeignKey(e => e.StepId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.LogArtifact)
                .WithOne()
                .HasForeignKey<StepRun>(e => e.LogArtifactId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "alice", Email = "alice@example.com", PasswordHash = "hash1", CreatedAt = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Utc), Role = UserRole.Admin },
            new User { Id = 2, Username = "bob", Email = "bob@example.com", PasswordHash = "hash2", CreatedAt = new DateTime(2024, 2, 14, 0, 0, 0, DateTimeKind.Utc), Role = UserRole.Developer },
            new User { Id = 3, Username = "carol", Email = "carol@example.com", PasswordHash = "hash3", CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc), Role = UserRole.Developer },
            new User { Id = 4, Username = "dave", Email = "dave@example.com", PasswordHash = "hash4", CreatedAt = new DateTime(2024, 4, 20, 0, 0, 0, DateTimeKind.Utc), Role = UserRole.Viewer }
        );

        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "REST API", Description = "Backend REST API service", RepositoryUrl = "https://github.com/example/rest-api", CreatedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), IsActive = true, OwnerId = 1 },
            new Project { Id = 2, Name = "Frontend", Description = "React/TypeScript frontend application", RepositoryUrl = "https://github.com/example/frontend", CreatedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = true, OwnerId = 3 },
            new Project { Id = 3, Name = "Infrastructure", Description = "Terraform configs and deployment scripts", RepositoryUrl = "https://github.com/example/infra", CreatedAt = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc), IsActive = false, OwnerId = 1 }
        );

        modelBuilder.Entity<ProjectMember>().HasData(
            new ProjectMember { Id = 1, UserId = 1, ProjectId = 1, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 2, UserId = 2, ProjectId = 1, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 3, UserId = 4, ProjectId = 1, Role = MemberRole.Viewer, JoinedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 4, UserId = 3, ProjectId = 2, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 5, UserId = 2, ProjectId = 2, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 2, 5, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 6, UserId = 1, ProjectId = 3, Role = MemberRole.Owner, JoinedAt = new DateTime(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 7, UserId = 3, ProjectId = 3, Role = MemberRole.Contributor, JoinedAt = new DateTime(2024, 3, 10, 0, 0, 0, DateTimeKind.Utc) },
            new ProjectMember { Id = 8, UserId = 4, ProjectId = 3, Role = MemberRole.Viewer, JoinedAt = new DateTime(2024, 3, 15, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<Artifact>().HasData(
            new Artifact { Id = 1, FileName = "build.log", BlobUrl = "https://s3.example.com/logs/1/build.log", SizeBytes = 4096, CreatedAt = new DateTime(2024, 5, 1, 10, 5, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 2, FileName = "test.log", BlobUrl = "https://s3.example.com/logs/1/test.log", SizeBytes = 8192, CreatedAt = new DateTime(2024, 5, 1, 10, 6, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 3, FileName = "deploy.log", BlobUrl = "https://s3.example.com/logs/1/deploy.log", SizeBytes = 1024, CreatedAt = new DateTime(2024, 5, 1, 10, 7, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 4, FileName = "lint.log", BlobUrl = "https://s3.example.com/logs/2/lint.log", SizeBytes = 2048, CreatedAt = new DateTime(2024, 5, 3, 14, 1, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 5, FileName = "build.log", BlobUrl = "https://s3.example.com/logs/2/build.log", SizeBytes = 6144, CreatedAt = new DateTime(2024, 5, 3, 14, 3, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 6, FileName = "validate.log", BlobUrl = "https://s3.example.com/logs/3/validate.log", SizeBytes = 512, CreatedAt = new DateTime(2024, 4, 10, 8, 1, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 7, FileName = "plan.log", BlobUrl = "https://s3.example.com/logs/3/plan.log", SizeBytes = 3072, CreatedAt = new DateTime(2024, 4, 10, 8, 3, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 8, FileName = "apply.log", BlobUrl = "https://s3.example.com/logs/3/apply.log", SizeBytes = 2048, CreatedAt = new DateTime(2024, 4, 10, 8, 6, 0, DateTimeKind.Utc), RunLogId = null },
            new Artifact { Id = 10, FileName = "api.zip", BlobUrl = "https://s3.example.com/artifacts/api.zip", SizeBytes = 512000, CreatedAt = new DateTime(2024, 5, 1, 10, 8, 0, DateTimeKind.Utc), RunLogId = 1 },
            new Artifact { Id = 11, FileName = "dist.zip", BlobUrl = "https://s3.example.com/artifacts/dist.zip", SizeBytes = 204800, CreatedAt = new DateTime(2024, 5, 3, 14, 5, 0, DateTimeKind.Utc), RunLogId = 3 }
        );

        modelBuilder.Entity<Pipeline>().HasData(
            new Pipeline { Id = 1, Name = "CI Pipeline", Description = "Build and test on every push", Branch = "main", IsEnabled = true, CreatedAt = new DateTime(2024, 1, 20, 0, 0, 0, DateTimeKind.Utc), ProjectId = 1 },
            new Pipeline { Id = 2, Name = "Frontend CI", Description = "Lint and build the frontend bundle", Branch = "main", IsEnabled = true, CreatedAt = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), ProjectId = 2 },
            new Pipeline { Id = 3, Name = "Terraform Apply", Description = "Validate, plan and apply infrastructure changes", Branch = "main", IsEnabled = false, CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc), ProjectId = 3 }
        );

        modelBuilder.Entity<PipelineStep>().HasData(
            new PipelineStep { Id = 1, Name = "Build", Command = "dotnet build", Order = 1, TimeoutSeconds = 120, ContinueOnError = false, PipelineId = 1, LogArtifactId = 1 },
            new PipelineStep { Id = 2, Name = "Test", Command = "dotnet test", Order = 2, TimeoutSeconds = 180, ContinueOnError = false, PipelineId = 1, LogArtifactId = 2 },
            new PipelineStep { Id = 3, Name = "Deploy", Command = "./deploy.sh", Order = 3, TimeoutSeconds = 60, ContinueOnError = false, PipelineId = 1, LogArtifactId = 3 },
            new PipelineStep { Id = 4, Name = "Lint", Command = "npm run lint", Order = 1, TimeoutSeconds = 60, ContinueOnError = false, PipelineId = 2, LogArtifactId = 4 },
            new PipelineStep { Id = 5, Name = "Build", Command = "npm run build", Order = 2, TimeoutSeconds = 120, ContinueOnError = false, PipelineId = 2, LogArtifactId = 5 },
            new PipelineStep { Id = 6, Name = "Validate", Command = "terraform validate", Order = 1, TimeoutSeconds = 30, ContinueOnError = false, PipelineId = 3, LogArtifactId = 6 },
            new PipelineStep { Id = 7, Name = "Plan", Command = "terraform plan", Order = 2, TimeoutSeconds = 120, ContinueOnError = false, PipelineId = 3, LogArtifactId = 7 },
            new PipelineStep { Id = 8, Name = "Apply", Command = "terraform apply", Order = 3, TimeoutSeconds = 300, ContinueOnError = false, PipelineId = 3, LogArtifactId = 8 }
        );

        modelBuilder.Entity<RunLog>().HasData(
            new RunLog { Id = 1, PipelineId = 1, TriggeredByUserId = 2, StartedAt = new DateTime(2024, 5, 1, 10, 0, 0, DateTimeKind.Utc), FinishedAt = new DateTime(2024, 5, 1, 10, 8, 0, DateTimeKind.Utc), Status = RunStatus.Success, TriggerType = TriggerType.Push },
            new RunLog { Id = 2, PipelineId = 1, TriggeredByUserId = 1, StartedAt = new DateTime(2024, 5, 2, 9, 0, 0, DateTimeKind.Utc), FinishedAt = new DateTime(2024, 5, 2, 9, 3, 0, DateTimeKind.Utc), Status = RunStatus.Failed, TriggerType = TriggerType.PullRequest },
            new RunLog { Id = 3, PipelineId = 2, TriggeredByUserId = 3, StartedAt = new DateTime(2024, 5, 3, 14, 0, 0, DateTimeKind.Utc), FinishedAt = new DateTime(2024, 5, 3, 14, 5, 0, DateTimeKind.Utc), Status = RunStatus.Success, TriggerType = TriggerType.Push },
            new RunLog { Id = 4, PipelineId = 2, TriggeredByUserId = 2, StartedAt = new DateTime(2024, 5, 4, 9, 30, 0, DateTimeKind.Utc), FinishedAt = null, Status = RunStatus.Running, TriggerType = TriggerType.Manual },
            new RunLog { Id = 5, PipelineId = 3, TriggeredByUserId = 1, StartedAt = new DateTime(2024, 4, 10, 8, 0, 0, DateTimeKind.Utc), FinishedAt = new DateTime(2024, 4, 10, 8, 7, 0, DateTimeKind.Utc), Status = RunStatus.Success, TriggerType = TriggerType.Manual },
            new RunLog { Id = 6, PipelineId = 3, TriggeredByUserId = 1, StartedAt = new DateTime(2024, 4, 15, 12, 0, 0, DateTimeKind.Utc), FinishedAt = new DateTime(2024, 4, 15, 12, 2, 0, DateTimeKind.Utc), Status = RunStatus.Cancelled, TriggerType = TriggerType.Schedule }
        );
    }
}
