using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyTrack.Api.Controllers;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using Xunit;

namespace MyTrack.Tests.Controllers;

public class WorkLogsControllerTests
{
    [Fact]
    public async Task CreateAsync_ReturnsCreated()
    {
        var mockService = new Mock<IWorkLogService>();
        var mockLogger = new Mock<ILogger<WorkLogsController>>();
        var request = new CreateWorkLogRequest { ProjectId = 1, HoursWorked = 1 };
        var response = new WorkLogResponse { Id = 1, ProjectId = 1, HoursWorked = 1 };
        mockService.Setup(s => s.CreateAsync(It.IsAny<CreateWorkLogRequest>())).ReturnsAsync(response);

        var controller = new WorkLogsController(mockService.Object, mockLogger.Object);

        var result = await controller.CreateAsync(request);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(response, createdResult.Value);
    }

    [Fact]
    public async Task CreateAsync_ServiceThrowsArgumentException_ReturnsBadRequest()
    {
        var mockService = new Mock<IWorkLogService>();
        var mockLogger = new Mock<ILogger<WorkLogsController>>();
        var request = new CreateWorkLogRequest { ProjectId = 0, HoursWorked = 0 };
        mockService.Setup(s => s.CreateAsync(It.IsAny<CreateWorkLogRequest>())).ThrowsAsync(new ArgumentException("Invalid"));

        var controller = new WorkLogsController(mockService.Object, mockLogger.Object);

        var result = await controller.CreateAsync(request);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
