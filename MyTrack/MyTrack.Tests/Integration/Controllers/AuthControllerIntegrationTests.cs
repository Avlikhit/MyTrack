using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MyTrack.Api;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using Xunit;

namespace MyTrack.Tests.Integration.Controllers;

public class AuthControllerIntegrationTests : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    public async Task InitializeAsync()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
        await Task.CompletedTask;
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
    public async Task RegisterAsync_WithValidRequest_ReturnsOkAndLoginResponse()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = $"user_{Guid.NewGuid()}@test.com",
            Password = "SecurePassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(content);
        Assert.NotEmpty(content.Token);
        Assert.Equal(request.Email, content.Email);
        Assert.Equal($"{request.FirstName} {request.LastName}", content.FullName);
    }

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        var email = $"user_{Guid.NewGuid()}@test.com";
        var request = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            Password = "SecurePassword123!"
        };

        // Register first user
        await _client.PostAsJsonAsync("/api/auth/register", request);

        // Attempt to register with same email
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsOkAndLoginResponse()
    {
        // Arrange
        var email = $"user_{Guid.NewGuid()}@test.com";
        var password = "SecurePassword123!";

        var registerRequest = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            Password = password
        };

        // Register user first
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(content);
        Assert.NotEmpty(content.Token);
        Assert.Equal(email, content.Email);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@test.com",
            Password = "SomePassword123!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsBadRequest()
    {
        // Arrange
        var email = $"user_{Guid.NewGuid()}@test.com";
        var password = "SecurePassword123!";

        var registerRequest = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            Password = password
        };

        // Register user first
        await _client.PostAsJsonAsync("/api/auth/register", registerRequest);

        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = "WrongPassword!"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
