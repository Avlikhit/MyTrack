namespace MyTrack.Domain.Entities;

/// <summary>
/// Represents notification preferences for a user.
/// </summary>
public class NotificationSettings
{
    /// <summary>
    /// Gets or sets the notification settings identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether notifications are enabled.
    /// </summary>
    public bool NotificationsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether daily work log reminders are enabled.
    /// </summary>
    public bool DailyWorkLogReminderEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the daily reminder time.
    /// </summary>
    public TimeSpan DailyReminderTime { get; set; } = new(18, 0, 0);

    /// <summary>
    /// Gets or sets a value indicating whether monthly summary reminders are enabled.
    /// </summary>
    public bool MonthlySummaryReminderEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets how many days before the first day of the month the reminder should appear.
    /// </summary>
    public int MonthlyReminderDaysBefore { get; set; } = 1;

    /// <summary>
    /// Gets or sets the date and time when the settings were created.
    /// </summary>
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the settings were last modified.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }

    /// <summary>
    /// Gets or sets the related user.
    /// </summary>
    public User User { get; set; } = null!;
}