using Microsoft.AspNetCore.Identity;
using Moq;
using MyTrack.Application.Interfaces;
using MyTrack.Application.Services;
using MyTrack.Contracts.Requests;
using MyTrack.Domain.Entities;
using Xunit;

namespace MyTrack.Tests.Integration.Services;

public class AuthServiceIntegrationTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IJwtTokenService> _mockJwtTokenService;
    private AuthService _authService;

    public AuthServiceIntegrationTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _authService = new AuthService(_mockUserRepository.Object, _mockJwtTokenService.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithValidRequest_ReturnsLoginResponseWithToken()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Password = "SecurePassword123!"
        };

        var newUser = new User
        {
            Id = 1,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsActive = true,
            CreatedDateTime = DateTime.UtcNow
        };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((User)null);

        _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(newUser);

        _mockJwtTokenService.Setup(s => s.GenerateToken(It.IsAny<User>()))
            .Returns("test-token");

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newUser.Id, result.UserId);
        Assert.Equal($"{request.FirstName} {request.LastName}", result.FullName);
        Assert.Equal(request.Email, result.Email);
        Assert.NotEmpty(result.Token);

        _mockUserRepository.Verify(r => r.GetByEmailAsync(request.Email), Times.Once);
        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingEmail_ThrowsArgumentException()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "existing@test.com",
            Password = "SecurePassword123!"
        };

        var existingUser = new User
        {
            Id = 1,
            Email = request.Email,
            FirstName = "Jane",
            LastName = "Smith"
        };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _authService.RegisterAsync(request)
        );

        _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsLoginResponseWithToken()
    {
        // Arrange
        var email = "john.doe@test.com";
        var password = "SecurePassword123!";

        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            IsActive = true,
            CreatedDateTime = DateTime.UtcNow
        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        _mockUserRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(user);

        _mockJwtTokenService.Setup(s => s.GenerateToken(It.IsAny<User>()))
            .Returns("test-token");

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.UserId);
        Assert.Equal(user.Email, result.Email);
        Assert.NotEmpty(result.Token);

        _mockUserRepository.Verify(r => r.GetByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        var request = new LoginRequest
        {
            Email = "nonexistent@test.com",
            Password = "SomePassword123!"
        };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((User)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _authService.LoginAsync(request)
        );
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ThrowsArgumentException()
    {
        // Arrange
        var email = "john.doe@test.com";
        var correctPassword = "SecurePassword123!";
        var wrongPassword = "WrongPassword!";

        var request = new LoginRequest
        {
            Email = email,
            Password = wrongPassword
        };

        var user = new User
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = email,
            IsActive = true
        };

        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, correctPassword);

        _mockUserRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(user);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _authService.LoginAsync(request)
        );
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_ThrowsArgumentException()
    {
        // Arrange
        var email = "inactive@test.com";
        var password = "SecurePassword123!";

        var request = new LoginRequest
        {
            Email = email,
            Password = password
        };

        var inactiveUser = new User
        {
            Id = 1,
            Email = email,
            IsActive = false
        };

        _mockUserRepository.Setup(r => r.GetByEmailAsync(email))
            .ReturnsAsync(inactiveUser);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => _authService.LoginAsync(request)
        );
    }
}
