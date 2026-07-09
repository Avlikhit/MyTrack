using Microsoft.EntityFrameworkCore;
using MyTrack.API.Models;
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

    /// <summary>
    /// Gets or sets the users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the PayInformations table.
    /// </summary>
    public DbSet<PayInformation> PayInformations { get; set; }

    /// <summary>
    /// Gets or sets the payroll settings table.
    /// </summary>
    public DbSet<PayrollSettings> PayrollSettings { get; set; }

    /// <summary>
    /// Configures entity relationships.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.User)
            .WithMany(u => u.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkLog>()
            .HasOne(w => w.User)
            .WithMany(u => u.WorkLogs)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkLog>()
            .HasOne(w => w.Project)
            .WithMany(p => p.WorkLogs)
            .HasForeignKey(w => w.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkLog>()
            .Property(w => w.HoursWorked)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PayInformation>()
    .Property(p => p.GrossPay)
    .HasPrecision(10, 2);

        modelBuilder.Entity<PayInformation>()
            .Property(p => p.FederalTax)
            .HasPrecision(10, 2);

        modelBuilder.Entity<PayInformation>()
            .Property(p => p.StateTax)
            .HasPrecision(10, 2);

        modelBuilder.Entity<PayInformation>()
            .Property(p => p.SocialSecurityTax)
            .HasPrecision(10, 2);

        modelBuilder.Entity<PayInformation>()
            .Property(p => p.MedicareTax)
            .HasPrecision(10, 2);

        modelBuilder.Entity<PayInformation>()
            .Property(p => p.OtherDeductions)
            .HasPrecision(10, 2);

        modelBuilder.Entity<PayInformation>()
            .Property(p => p.NetPay)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Project>()
            .Property(p => p.HourlyRate)
            .HasPrecision(10, 2);

        modelBuilder.Entity<PayrollSettings>()
            .Property(p => p.FederalTaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PayrollSettings>()
            .Property(p => p.StateTaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PayrollSettings>()
            .Property(p => p.SocialSecurityTaxPercent)
            .HasPrecision(5, 2);

        modelBuilder.Entity<PayrollSettings>()
            .Property(p => p.MedicareTaxPercent)
            .HasPrecision(5, 2);

    }
}