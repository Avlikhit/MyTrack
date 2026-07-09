namespace MyTrack.Contracts.Responses;

/// <summary>
/// Represents profile information for the logged-in user.
/// </summary>
public class UserProfileResponse
{
    /// <summary>
    /// Gets or sets the user id.
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
    /// Gets or sets the user's role.
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's contact number.
    /// </summary>
    public string ContactNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's home address.
    /// </summary>
    public string HomeAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's work address.
    /// </summary>
    public string WorkAddress { get; set; } = string.Empty;
}