using Moq;
using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;
using MyTrack.Contracts.Requests;
using MyTrack.Domain.Entities;
using Xunit;

namespace MyTrack.Tests.Integration.Services;

public class WorkLogServiceIntegrationTests
{
    private Mock<IWorkLogRepository> _mockWorkLogRepository;
    private Mock<ICurrentUserService> _mockCurrentUserService;
    private Mock<IProjectRepository> _mockProjectRepository;
    private WorkLogService _workLogService;

    public WorkLogServiceIntegrationTests()
    {
        _mockWorkLogRepository = new Mock<IWorkLogRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockProjectRepository = new Mock<IProjectRepository>();
        _workLogService = new WorkLogService(
            _mockWorkLogRepository.Object,
            _mockCurrentUserService.Object,
            _mockProjectRepository.Object);

        _mockCurrentUserService.SetupGet(c => c.UserId).Returns(1);
        _mockCurrentUserService.SetupGet(c => c.Email).Returns("test@test.com");
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsWorkLogResponse()
    {
        // Arrange
        var projectId = 1;
        var request = new CreateWorkLogRequest
        {
            ProjectId = projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development",
            Description = "Developed new feature",
            TicketNumber = "TICKET-001"
        };

        var project = new Project { Id = projectId, Name = "Test Project" };

        var savedWorkLog = new WorkLog
        {
            Id = 1,
            UserId = _mockCurrentUserService.Object.UserId,
            ProjectId = request.ProjectId,
            WorkDate = request.WorkDate,
            HoursWorked = request.HoursWorked,
            TaskType = request.TaskType,
            Description = request.Description,
            TicketNumber = request.TicketNumber,
            CreatedDateTime = DateTime.UtcNow
        };

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(project);

        _mockWorkLogRepository.Setup(r => r.AddAsync(It.IsAny<WorkLog>()))
            .ReturnsAsync(savedWorkLog);

        // Act
        var result = await _workLogService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(savedWorkLog.Id, result.Id);
        Assert.Equal(savedWorkLog.ProjectId, result.ProjectId);
        Assert.Equal(savedWorkLog.HoursWorked, result.HoursWorked);
        Assert.Equal(savedWorkLog.TaskType, result.TaskType);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId), Times.Once);
        _mockWorkLogRepository.Verify(r => r.AddAsync(It.IsAny<WorkLog>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _workLogService.CreateAsync(null)
        );
    }

