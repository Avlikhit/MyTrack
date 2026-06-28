namespace MyTrack.Contracts.Requests;

/// <summary>
/// Represents the request data required to update an existing project.
/// </summary>
public class UpdateProjectRequest
{
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
}