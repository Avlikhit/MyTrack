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

    /// <summary>
    /// Gets the logged-in user's profile.
    /// </summary>
    /// <param name="userId">The logged-in user id.</param>
    /// <returns>The user profile.</returns>
    Task<UserProfileResponse> GetProfileAsync(int userId);

    /// <summary>
    /// Updates the logged-in user's profile.
    /// </summary>
    /// <param name="userId">The logged-in user id.</param>
    /// <param name="request">The profile update request.</param>
    /// <returns>The updated user profile.</returns>
    Task<UserProfileResponse> UpdateProfileAsync(int userId, UpdateUserProfileRequest request);

    /// <summary>
    /// Changes the logged-in user's password.
    /// </summary>
    /// <param name="userId">The logged-in user id.</param>
    /// <param name="request">The change password request.</param>
    Task ChangePasswordAsync(int userId, ChangePasswordRequest request);

}