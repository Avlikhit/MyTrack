using Microsoft.AspNetCore.Identity;
using MyTrack.Domain.Entities;

namespace MyTrack.Infrastructure.Data;

/// <summary>
/// Seeds default development data for the MyTrack database.
/// </summary>
public static class MyTrackDbSeeder
{
    public static async Task SeedAsync(MyTrackDbContext context)
    {
        var user = context.Users.FirstOrDefault(x => x.Email == "testuser@example.com");

        if (user is null)
        {
            user = new User
            {
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@example.com",
                IsActive = true,
                CreatedDateTime = DateTime.UtcNow
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Password123!");

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        var personalProject = context.Projects
            .FirstOrDefault(x => x.Name == "Personal Portfolio" && x.UserId == user.Id);

        if (personalProject is null)
        {
            personalProject = new Project
            {
                UserId = user.Id,
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
            .FirstOrDefault(x => x.Name == "Learning Project" && x.UserId == user.Id);

        if (learningProject is null)
        {
            learningProject = new Project
            {
                UserId = user.Id,
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

        if (!context.WorkLogs.Any(x => x.UserId == user.Id))
        {
            var workLogs = new List<WorkLog>
            {
                new()
                {
                    UserId = user.Id,
                    ProjectId = personalProject.Id,
                    WorkDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                    TicketNumber = "SAMPLE-001",
                    TaskType = "Planning",
                    Description = "Created sample project plan.",
                    HoursWorked = 2,
                    Blockers = "None",
                    Learnings = "Learned project planning.",
                    NextSteps = "Continue backend development.",
                    CreatedDateTime = DateTime.UtcNow
                },
                new()
                {
                    UserId = user.Id,
                    ProjectId = learningProject.Id,
                    WorkDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                    TicketNumber = "SAMPLE-002",
                    TaskType = "Development",
                    Description = "Tested API endpoints using seeded data.",
                    HoursWorked = 2.5m,
                    Blockers = "None",
                    Learnings = "Learned seeded test data.",
                    NextSteps = "Test user-specific filtering.",
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            await context.WorkLogs.AddRangeAsync(workLogs);
            await context.SaveChangesAsync();
        }
    }
}