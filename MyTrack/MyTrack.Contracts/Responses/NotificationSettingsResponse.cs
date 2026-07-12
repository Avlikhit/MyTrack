namespace MyTrack.Contracts.Responses;

/// <summary>
/// Represents notification settings for the logged-in user.
/// </summary>
public class NotificationSettingsResponse
{
    /// <summary>
    /// Gets or sets the notification settings identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether notifications are enabled.
    /// </summary>
    public bool NotificationsEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether daily work log reminders are enabled.
    /// </summary>
    public bool DailyWorkLogReminderEnabled { get; set; }

    /// <summary>
    /// Gets or sets the daily reminder time.
    /// </summary>
    public TimeSpan DailyReminderTime { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether monthly summary reminders are enabled.
    /// </summary>
    public bool MonthlySummaryReminderEnabled { get; set; }

    /// <summary>
    /// Gets or sets how many days before the first day of the month the reminder should appear.
    /// </summary>
    public int MonthlyReminderDaysBefore { get; set; }
}