namespace MyTrack.Contracts.Responses;

/// <summary>
/// Represents the response data returned for a work log.
/// </summary>
public class WorkLogResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the work log.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the date on which the work was performed.
    /// </summary>
    public DateOnly WorkDate { get; set; }

    /// <summary>
    /// Gets or sets the project identifier associated with the work log.
    /// </summary>
    public int ProjectId { get; set; }

    /// <summary>
    /// Gets or sets the project name associated with the work log.
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional ticket, task, or issue number.
    /// </summary>
    public string? TicketNumber { get; set; }

    /// <summary>
    /// Gets or sets the type of work performed.
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

    /// <summary>
    /// Gets or sets the date and time when the work log was created.
    /// </summary>
    public DateTime CreatedDateTime { get; set; }
}