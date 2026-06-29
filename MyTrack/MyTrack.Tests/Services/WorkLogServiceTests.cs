using System;
using System.Threading.Tasks;
using Moq;
using MyTrack.Application.Services;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Domain.Entities;
using Xunit;

namespace MyTrack.Tests.Services;

public class WorkLogServiceTests
{
    [Fact]
    public async Task CreateAsync_CallsRepositoryAndReturnsResponse()
    {
        var mockRepo = new Mock<IWorkLogRepository>();
        var mockProjectRepo = new Mock<IProjectRepository>();
        var mockCurrentUser = new Mock<ICurrentUserService>();

        mockCurrentUser.SetupGet(c => c.UserId).Returns(42);
        mockCurrentUser.SetupGet(c => c.Email).Returns("test@example.com");

        var request = new CreateWorkLogRequest { ProjectId = 1, HoursWorked = 2 };
        var savedWorkLog = new WorkLog { Id = 1, ProjectId = 1, HoursWorked = 2 };
        var project = new Project { Id = 1, Name = "Test" };

        mockRepo.Setup(r => r.AddAsync(It.IsAny<WorkLog>())).ReturnsAsync(savedWorkLog);
        mockProjectRepo.Setup(r => r.GetByIdAsync(request.ProjectId, mockCurrentUser.Object.UserId)).ReturnsAsync(project);

        var service = new WorkLogService(mockRepo.Object, mockCurrentUser.Object, mockProjectRepo.Object);

        var response = await service.CreateAsync(request);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<WorkLog>()), Times.Once);
        Assert.Equal(savedWorkLog.Id, response.Id);
        Assert.Equal(savedWorkLog.ProjectId, response.ProjectId);
    }
}
