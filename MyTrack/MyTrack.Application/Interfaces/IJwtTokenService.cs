using MyTrack.Domain.Entities;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines operations for generating JWT tokens.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>The generated JWT token.</returns>
    string GenerateToken(User user);
}