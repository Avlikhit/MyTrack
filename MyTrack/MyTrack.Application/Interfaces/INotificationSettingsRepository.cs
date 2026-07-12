using MyTrack.Domain.Entities;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines repository operations for notification settings.
/// </summary>
public interface INotificationSettingsRepository
{
    /// <summary>
    /// Gets notification settings for a user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>The user's notification settings, or null when none exist.</returns>
    Task<NotificationSettings?> GetByUserIdAsync(int userId);

    /// <summary>
    /// Adds notification settings.
    /// </summary>
    /// <param name="notificationSettings">The notification settings to add.</param>
    /// <returns>The created notification settings.</returns>
    Task<NotificationSettings> AddAsync(NotificationSettings notificationSettings);

    /// <summary>
    /// Updates notification settings.
    /// </summary>
    /// <param name="notificationSettings">The notification settings to update.</param>
    /// <returns>The updated notification settings.</returns>
    Task<NotificationSettings> UpdateAsync(NotificationSettings notificationSettings);
}