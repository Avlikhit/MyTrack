using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Api.Controllers;

/// <summary>
/// Handles authentication requests.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> RegisterAsync(RegisterUserRequest request)
    {
        var response = await _authService.RegisterAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Logs in an existing user.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        return Ok(response);
    }

    /// <summary>
    /// Gets the logged-in user's profile.
    /// </summary>
    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserProfileResponse>> GetProfileAsync()
    {
        var response = await _authService.GetProfileAsync(GetUserId());

        return Ok(response);
    }

    /// <summary>
    /// Updates the logged-in user's profile.
    /// </summary>
    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserProfileResponse>> UpdateProfileAsync(UpdateUserProfileRequest request)
    {
        var response = await _authService.UpdateProfileAsync(GetUserId(), request);

        return Ok(response);
    }

    /// <summary>
    /// Changes the logged-in user's password.
    /// </summary>
    [Authorize]
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        await _authService.ChangePasswordAsync(GetUserId(), request);

        return NoContent();
    }

    /// <summary>
    /// Gets the logged-in user id from claims.
    /// </summary>
    /// <returns>The logged-in user id.</returns>
    private int GetUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            throw new UnauthorizedAccessException("User id claim is missing.");
        }

        return int.Parse(userIdClaim);
    }
}