    [Fact]
    public async Task CreateAsync_WithInvalidProject_ThrowsArgumentException()
    {
        // Arrange
        var request = new CreateWorkLogRequest
        {
            ProjectId = 99999,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8
        };

        _mockProjectRepository.Setup(r => r.GetByIdAsync(request.ProjectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync((Project)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _workLogService.CreateAsync(request)
        );

        _mockWorkLogRepository.Verify(r => r.AddAsync(It.IsAny<WorkLog>()), Times.Never);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsWorkLogResponse()
    {
        // Arrange
        var workLogId = 1;
        var workLog = new WorkLog
        {
            Id = workLogId,
            UserId = _mockCurrentUserService.Object.UserId,
            ProjectId = 1,
            HoursWorked = 8,
            TaskType = "Development",
            CreatedDateTime = DateTime.UtcNow
        };

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(workLog);

        // Act
        var result = await _workLogService.GetByIdAsync(workLogId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(workLog.Id, result.Id);
        Assert.Equal(workLog.HoursWorked, result.HoursWorked);

        _mockWorkLogRepository.Verify(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var workLogId = 99999;

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync((WorkLog)null);

        // Act
        var result = await _workLogService.GetByIdAsync(workLogId);

        // Assert
        Assert.Null(result);

        _mockWorkLogRepository.Verify(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task GetByDateAsync_ReturnsWorkLogList()
    {
        // Arrange
        var workDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var workLogs = new[]
        {
            new WorkLog { Id = 1, WorkDate = workDate, UserId = _mockCurrentUserService.Object.UserId, HoursWorked = 4 },
            new WorkLog { Id = 2, WorkDate = workDate, UserId = _mockCurrentUserService.Object.UserId, HoursWorked = 4 }
        };

        _mockWorkLogRepository.Setup(r => r.GetByDateAsync(workDate, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(workLogs);

        // Act
        var result = await _workLogService.GetByDateAsync(workDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockWorkLogRepository.Verify(r => r.GetByDateAsync(workDate, _mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task GetByDateRangeAsync_ReturnsWorkLogList()
    {
        // Arrange
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var workLogs = new[]
        {
            new WorkLog { Id = 1, WorkDate = startDate, UserId = _mockCurrentUserService.Object.UserId, HoursWorked = 8 },
            new WorkLog { Id = 2, WorkDate = startDate.AddDays(1), UserId = _mockCurrentUserService.Object.UserId, HoursWorked = 8 }
        };

        _mockWorkLogRepository.Setup(r => r.GetByDateRangeAsync(startDate, endDate, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(workLogs);

        // Act
        var result = await _workLogService.GetByDateRangeAsync(startDate, endDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockWorkLogRepository.Verify(r => r.GetByDateRangeAsync(startDate, endDate, _mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsUpdatedWorkLogResponse()
    {
        // Arrange
        var workLogId = 1;
        var projectId = 1;
        var existingWorkLog = new WorkLog
        {
            Id = workLogId,
            UserId = _mockCurrentUserService.Object.UserId,
            ProjectId = projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development"
        };

        var updateRequest = new UpdateWorkLogRequest
        {
            ProjectId = projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 10,
            TaskType = "Testing"
        };

        var project = new Project { Id = projectId, Name = "Test Project" };

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(existingWorkLog);

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(project);

        var updatedWorkLog = new WorkLog
        {
            Id = workLogId,
            UserId = existingWorkLog.UserId,
            ProjectId = updateRequest.ProjectId,
            WorkDate = updateRequest.WorkDate,
            HoursWorked = updateRequest.HoursWorked,
            TaskType = updateRequest.TaskType,
            CreatedDateTime = existingWorkLog.CreatedDateTime,
            ModifiedDateTime = DateTime.UtcNow
        };

        _mockWorkLogRepository.Setup(r => r.UpdateAsync(It.IsAny<WorkLog>()))
            .ReturnsAsync(updatedWorkLog);

        // Act
        var result = await _workLogService.UpdateAsync(workLogId, updateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(workLogId, result.Id);
        Assert.Equal(updateRequest.HoursWorked, result.HoursWorked);
        Assert.Equal(updateRequest.TaskType, result.TaskType);

        _mockWorkLogRepository.Verify(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId), Times.Once);
        _mockWorkLogRepository.Verify(r => r.UpdateAsync(It.IsAny<WorkLog>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var workLogId = 99999;
        var updateRequest = new UpdateWorkLogRequest { ProjectId = 1, HoursWorked = 8 };

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync((WorkLog)null);

        // Act
        var result = await _workLogService.UpdateAsync(workLogId, updateRequest);

        // Assert
        Assert.Null(result);

        _mockWorkLogRepository.Verify(r => r.UpdateAsync(It.IsAny<WorkLog>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _workLogService.UpdateAsync(1, null)
        );
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidWorkLogId_ThrowsArgumentException()
    {
        // Arrange
        var updateRequest = new UpdateWorkLogRequest { ProjectId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _workLogService.UpdateAsync(-1, updateRequest)
        );
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidProject_ThrowsArgumentException()
    {
        // Arrange
        var workLogId = 1;
        var invalidProjectId = 99999;
        var existingWorkLog = new WorkLog
        {
            Id = workLogId,
            UserId = _mockCurrentUserService.Object.UserId,
            ProjectId = 1
        };

        var updateRequest = new UpdateWorkLogRequest
        {
            ProjectId = invalidProjectId,
            HoursWorked = 8
        };

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(existingWorkLog);

        _mockProjectRepository.Setup(r => r.GetByIdAsync(invalidProjectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync((Project)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _workLogService.UpdateAsync(workLogId, updateRequest)
        );

        _mockWorkLogRepository.Verify(r => r.UpdateAsync(It.IsAny<WorkLog>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var workLogId = 1;
        var workLog = new WorkLog { Id = workLogId, UserId = _mockCurrentUserService.Object.UserId };

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(workLog);

        _mockWorkLogRepository.Setup(r => r.DeleteAsync(workLog))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _workLogService.DeleteAsync(workLogId);

        // Assert
        Assert.True(result);

        _mockWorkLogRepository.Verify(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId), Times.Once);
        _mockWorkLogRepository.Verify(r => r.DeleteAsync(workLog), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var workLogId = 99999;

        _mockWorkLogRepository.Setup(r => r.GetByIdAsync(workLogId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync((WorkLog)null);

        // Act
        var result = await _workLogService.DeleteAsync(workLogId);

        // Assert
        Assert.False(result);

        _mockWorkLogRepository.Verify(r => r.DeleteAsync(It.IsAny<WorkLog>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidWorkLogId_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _workLogService.DeleteAsync(-1)
        );
    }
}
