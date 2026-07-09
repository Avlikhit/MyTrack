namespace MyTrack.Contracts.Requests;

/// <summary>
/// Represents the request data required to register a new user.
/// </summary>
public class RegisterUserRequest
{
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

    /// <summary>
    /// Gets or sets the user's password.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}