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
        var request = new CreateWorkLogRequest { ProjectId = 1, HoursWorked = 2 };
        var savedWorkLog = new WorkLog { Id = 1, ProjectId = 1, HoursWorked = 2 };
        mockRepo.Setup(r => r.AddAsync(It.IsAny<WorkLog>())).ReturnsAsync(savedWorkLog);

        var service = new WorkLogService(mockRepo.Object);

        var response = await service.CreateAsync(request);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<WorkLog>()), Times.Once);
        Assert.Equal(savedWorkLog.Id, response.Id);
        Assert.Equal(savedWorkLog.ProjectId, response.ProjectId);
    }
}
