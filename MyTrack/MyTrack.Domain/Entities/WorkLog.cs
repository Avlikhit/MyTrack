namespace MyTrack.Domain.Entities;

/// <summary>
/// Represents a single work activity recorded by the user.
///
/// A WorkLog captures the work completed for a specific date, including
/// project information, task details, hours worked, learnings, blockers,
/// and the next planned steps.
///
/// This entity is the primary business object used throughout the MyTrack
/// application and serves as the foundation for reporting, productivity
/// tracking, and payroll calculations.
/// </summary>
public class WorkLog
{
    /// <summary>
    /// Gets or sets the unique identifier for the work log.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date on which the work was performed.
    /// </summary>
    public DateOnly WorkDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the project associated with this work log.
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;// TODO: will decide at time of creating data objects

    /// <summary>
    /// Gets or sets the ticket, task, or issue number associated with this work.
    /// This value is optional.
    /// </summary>
    public string? TicketNumber { get; set; }

    /// <summary>
    /// Gets or sets the category of work performed,
    /// such as Development, Testing, Meeting, Research, or Support.
    /// </summary>
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a detailed description of the work completed.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total number of hours spent on this work activity.
    /// </summary>
    public decimal HoursWorked { get; set; }

    /// <summary>
    /// Gets or sets any blockers or issues encountered while performing the work.
    /// This value is optional.
    /// </summary>
    public string? Blockers { get; set; }

    /// <summary>
    /// Gets or sets the knowledge or experience gained from completing the work.
    /// This value is optional.
    /// </summary>
    public string? Learnings { get; set; }

    /// <summary>
    /// Gets or sets the planned next steps related to this work activity.
    /// This value is optional.
    /// </summary>
    public string? NextSteps { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the work log was created.
    /// </summary>
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the work log was last modified.
    /// This value is optional until the record is updated.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with this work log.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project associated with this work log.
    /// </summary>
    public Project? Project { get; set; }
}