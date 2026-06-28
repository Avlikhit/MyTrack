using MyTrack.Domain.Entities;

namespace MyTrack.Infrastructure.Data;

/// <summary>
/// Seeds default development data for the MyTrack database.
/// </summary>
public static class MyTrackDbSeeder
{
    /// <summary>
    /// Seeds initial development data if it does not already exist.
    /// </summary>
    /// <param name="context">The database context.</param>
    public static async Task SeedAsync(MyTrackDbContext context)
    {
        var personalProject = context.Projects
            .FirstOrDefault(x => x.Name == "Personal Portfolio");

        if (personalProject is null)
        {
            personalProject = new Project
            {
                Name = "Personal Portfolio",
                Description = "Sample personal website project.",
                ColorCode = "#2563EB",
                DisplayOrder = 1,
                IsDefault = true,
                IsActive = true,
                CreatedDateTime = DateTime.UtcNow
            };

            await context.Projects.AddAsync(personalProject);
            await context.SaveChangesAsync();
        }

        var learningProject = context.Projects
            .FirstOrDefault(x => x.Name == "Learning Project");

        if (learningProject is null)
        {
            learningProject = new Project
            {
                Name = "Learning Project",
                Description = "Sample project used for learning and testing.",
                ColorCode = "#10B981",
                DisplayOrder = 2,
                IsDefault = false,
                IsActive = true,
                CreatedDateTime = DateTime.UtcNow
            };

            await context.Projects.AddAsync(learningProject);
            await context.SaveChangesAsync();
        }

        if (!context.WorkLogs.Any())
        {
            var workLogs = new List<WorkLog>
            {
                new()
                {
                    WorkDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
                    ProjectId = personalProject.Id,
                    TicketNumber = "SAMPLE-001",
                    TaskType = "Planning",
                    Description = "Created initial project plan and outlined core features.",
                    HoursWorked = 2,
                    Blockers = "None",
                    Learnings = "Learned how to define project requirements.",
                    NextSteps = "Create initial backend structure.",
                    CreatedDateTime = DateTime.UtcNow
                },
                new()
                {
                    WorkDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-3)),
                    ProjectId = learningProject.Id,
                    TicketNumber = "SAMPLE-002",
                    TaskType = "Development",
                    Description = "Created backend API structure and configured Swagger.",
                    HoursWorked = 2.5m,
                    Blockers = "None",
                    Learnings = "Learned how API endpoints are organized.",
                    NextSteps = "Add database support.",
                    CreatedDateTime = DateTime.UtcNow
                },
                new()
                {
                    WorkDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                    ProjectId = learningProject.Id,
                    TicketNumber = "SAMPLE-003",
                    TaskType = "Testing",
                    Description = "Tested API endpoints using sample data.",
                    HoursWorked = 1.5m,
                    Blockers = "None",
                    Learnings = "Learned how to verify API responses.",
                    NextSteps = "Improve validation and logging.",
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            await context.WorkLogs.AddRangeAsync(workLogs);
            await context.SaveChangesAsync();
        }
    }
}