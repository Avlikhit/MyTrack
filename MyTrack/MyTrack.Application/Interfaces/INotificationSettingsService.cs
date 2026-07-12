using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines application operations for user notification settings.
/// </summary>
public interface INotificationSettingsService
{
    /// <summary>
    /// Gets notification settings for the logged-in user.
    /// </summary>
    /// <param name="userId">The logged-in user identifier.</param>
    /// <returns>The user's notification settings.</returns>
    Task<NotificationSettingsResponse> GetAsync(int userId);

    /// <summary>
    /// Updates notification settings for the logged-in user.
    /// </summary>
    /// <param name="userId">The logged-in user identifier.</param>
    /// <param name="request">The notification settings update request.</param>
    /// <returns>The updated notification settings.</returns>
    Task<NotificationSettingsResponse> UpdateAsync(
        int userId,
        UpdateNotificationSettingsRequest request);
}