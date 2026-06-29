namespace MyTrack.Domain.Entities;

/// <summary>
/// Represents a project or work area tracked in the MyTrack application.
///
/// A project is used to group related work logs together so the application
/// can generate reports by project, calculate time spent, and organize daily
/// work activities.
/// </summary>
public class Project
{
    /// <summary>
    /// Gets or sets the unique identifier for the project.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets optional details describing the purpose of the project.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the project is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the date and time when the project was created.
    /// </summary>
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the project was last modified.
    /// This value is optional until the project is updated.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the work logs associated with this project.
    /// </summary>
    public ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();

    /// <summary>
    /// Gets or sets the color code used to display the project in the UI.
    /// Example: #4F46E5.
    /// </summary>
    public string? ColorCode { get; set; }

    /// <summary>
    /// Gets or sets the display order for showing projects in dropdowns and lists.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this project is the default selected project.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets the user identifier that owns this project.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user that owns this project.
    /// </summary>
    public User? User { get; set; }
}