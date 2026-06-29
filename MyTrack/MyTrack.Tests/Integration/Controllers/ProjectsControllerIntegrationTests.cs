using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MyTrack.Api;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using Xunit;

namespace MyTrack.Tests.Integration.Controllers;

public class ProjectsControllerIntegrationTests : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private string _authToken;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();

        // Register and login to get auth token
        var email = $"user_{Guid.NewGuid()}@test.com";
        var password = "SecurePassword123!";

        var registerRequest = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            Password = password
        };

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerRequest);
        var loginResponse = await registerResponse.Content.ReadFromJsonAsync<LoginResponse>();
        _authToken = loginResponse.Token;

        _client.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_ReturnsCreatedAndProjectResponse()
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

        // Act
        var response = await _client.PostAsJsonAsync("/api/projects", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(content);
        Assert.NotEqual(0, content.Id);
        Assert.Equal(request.Name, content.Name);
        Assert.Equal(request.Description, content.Description);
        Assert.Equal(request.ColorCode, content.ColorCode);
    }

    [Fact]
    public async Task CreateAsync_WithoutAuthorization_ReturnsUnauthorized()
    {
        // Arrange
        var clientWithoutAuth = _factory.CreateClient();
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "A test project"
        };

        // Act
        var response = await clientWithoutAuth.PostAsJsonAsync("/api/projects", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsOkAndProjectResponse()
    {
        // Arrange - Create a project first
        var createRequest = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "A test project"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Act
        var response = await _client.GetAsync($"/api/projects/{createdProject.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(content);
        Assert.Equal(createdProject.Id, content.Id);
        Assert.Equal(createdProject.Name, content.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/projects/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkAndProjectList()
    {
        // Arrange - Create some projects
        for (int i = 0; i < 3; i++)
        {
            var request = new CreateProjectRequest
            {
                Name = $"Test Project {i}",
                Description = $"Test project description {i}"
            };

            await _client.PostAsJsonAsync("/api/projects", request);
        }

        // Act
        var response = await _client.GetAsync("/api/projects");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectResponse>>();
        Assert.NotNull(content);
        Assert.NotEmpty(content);
    }

    [Fact]
    public async Task GetActiveAsync_ReturnsOkAndActiveProjectList()
    {
        // Arrange - Create an active project
        var request = new CreateProjectRequest
        {
            Name = "Active Project",
            Description = "An active project"
        };

        await _client.PostAsJsonAsync("/api/projects", request);

        // Act
        var response = await _client.GetAsync("/api/projects/active");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectResponse>>();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsOkAndUpdatedProjectResponse()
    {
        // Arrange - Create a project first
        var createRequest = new CreateProjectRequest
        {
            Name = "Original Project",
            Description = "Original description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        var updateRequest = new UpdateProjectRequest
        {
            Name = "Updated Project",
            Description = "Updated description",
            IsActive = false
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/projects/{createdProject.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(content);
        Assert.Equal(createdProject.Id, content.Id);
        Assert.Equal(updateRequest.Name, content.Name);
        Assert.Equal(updateRequest.Description, content.Description);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new UpdateProjectRequest
        {
            Name = "Updated Project"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/projects/99999", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsNoContent()
    {
        // Arrange - Create a project first
        var createRequest = new CreateProjectRequest
        {
            Name = "Project to Delete",
            Description = "This project will be deleted"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/projects", createRequest);
        var createdProject = await createResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/projects/{createdProject.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify project is deleted
        var getResponse = await _client.GetAsync($"/api/projects/{createdProject.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/projects/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
