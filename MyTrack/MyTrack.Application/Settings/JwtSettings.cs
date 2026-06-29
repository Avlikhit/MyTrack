namespace MyTrack.Application.Settings;

/// <summary>
/// Represents JWT authentication configuration settings.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets the token issuer.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token audience.
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the secret key used to sign JWT tokens.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the token expiration time in minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; }
}