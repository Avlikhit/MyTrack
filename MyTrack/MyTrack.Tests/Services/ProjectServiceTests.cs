using System;
using System.Threading.Tasks;
using Moq;
using MyTrack.Application.Services;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Domain.Entities;
using Xunit;

namespace MyTrack.Tests.Services;

public class ProjectServiceTests
{
    [Fact]
    public async Task CreateAsync_CallsRepositoryAndReturnsResponse()
    {
        var mockRepo = new Mock<IProjectRepository>();
        var mockCurrentUser = new Mock<ICurrentUserService>();
        mockCurrentUser.SetupGet(c => c.UserId).Returns(42);
        mockCurrentUser.SetupGet(c => c.Email).Returns("test@example.com");

        var request = new CreateProjectRequest { Name = "Test" };
        var savedProject = new Project { Id = 1, Name = "Test" };
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(savedProject);

        var service = new ProjectService(mockRepo.Object, mockCurrentUser.Object);

        var response = await service.CreateAsync(request);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        Assert.Equal(savedProject.Id, response.Id);
        Assert.Equal(savedProject.Name, response.Name);
    }
}
