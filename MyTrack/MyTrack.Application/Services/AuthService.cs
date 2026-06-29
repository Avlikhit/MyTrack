using Microsoft.AspNetCore.Identity;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using MyTrack.Domain.Entities;

namespace MyTrack.Application.Services;

/// <summary>
/// Provides authentication logic for registering and logging in users.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthService"/> class.
    /// </summary>
    public AuthService(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = new PasswordHasher<User>();
    }

    /// <inheritdoc/>
    public async Task<LoginResponse> RegisterAsync(RegisterUserRequest request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new ArgumentException("A user with this email already exists.");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsActive = true,
            CreatedDateTime = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        var savedUser = await _userRepository.AddAsync(user);

        return new LoginResponse
        {
            UserId = savedUser.Id,
            FullName = $"{savedUser.FirstName} {savedUser.LastName}",
            Email = savedUser.Email,
            Token = _jwtTokenService.GenerateToken(savedUser)
        };
    }

    /// <inheritdoc/>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null || !user.IsActive)
        {
            throw new ArgumentException("Invalid email or password.");
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new ArgumentException("Invalid email or password.");
        }

        return new LoginResponse
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Token = _jwtTokenService.GenerateToken(user)
        };
    }
}