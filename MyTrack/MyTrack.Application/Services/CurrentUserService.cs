using Microsoft.AspNetCore.Http;
using MyTrack.Application.Interfaces;
using System.Security.Claims;

namespace MyTrack.Api.Services;

/// <summary>
/// Provides information about the currently authenticated user from the HTTP context.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public int UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userId, out var parsedUserId)
                ? parsedUserId
                : 0;
        }
    }

    /// <inheritdoc/>
    public string Email
    {
        get
        {
            return _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
    }
}