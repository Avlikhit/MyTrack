using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using MyTrack.Domain.Entities;

namespace MyTrack.Application.Services;

/// <summary>
/// Provides application logic for user notification settings.
/// </summary>
public class NotificationSettingsService : INotificationSettingsService
{
    private readonly INotificationSettingsRepository _notificationSettingsRepository;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="NotificationSettingsService"/> class.
    /// </summary>
    /// <param name="notificationSettingsRepository">
    /// The notification settings repository.
    /// </param>
    public NotificationSettingsService(
        INotificationSettingsRepository notificationSettingsRepository)
    {
        _notificationSettingsRepository = notificationSettingsRepository;
    }

    /// <inheritdoc/>
    public async Task<NotificationSettingsResponse> GetAsync(int userId)
    {
        var settings =
            await _notificationSettingsRepository.GetByUserIdAsync(userId);

        if (settings is null)
        {
            settings = new NotificationSettings
            {
                UserId = userId,
                NotificationsEnabled = true,
                DailyWorkLogReminderEnabled = true,
                DailyReminderTime = new TimeSpan(18, 0, 0),
                MonthlySummaryReminderEnabled = true,
                MonthlyReminderDaysBefore = 1,
                CreatedDateTime = DateTime.UtcNow
            };

            settings =
                await _notificationSettingsRepository.AddAsync(settings);
        }

        return MapToResponse(settings);
    }

    /// <inheritdoc/>
    public async Task<NotificationSettingsResponse> UpdateAsync(
        int userId,
        UpdateNotificationSettingsRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.MonthlyReminderDaysBefore < 0 ||
            request.MonthlyReminderDaysBefore > 31)
        {
            throw new ArgumentException(
                "Monthly reminder days before must be between 0 and 31.");
        }

        var settings =
            await _notificationSettingsRepository.GetByUserIdAsync(userId);

        if (settings is null)
        {
            settings = new NotificationSettings
            {
                UserId = userId,
                CreatedDateTime = DateTime.UtcNow
            };

            settings =
                await _notificationSettingsRepository.AddAsync(settings);
        }

        settings.NotificationsEnabled = request.NotificationsEnabled;
        settings.DailyWorkLogReminderEnabled =
            request.DailyWorkLogReminderEnabled;
        settings.DailyReminderTime = request.DailyReminderTime;
        settings.MonthlySummaryReminderEnabled =
            request.MonthlySummaryReminderEnabled;
        settings.MonthlyReminderDaysBefore =
            request.MonthlyReminderDaysBefore;
        settings.ModifiedDateTime = DateTime.UtcNow;

        var updatedSettings =
            await _notificationSettingsRepository.UpdateAsync(settings);

        return MapToResponse(updatedSettings);
    }

    /// <summary>
    /// Maps notification settings to a response.
    /// </summary>
    /// <param name="settings">The notification settings entity.</param>
    /// <returns>The notification settings response.</returns>
    private static NotificationSettingsResponse MapToResponse(
        NotificationSettings settings)
    {
        return new NotificationSettingsResponse
        {
            Id = settings.Id,
            NotificationsEnabled = settings.NotificationsEnabled,
            DailyWorkLogReminderEnabled =
                settings.DailyWorkLogReminderEnabled,
            DailyReminderTime = settings.DailyReminderTime,
            MonthlySummaryReminderEnabled =
                settings.MonthlySummaryReminderEnabled,
            MonthlyReminderDaysBefore =
                settings.MonthlyReminderDaysBefore
        };
    }
}