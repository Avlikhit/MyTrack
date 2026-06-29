using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Api.Controllers;

/// <summary>
/// Handles API requests related to work logs.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkLogsController : ControllerBase
{
    private readonly IWorkLogService _workLogService;
    private readonly ILogger<WorkLogsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkLogsController"/> class.
    /// </summary>
    /// <param name="workLogService">The work log application service.</param>
    /// <param name="logger">The logger instance.</param>
    public WorkLogsController(
        IWorkLogService workLogService,
        ILogger<WorkLogsController> logger)
    {
        _workLogService = workLogService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new work log.
    /// </summary>
    /// <param name="request">The work log creation request.</param>
    /// <returns>The created work log.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(WorkLogResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WorkLogResponse>> CreateAsync(CreateWorkLogRequest request)
    {
        var response = await _workLogService.CreateAsync(request);

        return Created($"/api/worklogs/{response.Id}", response);
    }

    /// <summary>
    /// Updates an existing work log.
    /// </summary>
    /// <param name="id">The work log identifier.</param>
    /// <param name="request">The work log update request.</param>
    /// <returns>The updated work log.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(WorkLogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WorkLogResponse>> UpdateAsync(int id, UpdateWorkLogRequest request)
    {
        var response = await _workLogService.UpdateAsync(id, request);

        if (response is null)
        {
            return NotFound(new
            {
                message = $"Work log with id {id} was not found."
            });
        }

        return Ok(response);
    }

    /// <summary>
    /// Deletes an existing work log.
    /// </summary>
    /// <param name="id">The work log identifier.</param>
    /// <returns>No content if deleted successfully.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var isDeleted = await _workLogService.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound(new
            {
                message = $"Work log with id {id} was not found."
            });
        }

        return NoContent();

    }

    /// <summary>
    /// Gets a work log by its unique identifier.
    /// </summary>
    /// <param name="id">The work log identifier.</param>
    /// <returns>The matching work log if found.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(WorkLogResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<WorkLogResponse>> GetByIdAsync(int id)
    {
        var response = await _workLogService.GetByIdAsync(id);

        if (response is null)
        {
            return NotFound(new
            {
                message = $"Work log with id {id} was not found."
            });
        }

        return Ok(response);

    }

    /// <summary>
    /// Gets all work logs for a specific date.
    /// </summary>
    /// <param name="workDate">The work date.</param>
    /// <returns>Work logs for the selected date.</returns>
    [HttpGet("date/{workDate}")]
    [ProducesResponseType(typeof(IEnumerable<WorkLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<WorkLogResponse>>> GetByDateAsync(DateOnly workDate)
    {

        var response = await _workLogService.GetByDateAsync(workDate);

        return Ok(response);

    }

    /// <summary>
    /// Gets all work logs within a date range.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    /// <returns>Work logs within the selected date range.</returns>
    [HttpGet("range")]
    [ProducesResponseType(typeof(IEnumerable<WorkLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<WorkLogResponse>>> GetByDateRangeAsync(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate)
    {

        if (startDate > endDate)
        {
            return BadRequest(new
            {
                message = "Start date cannot be greater than end date."
            });
        }

        var response = await _workLogService.GetByDateRangeAsync(startDate, endDate);

        return Ok(response);

    }
}