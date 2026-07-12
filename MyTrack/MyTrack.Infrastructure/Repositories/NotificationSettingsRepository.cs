using Microsoft.EntityFrameworkCore;
using MyTrack.Application.Interfaces;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Data;

namespace MyTrack.Infrastructure.Repositories;

/// <summary>
/// Provides data access operations for notification settings.
/// </summary>
public class NotificationSettingsRepository : INotificationSettingsRepository
{
    private readonly MyTrackDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationSettingsRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public NotificationSettingsRepository(MyTrackDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<NotificationSettings?> GetByUserIdAsync(int userId)
    {
        return await _context.NotificationSettings
            .FirstOrDefaultAsync(settings => settings.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task<NotificationSettings> AddAsync(
        NotificationSettings notificationSettings)
    {
        _context.NotificationSettings.Add(notificationSettings);
        await _context.SaveChangesAsync();

        return notificationSettings;
    }

    /// <inheritdoc/>
    public async Task<NotificationSettings> UpdateAsync(
        NotificationSettings notificationSettings)
    {
        _context.NotificationSettings.Update(notificationSettings);
        await _context.SaveChangesAsync();

        return notificationSettings;
    }
}