using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Api.Controllers;

/// <summary>
/// Handles notification settings for the logged-in user.
/// </summary>
[ApiController]
[Route("api/notification-settings")]
[Authorize]
public class NotificationSettingsController : ControllerBase
{
    private readonly INotificationSettingsService _notificationSettingsService;

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="NotificationSettingsController"/> class.
    /// </summary>
    /// <param name="notificationSettingsService">
    /// The notification settings service.
    /// </param>
    public NotificationSettingsController(
        INotificationSettingsService notificationSettingsService)
    {
        _notificationSettingsService = notificationSettingsService;
    }

    /// <summary>
    /// Gets notification settings for the logged-in user.
    /// </summary>
    /// <returns>The user's notification settings.</returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(NotificationSettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NotificationSettingsResponse>> GetAsync()
    {
        var response = await _notificationSettingsService.GetAsync(GetUserId());

        return Ok(response);
    }

    /// <summary>
    /// Updates notification settings for the logged-in user.
    /// </summary>
    /// <param name="request">The notification settings update request.</param>
    /// <returns>The updated notification settings.</returns>
    [HttpPut]
    [ProducesResponseType(
        typeof(NotificationSettingsResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<NotificationSettingsResponse>> UpdateAsync(
        UpdateNotificationSettingsRequest request)
    {
        try
        {
            var response = await _notificationSettingsService.UpdateAsync(
                GetUserId(),
                request);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    /// <summary>
    /// Gets the logged-in user identifier from authentication claims.
    /// </summary>
    /// <returns>The logged-in user identifier.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when the user identifier claim is missing or invalid.
    /// </exception>
    private int GetUserId()
    {
        var userIdClaim =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("userId")?.Value
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) ||
            !int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException(
                "A valid user id claim is missing.");
        }

        return userId;
    }
}