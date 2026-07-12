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
        if (!IsValidPassword(request.Password))
        {
            throw new ArgumentException(
                "Password must be at least 8 characters and include a number and special character.");
        }

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
            Role = request.Role,
            ContactNumber = request.ContactNumber,
            HomeAddress = request.HomeAddress,
            WorkAddress = request.WorkAddress,
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
    /// <inheritdoc/>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var email = request.Email.Trim();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            throw new ArgumentException(
                "No account exists with this email address. Please register first.");
        }

        if (!user.IsActive)
        {
            throw new ArgumentException(
                "This account is inactive. Please contact support.");
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new ArgumentException("The password you entered is incorrect.");
        }

        return new LoginResponse
        {
            UserId = user.Id,
            FullName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Token = _jwtTokenService.GenerateToken(user)
        };
    }

    /// <inheritdoc/>
    public async Task<UserProfileResponse> GetProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            throw new ArgumentException("User profile not found.");
        }

        return MapToProfileResponse(user);
    }

    /// <inheritdoc/>
    public async Task<UserProfileResponse> UpdateProfileAsync(
        int userId,
        UpdateUserProfileRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            throw new ArgumentException("User profile not found.");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Role = request.Role;
        user.ContactNumber = request.ContactNumber;
        user.HomeAddress = request.HomeAddress;
        user.WorkAddress = request.WorkAddress;
        user.ModifiedDateTime = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);

        return MapToProfileResponse(updatedUser);
    }

    /// <inheritdoc/>
    public async Task ChangePasswordAsync(
        int userId,
        ChangePasswordRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            throw new ArgumentException("New password and confirm password do not match.");
        }

        if (!IsValidPassword(request.NewPassword))
        {
            throw new ArgumentException("Password must be at least 8 characters and include a number and special character.");
        }

        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null || !user.IsActive)
        {
            throw new ArgumentException("User profile not found.");
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.CurrentPassword);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new ArgumentException("Current password is incorrect.");
        }

        user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
        user.ModifiedDateTime = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// Maps a user entity to a profile response.
    /// </summary>
    /// <param name="user">The user entity.</param>
    /// <returns>The user profile response.</returns>
    private static UserProfileResponse MapToProfileResponse(User user)
    {
        return new UserProfileResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Role = user.Role,
            ContactNumber = user.ContactNumber,
            HomeAddress = user.HomeAddress,
            WorkAddress = user.WorkAddress
        };
    }

    /// <summary>
    /// Validates password rules.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <returns>True if the password is valid; otherwise, false.</returns>
    private static bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password)
            && password.Length >= 8
            && password.Any(char.IsDigit)
            && password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}