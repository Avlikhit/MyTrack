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

public class ProjectsControllerTests
{
    [Fact]
    public async Task CreateAsync_ReturnsCreated()
    {
        var mockService = new Mock<IProjectService>();
        var mockLogger = new Mock<ILogger<ProjectsController>>();
        var request = new CreateProjectRequest { Name = "Test" };
        var response = new ProjectResponse { Id = 1, Name = "Test" };
        mockService.Setup(s => s.CreateAsync(It.IsAny<CreateProjectRequest>())).ReturnsAsync(response);

        var controller = new ProjectsController(mockService.Object, mockLogger.Object);

        var result = await controller.CreateAsync(request);

        var createdResult = Assert.IsType<CreatedResult>(result.Result);
        Assert.Equal(response, createdResult.Value);
    }

    [Fact]
    public async Task CreateAsync_ServiceThrowsArgumentException_ReturnsBadRequest()
    {
        var mockService = new Mock<IProjectService>();
        var mockLogger = new Mock<ILogger<ProjectsController>>();
        var request = new CreateProjectRequest { Name = "" };
        mockService.Setup(s => s.CreateAsync(It.IsAny<CreateProjectRequest>())).ThrowsAsync(new ArgumentException("Invalid"));

        var controller = new ProjectsController(mockService.Object, mockLogger.Object);

        var result = await controller.CreateAsync(request);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
