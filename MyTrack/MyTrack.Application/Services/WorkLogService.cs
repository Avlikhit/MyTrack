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
    private readonly IWorkLogRepository _workLogRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkLogService"/> class.
    /// </summary>
    /// <param name="workLogRepository">The work log repository.</param>
    public WorkLogService(IWorkLogRepository workLogRepository)
    {
        _workLogRepository = workLogRepository;
    }

    /// <inheritdoc/>
    public async Task<WorkLogResponse> CreateAsync(CreateWorkLogRequest request)
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

        var savedWorkLog = await _workLogRepository.AddAsync(workLog);

        return MapToResponse(savedWorkLog);
    }

    /// <inheritdoc/>
    public async Task<WorkLogResponse?> GetByIdAsync(int id)
    {
        var workLog = await _workLogRepository.GetByIdAsync(id);

        return workLog is null ? null : MapToResponse(workLog);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<WorkLogResponse>> GetByDateAsync(DateOnly workDate)
    {
        var workLogs = await _workLogRepository.GetByDateAsync(workDate);

        return workLogs.Select(MapToResponse);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<WorkLogResponse>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate)
    {
        var workLogs = await _workLogRepository.GetByDateRangeAsync(startDate, endDate);

        return workLogs.Select(MapToResponse);
    }

    /// <summary>
    /// Maps a WorkLog domain entity to a WorkLogResponse contract.
    /// </summary>
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