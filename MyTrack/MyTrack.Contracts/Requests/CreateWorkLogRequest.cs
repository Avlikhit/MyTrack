namespace MyTrack.Contracts.Requests;

/// <summary>
/// Represents the request data required to create a new work log.
/// </summary>
public class CreateWorkLogRequest
{
    /// <summary>
    /// Gets or sets the date on which the work was performed.
    /// </summary>
    public DateOnly WorkDate { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with the work log.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the optional ticket, task, or issue number.
    /// </summary>
    public string? TicketNumber { get; set; }

    /// <summary>
    /// Gets or sets the type of work performed.
    /// Examples: Development, Testing, Meeting, Support, Research.
    /// </summary>
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the work completed.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of hours spent on the work.
    /// </summary>
    public decimal HoursWorked { get; set; }

    /// <summary>
    /// Gets or sets any blockers faced during the work.
    /// </summary>
    public string? Blockers { get; set; }

    /// <summary>
    /// Gets or sets what was learned while completing the work.
    /// </summary>
    public string? Learnings { get; set; }

    /// <summary>
    /// Gets or sets the planned next steps.
    /// </summary>
    public string? NextSteps { get; set; }
}