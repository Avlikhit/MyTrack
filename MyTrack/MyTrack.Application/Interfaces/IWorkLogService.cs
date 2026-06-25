using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines application operations for managing work logs.
/// </summary>
public interface IWorkLogService
{
    /// <summary>
    /// Creates a new work log.
    /// </summary>
    /// <param name="request">The work log creation request.</param>
    /// <returns>The created work log response.</returns>
    Task<WorkLogResponse> CreateAsync(CreateWorkLogRequest request);

    /// <summary>
    /// Gets a work log by its unique identifier.
    /// </summary>
    /// <param name="id">The work log identifier.</param>
    /// <returns>The matching work log response, if found.</returns>
    Task<WorkLogResponse?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all work logs for a specific date.
    /// </summary>
    /// <param name="workDate">The work date.</param>
    /// <returns>A collection of work log responses.</returns>
    Task<IEnumerable<WorkLogResponse>> GetByDateAsync(DateOnly workDate);

    /// <summary>
    /// Gets all work logs within a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <returns>A collection of work log responses.</returns>
    Task<IEnumerable<WorkLogResponse>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}