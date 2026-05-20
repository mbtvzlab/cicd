using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CiCd.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RepositoryUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pipelines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Branch = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pipelines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pipelines_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JoinedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RunLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TriggerType = table.Column<int>(type: "integer", nullable: false),
                    PipelineId = table.Column<int>(type: "integer", nullable: false),
                    TriggeredByUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RunLogs_Pipelines_PipelineId",
                        column: x => x.PipelineId,
                        principalTable: "Pipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RunLogs_Users_TriggeredByUserId",
                        column: x => x.TriggeredByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Artifacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    BlobUrl = table.Column<string>(type: "text", nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RunLogId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artifacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artifacts_RunLogs_RunLogId",
                        column: x => x.RunLogId,
                        principalTable: "RunLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PipelineSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Command = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TimeoutSeconds = table.Column<int>(type: "integer", nullable: false),
                    ContinueOnError = table.Column<bool>(type: "boolean", nullable: false),
                    PipelineId = table.Column<int>(type: "integer", nullable: false),
                    LogArtifactId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PipelineSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PipelineSteps_Artifacts_LogArtifactId",
                        column: x => x.LogArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PipelineSteps_Pipelines_PipelineId",
                        column: x => x.PipelineId,
                        principalTable: "Pipelines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StepRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StepId = table.Column<int>(type: "integer", nullable: false),
                    RunLogId = table.Column<int>(type: "integer", nullable: false),
                    ExecutionOrder = table.Column<int>(type: "integer", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ExitCode = table.Column<int>(type: "integer", nullable: true),
                    ConsoleOutput = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    LogArtifactId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepRuns_Artifacts_LogArtifactId",
                        column: x => x.LogArtifactId,
                        principalTable: "Artifacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StepRuns_PipelineSteps_StepId",
                        column: x => x.StepId,
                        principalTable: "PipelineSteps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StepRuns_RunLogs_RunLogId",
                        column: x => x.RunLogId,
                        principalTable: "RunLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artifacts",
                columns: new[] { "Id", "BlobUrl", "CreatedAt", "FileName", "RunLogId", "SizeBytes" },
                values: new object[,]
                {
                    { 1, "https://s3.example.com/logs/1/build.log", new DateTime(2024, 5, 1, 10, 5, 0, 0, DateTimeKind.Utc), "build.log", null, 4096L },
                    { 2, "https://s3.example.com/logs/1/test.log", new DateTime(2024, 5, 1, 10, 6, 0, 0, DateTimeKind.Utc), "test.log", null, 8192L },
                    { 3, "https://s3.example.com/logs/1/deploy.log", new DateTime(2024, 5, 1, 10, 7, 0, 0, DateTimeKind.Utc), "deploy.log", null, 1024L },
                    { 4, "https://s3.example.com/logs/2/lint.log", new DateTime(2024, 5, 3, 14, 1, 0, 0, DateTimeKind.Utc), "lint.log", null, 2048L },
                    { 5, "https://s3.example.com/logs/2/build.log", new DateTime(2024, 5, 3, 14, 3, 0, 0, DateTimeKind.Utc), "build.log", null, 6144L },
                    { 6, "https://s3.example.com/logs/3/validate.log", new DateTime(2024, 4, 10, 8, 1, 0, 0, DateTimeKind.Utc), "validate.log", null, 512L },
                    { 7, "https://s3.example.com/logs/3/plan.log", new DateTime(2024, 4, 10, 8, 3, 0, 0, DateTimeKind.Utc), "plan.log", null, 3072L },
                    { 8, "https://s3.example.com/logs/3/apply.log", new DateTime(2024, 4, 10, 8, 6, 0, 0, DateTimeKind.Utc), "apply.log", null, 2048L }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "alice@example.com", "hash1", 0, "alice" },
                    { 2, new DateTime(2024, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), "bob@example.com", "hash2", 1, "bob" },
                    { 3, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "carol@example.com", "hash3", 1, "carol" },
                    { 4, new DateTime(2024, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), "dave@example.com", "hash4", 2, "dave" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Name", "OwnerId", "RepositoryUrl" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Backend REST API service", true, "REST API", 1, "https://github.com/example/rest-api" },
                    { 2, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "React/TypeScript frontend application", true, "Frontend", 3, "https://github.com/example/frontend" },
                    { 3, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Terraform configs and deployment scripts", false, "Infrastructure", 1, "https://github.com/example/infra" }
                });

            migrationBuilder.InsertData(
                table: "Pipelines",
                columns: new[] { "Id", "Branch", "CreatedAt", "Description", "IsEnabled", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 1, "main", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Build and test on every push", true, "CI Pipeline", 1 },
                    { 2, "main", new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Lint and build the frontend bundle", true, "Frontend CI", 2 },
                    { 3, "main", new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Validate, plan and apply infrastructure changes", false, "Terraform Apply", 3 }
                });

            migrationBuilder.InsertData(
                table: "ProjectMembers",
                columns: new[] { "Id", "JoinedAt", "ProjectId", "Role", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 0, 1 },
                    { 2, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, 2 },
                    { 3, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, 4 },
                    { 4, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, 0, 3 },
                    { 5, new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, 2 },
                    { 6, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 0, 1 },
                    { 7, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, 1, 3 },
                    { 8, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2, 4 }
                });

            migrationBuilder.InsertData(
                table: "PipelineSteps",
                columns: new[] { "Id", "Command", "ContinueOnError", "LogArtifactId", "Name", "Order", "PipelineId", "TimeoutSeconds" },
                values: new object[,]
                {
                    { 1, "dotnet build", false, 1, "Build", 1, 1, 120 },
                    { 2, "dotnet test", false, 2, "Test", 2, 1, 180 },
                    { 3, "./deploy.sh", false, 3, "Deploy", 3, 1, 60 },
                    { 4, "npm run lint", false, 4, "Lint", 1, 2, 60 },
                    { 5, "npm run build", false, 5, "Build", 2, 2, 120 },
                    { 6, "terraform validate", false, 6, "Validate", 1, 3, 30 },
                    { 7, "terraform plan", false, 7, "Plan", 2, 3, 120 },
                    { 8, "terraform apply", false, 8, "Apply", 3, 3, 300 }
                });

            migrationBuilder.InsertData(
                table: "RunLogs",
                columns: new[] { "Id", "FinishedAt", "PipelineId", "StartedAt", "Status", "TriggerType", "TriggeredByUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 5, 1, 10, 8, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 5, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 1, 2 },
                    { 2, new DateTime(2024, 5, 2, 9, 3, 0, 0, DateTimeKind.Utc), 1, new DateTime(2024, 5, 2, 9, 0, 0, 0, DateTimeKind.Utc), 3, 3, 1 },
                    { 3, new DateTime(2024, 5, 3, 14, 5, 0, 0, DateTimeKind.Utc), 2, new DateTime(2024, 5, 3, 14, 0, 0, 0, DateTimeKind.Utc), 2, 1, 3 },
                    { 4, null, 2, new DateTime(2024, 5, 4, 9, 30, 0, 0, DateTimeKind.Utc), 1, 0, 2 },
                    { 5, new DateTime(2024, 4, 10, 8, 7, 0, 0, DateTimeKind.Utc), 3, new DateTime(2024, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc), 2, 0, 1 },
                    { 6, new DateTime(2024, 4, 15, 12, 2, 0, 0, DateTimeKind.Utc), 3, new DateTime(2024, 4, 15, 12, 0, 0, 0, DateTimeKind.Utc), 4, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Artifacts",
                columns: new[] { "Id", "BlobUrl", "CreatedAt", "FileName", "RunLogId", "SizeBytes" },
                values: new object[,]
                {
                    { 10, "https://s3.example.com/artifacts/api.zip", new DateTime(2024, 5, 1, 10, 8, 0, 0, DateTimeKind.Utc), "api.zip", 1, 512000L },
                    { 11, "https://s3.example.com/artifacts/dist.zip", new DateTime(2024, 5, 3, 14, 5, 0, 0, DateTimeKind.Utc), "dist.zip", 3, 204800L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artifacts_RunLogId",
                table: "Artifacts",
                column: "RunLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Pipelines_ProjectId",
                table: "Pipelines",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PipelineSteps_LogArtifactId",
                table: "PipelineSteps",
                column: "LogArtifactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PipelineSteps_PipelineId",
                table: "PipelineSteps",
                column: "PipelineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OwnerId",
                table: "Projects",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RunLogs_PipelineId",
                table: "RunLogs",
                column: "PipelineId");

            migrationBuilder.CreateIndex(
                name: "IX_RunLogs_TriggeredByUserId",
                table: "RunLogs",
                column: "TriggeredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StepRuns_LogArtifactId",
                table: "StepRuns",
                column: "LogArtifactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StepRuns_RunLogId",
                table: "StepRuns",
                column: "RunLogId");

            migrationBuilder.CreateIndex(
                name: "IX_StepRuns_StepId",
                table: "StepRuns",
                column: "StepId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectMembers");

            migrationBuilder.DropTable(
                name: "StepRuns");

            migrationBuilder.DropTable(
                name: "PipelineSteps");

            migrationBuilder.DropTable(
                name: "Artifacts");

            migrationBuilder.DropTable(
                name: "RunLogs");

            migrationBuilder.DropTable(
                name: "Pipelines");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
