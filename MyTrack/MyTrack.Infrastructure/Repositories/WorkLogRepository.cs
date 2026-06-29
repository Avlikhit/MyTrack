using Microsoft.EntityFrameworkCore;
using MyTrack.Application.Interfaces;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Data;

namespace MyTrack.Infrastructure.Repositories;

/// <summary>
/// Provides data access operations for work logs.
/// </summary>
public class WorkLogRepository : IWorkLogRepository
{
    private readonly MyTrackDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkLogRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public WorkLogRepository(MyTrackDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<WorkLog> AddAsync(WorkLog workLog)
    {
        ArgumentNullException.ThrowIfNull(workLog);

        await _context.WorkLogs.AddAsync(workLog);
        await _context.SaveChangesAsync();

        return workLog;
    }

    /// <inheritdoc/>
    public async Task<WorkLog> UpdateAsync(WorkLog workLog)
    {
        _context.WorkLogs.Update(workLog);

        await _context.SaveChangesAsync();

        return workLog;
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(WorkLog workLog)
    {
        _context.WorkLogs.Remove(workLog);

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<WorkLog?> GetByIdAsync(int id)
    {
        return await _context.WorkLogs
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<WorkLog?> GetByIdAsync(int id, int userId)
    {
        return await _context.WorkLogs
            .Include(w => w.Project)
            .FirstOrDefaultAsync(w =>
                w.Id == id &&
                w.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<WorkLog>> GetByDateRangeAsync(
        DateOnly startDate,
        DateOnly endDate,
        int userId)
    {
        return await _context.WorkLogs
            .Include(w => w.Project)
            .Where(w =>
                w.WorkDate >= startDate &&
                w.WorkDate <= endDate &&
                w.UserId == userId)
            .OrderByDescending(w => w.WorkDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<WorkLog>> GetByDateAsync(
        DateOnly workDate,
        int userId)
    {
        return await _context.WorkLogs
            .Include(w => w.Project)
            .Where(w =>
                w.WorkDate == workDate &&
                w.UserId == userId)
            .OrderByDescending(w => w.WorkDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<WorkLog>> GetByDateAsync(DateOnly workDate)
    {
        return await _context.WorkLogs
            .Where(x => x.WorkDate == workDate)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<WorkLog>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        return await _context.WorkLogs
            .Where(x => x.WorkDate >= startDate &&
                        x.WorkDate <= endDate)
            .ToListAsync();
    }
}
