using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines authentication operations for MyTrack users.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user.
    /// </summary>
    Task<LoginResponse> RegisterAsync(RegisterUserRequest request);

    /// <summary>
    /// Logs in an existing user.
    /// </summary>
    Task<LoginResponse> LoginAsync(LoginRequest request);
}