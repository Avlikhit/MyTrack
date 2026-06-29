using MyTrack.Domain.Entities;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines data access operations for work logs.
/// </summary>
public interface IWorkLogRepository
{
    /// <summary>
    /// Adds a new work log.
    /// </summary>
    Task<WorkLog> AddAsync(WorkLog workLog);

    /// <summary>
    /// Updates an existing work log.
    /// </summary>
    Task<WorkLog> UpdateAsync(WorkLog workLog);

    /// <summary>
    /// Deletes an existing work log.
    /// </summary>
    Task DeleteAsync(WorkLog workLog);

    /// <summary>
    /// Gets a work log by id.
    /// </summary>
    Task<WorkLog?> GetByIdAsync(int id);

    /// <summary>
    /// Gets a work log by id for a specific user.
    /// </summary>
    Task<WorkLog?> GetByIdAsync(int id, int userId);

    /// <summary>
    /// Gets work logs for a specific date.
    /// </summary>
    Task<IEnumerable<WorkLog>> GetByDateAsync(DateOnly workDate);

    /// <summary>
    /// Gets work logs for a specific date and user.
    /// </summary>
    Task<IEnumerable<WorkLog>> GetByDateAsync(DateOnly workDate, int userId);

    /// <summary>
    /// Gets work logs between two dates.
    /// </summary>
    Task<IEnumerable<WorkLog>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);

    /// <summary>
    /// Gets work logs between two dates for a specific user.
    /// </summary>
    Task<IEnumerable<WorkLog>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, int userId);
}