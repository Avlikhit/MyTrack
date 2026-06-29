namespace MyTrack.Domain.Entities;

/// <summary>
/// Represents an application user who can log in and manage personal MyTrack data.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique user identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the hashed password.
    /// Plain text passwords must never be stored.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the user was last modified.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the projects owned by the user.
    /// </summary>
    public ICollection<Project> Projects { get; set; } = new List<Project>();

    /// <summary>
    /// Gets or sets the work logs owned by the user.
    /// </summary>
    public ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();
}