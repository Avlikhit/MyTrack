using Microsoft.EntityFrameworkCore;
using MyTrack.Domain.Entities;

namespace MyTrack.Infrastructure.Data;

/// <summary>
/// Represents the Entity Framework database context for the MyTrack application.
/// </summary>
public class MyTrackDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyTrackDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    public MyTrackDbContext(DbContextOptions<MyTrackDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the projects table.
    /// </summary>
    public DbSet<Project> Projects { get; set; }

    /// <summary>
    /// Gets or sets the work logs table.
    /// </summary>
    public DbSet<WorkLog> WorkLogs { get; set; }
}