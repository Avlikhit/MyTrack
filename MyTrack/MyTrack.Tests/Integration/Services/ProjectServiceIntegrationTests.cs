using Moq;
using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;
using MyTrack.Contracts.Requests;
using MyTrack.Domain.Entities;
using Xunit;

namespace MyTrack.Tests.Integration.Services;

public class ProjectServiceIntegrationTests
{
    private Mock<IProjectRepository> _mockProjectRepository;
    private Mock<ICurrentUserService> _mockCurrentUserService;
    private ProjectService _projectService;

    public ProjectServiceIntegrationTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _projectService = new ProjectService(_mockProjectRepository.Object, _mockCurrentUserService.Object);

        _mockCurrentUserService.SetupGet(c => c.UserId).Returns(1);
        _mockCurrentUserService.SetupGet(c => c.Email).Returns("test@test.com");
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsProjectResponse()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "A test project",
            ColorCode = "#FF5733",
            DisplayOrder = 1,
            IsDefault = false
        };

        var savedProject = new Project
        {
            Id = 1,
            UserId = _mockCurrentUserService.Object.UserId,
            Name = request.Name,
            Description = request.Description,
            ColorCode = request.ColorCode,
            DisplayOrder = request.DisplayOrder,
            IsDefault = request.IsDefault,
            IsActive = true,
            CreatedDateTime = DateTime.UtcNow
        };

        _mockProjectRepository.Setup(r => r.AddAsync(It.IsAny<Project>()))
            .ReturnsAsync(savedProject);

        // Act
        var result = await _projectService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(savedProject.Id, result.Id);
        Assert.Equal(savedProject.Name, result.Name);
        Assert.Equal(savedProject.Description, result.Description);
        Assert.Equal(savedProject.ColorCode, result.ColorCode);
        Assert.True(result.IsActive);

        _mockProjectRepository.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _projectService.CreateAsync(null)
        );
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsProjectResponse()
    {
        // Arrange
        var projectId = 1;
        var project = new Project
        {
            Id = projectId,
            UserId = _mockCurrentUserService.Object.UserId,
            Name = "Test Project",
            Description = "A test project",
            IsActive = true,
            CreatedDateTime = DateTime.UtcNow
        };

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(project);

        // Act
        var result = await _projectService.GetByIdAsync(projectId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(project.Id, result.Id);
        Assert.Equal(project.Name, result.Name);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var projectId = 99999;

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync((Project)null);

        // Act
        var result = await _projectService.GetByIdAsync(projectId);

        // Assert
        Assert.Null(result);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsProjectList()
    {
        // Arrange
        var projects = new[]
        {
            new Project { Id = 1, Name = "Project 1", UserId = _mockCurrentUserService.Object.UserId },
            new Project { Id = 2, Name = "Project 2", UserId = _mockCurrentUserService.Object.UserId },
            new Project { Id = 3, Name = "Project 3", UserId = _mockCurrentUserService.Object.UserId }
        };

        _mockProjectRepository.Setup(r => r.GetAllAsync(_mockCurrentUserService.Object.UserId))
            .ReturnsAsync(projects);

        // Act
        var result = await _projectService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());

        _mockProjectRepository.Verify(r => r.GetAllAsync(_mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task GetActiveAsync_ReturnsActiveProjectList()
    {
        // Arrange
        var projects = new[]
        {
            new Project { Id = 1, Name = "Active Project 1", IsActive = true, UserId = _mockCurrentUserService.Object.UserId },
            new Project { Id = 2, Name = "Active Project 2", IsActive = true, UserId = _mockCurrentUserService.Object.UserId }
        };

        _mockProjectRepository.Setup(r => r.GetActiveAsync(_mockCurrentUserService.Object.UserId))
            .ReturnsAsync(projects);

        // Act
        var result = await _projectService.GetActiveAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        _mockProjectRepository.Verify(r => r.GetActiveAsync(_mockCurrentUserService.Object.UserId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsUpdatedProjectResponse()
    {
        // Arrange
        var projectId = 1;
        var existingProject = new Project
        {
            Id = projectId,
            UserId = _mockCurrentUserService.Object.UserId,
            Name = "Original Project",
            Description = "Original description",
            IsActive = true
        };

        var updateRequest = new UpdateProjectRequest
        {
            Name = "Updated Project",
            Description = "Updated description",
            IsActive = false
        };

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(existingProject);

        var updatedProject = new Project
        {
            Id = projectId,
            UserId = existingProject.UserId,
            Name = updateRequest.Name,
            Description = updateRequest.Description,
            IsActive = updateRequest.IsActive,
            CreatedDateTime = existingProject.CreatedDateTime,
            ModifiedDateTime = DateTime.UtcNow
        };

        _mockProjectRepository.Setup(r => r.UpdateAsync(It.IsAny<Project>()))
            .ReturnsAsync(updatedProject);

        // Act
        var result = await _projectService.UpdateAsync(projectId, updateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.Id);
        Assert.Equal(updateRequest.Name, result.Name);
        Assert.Equal(updateRequest.Description, result.Description);
        Assert.False(result.IsActive);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId), Times.Once);
        _mockProjectRepository.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var projectId = 99999;
        var updateRequest = new UpdateProjectRequest { Name = "Updated" };

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId))
            .ReturnsAsync((Project)null);

        // Act
        var result = await _projectService.UpdateAsync(projectId, updateRequest);

        // Assert
        Assert.Null(result);

        _mockProjectRepository.Verify(r => r.UpdateAsync(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _projectService.UpdateAsync(1, null)
        );
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidProjectId_ThrowsArgumentException()
    {
        // Arrange
        var updateRequest = new UpdateProjectRequest { Name = "Updated" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _projectService.UpdateAsync(-1, updateRequest)
        );
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var projectId = 1;
        var project = new Project { Id = projectId, UserId = _mockCurrentUserService.Object.UserId };

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId))
            .ReturnsAsync(project);

        _mockProjectRepository.Setup(r => r.DeleteAsync(project))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _projectService.DeleteAsync(projectId);

        // Assert
        Assert.True(result);

        _mockProjectRepository.Verify(r => r.GetByIdAsync(projectId, _mockCurrentUserService.Object.UserId), Times.Once);
        _mockProjectRepository.Verify(r => r.DeleteAsync(project), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var projectId = 99999;

        _mockProjectRepository.Setup(r => r.GetByIdAsync(projectId))
            .ReturnsAsync((Project)null);

        // Act
        var result = await _projectService.DeleteAsync(projectId);

        // Assert
        Assert.False(result);

        _mockProjectRepository.Verify(r => r.DeleteAsync(It.IsAny<Project>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidProjectId_ThrowsArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _projectService.DeleteAsync(-1)
        );
    }
}
