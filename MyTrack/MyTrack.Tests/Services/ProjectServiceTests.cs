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
        var request = new CreateProjectRequest { Name = "Test" };
        var savedProject = new Project { Id = 1, Name = "Test" };
        mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(savedProject);

        var service = new ProjectService(mockRepo.Object);

        var response = await service.CreateAsync(request);

        mockRepo.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
        Assert.Equal(savedProject.Id, response.Id);
        Assert.Equal(savedProject.Name, response.Name);
    }

    [Fact]
    public async Task CreateAsync_InvalidName_ThrowsArgumentException()
    {
        var mockRepo = new Mock<IProjectRepository>();
        var service = new ProjectService(mockRepo.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(new CreateProjectRequest { Name = "" }));
    }
}
