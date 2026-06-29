namespace MyTrack.Application.Interfaces;

/// <summary>
/// Provides information about the currently authenticated user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the authenticated user's unique identifier.
    /// </summary>
    int UserId { get; }

    /// <summary>
    /// Gets the authenticated user's email address.
    /// </summary>
    string Email { get; }
}