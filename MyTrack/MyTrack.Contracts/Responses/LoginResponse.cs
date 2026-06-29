namespace MyTrack.Contracts.Responses;

/// <summary>
/// Represents the response returned after a successful login.
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the user's full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JWT access token.
    /// </summary>
    public string Token { get; set; } = string.Empty;
}