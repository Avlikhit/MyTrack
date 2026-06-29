using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MyTrack.Api;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using Xunit;

namespace MyTrack.Tests.Integration.Controllers;

public class WorkLogsControllerIntegrationTests : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private string _authToken;
    private int _projectId;

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

        // Create a project for testing work logs
        var projectRequest = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "A test project for work logs"
        };

        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectRequest);
        var createdProject = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();
        _projectId = createdProject.Id;
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
    public async Task CreateAsync_WithValidRequest_ReturnsCreatedAndWorkLogResponse()
    {
        // Arrange
        var request = new CreateWorkLogRequest
        {
            ProjectId = _projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development",
            Description = "Developed new feature",
            TicketNumber = "TICKET-001"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/worklogs", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<WorkLogResponse>();
        Assert.NotNull(content);
        Assert.NotEqual(0, content.Id);
        Assert.Equal(request.ProjectId, content.ProjectId);
        Assert.Equal(request.HoursWorked, content.HoursWorked);
        Assert.Equal(request.TaskType, content.TaskType);
    }

    [Fact]
    public async Task CreateAsync_WithoutAuthorization_ReturnsUnauthorized()
    {
        // Arrange
        var clientWithoutAuth = _factory.CreateClient();
        var request = new CreateWorkLogRequest
        {
            ProjectId = _projectId,
            HoursWorked = 8
        };

        // Act
        var response = await clientWithoutAuth.PostAsJsonAsync("/api/worklogs", request);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidProject_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateWorkLogRequest
        {
            ProjectId = 99999,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/worklogs", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsOkAndWorkLogResponse()
    {
        // Arrange - Create a work log first
        var createRequest = new CreateWorkLogRequest
        {
            ProjectId = _projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development",
            Description = "Developed new feature"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/worklogs", createRequest);
        var createdWorkLog = await createResponse.Content.ReadFromJsonAsync<WorkLogResponse>();

        // Act
        var response = await _client.GetAsync($"/api/worklogs/{createdWorkLog.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<WorkLogResponse>();
        Assert.NotNull(content);
        Assert.Equal(createdWorkLog.Id, content.Id);
        Assert.Equal(createdWorkLog.HoursWorked, content.HoursWorked);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/worklogs/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByDateAsync_WithValidDate_ReturnsOkAndWorkLogList()
    {
        // Arrange - Create a new project first for this test
        var projectRequest = new CreateProjectRequest
        {
            Name = "Get By Date Test Project",
            Description = "For GetByDateAsync test"
        };
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectRequest);
        var testProject = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        // Create work logs for today
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        for (int i = 0; i < 2; i++)
        {
            var request = new CreateWorkLogRequest
            {
                ProjectId = testProject.Id,
                WorkDate = today,
                HoursWorked = 4 + i,
                TaskType = "Development",
                Description = $"Task {i}"
            };

            await _client.PostAsJsonAsync("/api/worklogs", request);
        }

        // Act
        var response = await _client.GetAsync($"/api/worklogs/date/{today:yyyy-MM-dd}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<WorkLogResponse>>();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task GetByDateRangeAsync_WithValidRange_ReturnsOkAndWorkLogList()
    {
        // Arrange - Create work logs
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow);

        for (int i = 0; i < 2; i++)
        {
            var request = new CreateWorkLogRequest
            {
                ProjectId = _projectId,
                WorkDate = startDate.AddDays(i),
                HoursWorked = 8,
                TaskType = "Development"
            };

            await _client.PostAsJsonAsync("/api/worklogs", request);
        }

        // Act
        var response = await _client.GetAsync($"/api/worklogs/range?startDate={startDate}&endDate={endDate}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<IEnumerable<WorkLogResponse>>();
        Assert.NotNull(content);
    }

    [Fact]
    public async Task GetByDateRangeAsync_WithInvalidRange_ReturnsBadRequest()
    {
        // Arrange
        var startDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var endDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));

        // Act
        var response = await _client.GetAsync($"/api/worklogs/range?startDate={startDate}&endDate={endDate}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_ReturnsOkAndUpdatedWorkLogResponse()
    {
        // Arrange - Create a work log first
        var createRequest = new CreateWorkLogRequest
        {
            ProjectId = _projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development",
            Description = "Original description"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/worklogs", createRequest);
        var createdWorkLog = await createResponse.Content.ReadFromJsonAsync<WorkLogResponse>();

        var updateRequest = new UpdateWorkLogRequest
        {
            ProjectId = _projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 10,
            TaskType = "Testing",
            Description = "Updated description"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/worklogs/{createdWorkLog.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<WorkLogResponse>();
        Assert.NotNull(content);
        Assert.Equal(createdWorkLog.Id, content.Id);
        Assert.Equal(updateRequest.HoursWorked, content.HoursWorked);
        Assert.Equal(updateRequest.TaskType, content.TaskType);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new UpdateWorkLogRequest
        {
            ProjectId = _projectId,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development",
            Description = "Update test"
        };

        // Act
        var response = await _client.PutAsJsonAsync("/api/worklogs/99999", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsNoContent()
    {
        // Arrange - Create a new project and work log for this test
        var projectRequest = new CreateProjectRequest
        {
            Name = "Delete Test Project",
            Description = "For DeleteAsync test"
        };
        var projectResponse = await _client.PostAsJsonAsync("/api/projects", projectRequest);
        var testProject = await projectResponse.Content.ReadFromJsonAsync<ProjectResponse>();

        var createRequest = new CreateWorkLogRequest
        {
            ProjectId = testProject.Id,
            WorkDate = DateOnly.FromDateTime(DateTime.UtcNow),
            HoursWorked = 8,
            TaskType = "Development",
            Description = "Test work log for deletion"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/worklogs", createRequest);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var createdWorkLog = await createResponse.Content.ReadFromJsonAsync<WorkLogResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/worklogs/{createdWorkLog.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify work log is deleted
        var getResponse = await _client.GetAsync($"/api/worklogs/{createdWorkLog.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/worklogs/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
