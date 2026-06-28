namespace MyTrack.Contracts.Responses;

/// <summary>
/// Represents project data returned from the API.
/// </summary>
public class ProjectResponse
{
    /// <summary>
    /// Gets or sets the unique project identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets optional project details.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the color code used to display the project in the UI.
    /// </summary>
    public string? ColorCode { get; set; }

    /// <summary>
    /// Gets or sets the display order for showing projects.
    /// </summary>
    public int DisplayOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this project is the default project.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the project is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the project was created.
    /// </summary>
    public DateTime CreatedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the project was last modified.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }
}