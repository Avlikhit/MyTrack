using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using MyTrack.Domain.Entities;

namespace MyTrack.Application.Services;

/// <summary>
/// Provides application logic for managing work logs.
/// </summary>
public class WorkLogService : IWorkLogService
{
    /// <summary>
    /// Creates a new work log.
    /// </summary>
    /// <param name="request">The work log creation request.</param>
    /// <returns>The created work log response.</returns>
    public Task<WorkLogResponse> CreateAsync(CreateWorkLogRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.ProjectId <= 0)
        {
            throw new ArgumentException("ProjectId is required.", nameof(request));
        }

        if (request.HoursWorked <= 0)
        {
            throw new ArgumentException("HoursWorked must be greater than zero.", nameof(request));
        }

        var workLog = new WorkLog
        {
            Id = 1,
            WorkDate = request.WorkDate,
            ProjectId = request.ProjectId,
            TicketNumber = request.TicketNumber,
            TaskType = request.TaskType,
            Description = request.Description,
            HoursWorked = request.HoursWorked,
            Blockers = request.Blockers,
            Learnings = request.Learnings,
            NextSteps = request.NextSteps,
            CreatedDateTime = DateTime.UtcNow
        };

        var response = MapToResponse(workLog);

        return Task.FromResult(response);
    }

    /// <summary>
    /// Gets a work log by its unique identifier.
    /// </summary>
    /// <param name="id">The work log identifier.</param>
    /// <returns>The matching work log response, if found.</returns>
    public Task<WorkLogResponse?> GetByIdAsync(int id)
    {
        return Task.FromResult<WorkLogResponse?>(null);
    }

    /// <summary>
    /// Gets all work logs for a specific date.
    /// </summary>
    /// <param name="workDate">The work date.</param>
    /// <returns>A collection of work log responses.</returns>
    public Task<IEnumerable<WorkLogResponse>> GetByDateAsync(DateOnly workDate)
    {
        return Task.FromResult(Enumerable.Empty<WorkLogResponse>());
    }

    /// <summary>
    /// Gets all work logs within a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <returns>A collection of work log responses.</returns>
    public Task<IEnumerable<WorkLogResponse>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        return Task.FromResult(Enumerable.Empty<WorkLogResponse>());
    }

    /// <summary>
    /// Maps a WorkLog domain entity to a WorkLogResponse contract.
    /// </summary>
    /// <param name="workLog">The work log entity.</param>
    /// <returns>The work log response.</returns>
    private static WorkLogResponse MapToResponse(WorkLog workLog)
    {
        return new WorkLogResponse
        {
            Id = workLog.Id,
            WorkDate = workLog.WorkDate,
            ProjectId = workLog.ProjectId,
            ProjectName = workLog.Project?.Name ?? workLog.ProjectName,
            TicketNumber = workLog.TicketNumber,
            TaskType = workLog.TaskType,
            Description = workLog.Description,
            HoursWorked = workLog.HoursWorked,
            Blockers = workLog.Blockers,
            Learnings = workLog.Learnings,
            NextSteps = workLog.NextSteps,
            CreatedDateTime = workLog.CreatedDateTime
        };
    }
}