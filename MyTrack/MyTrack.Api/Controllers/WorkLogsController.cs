using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Api.Controllers;

/// <summary>
/// Handles API requests related to work logs.
/// </summary>
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
        try
        {
            var response = await _workLogService.CreateAsync(request);

            return Created($"/api/worklogs/{response.Id}", response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid request while creating work log.");

            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while creating work log.");

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "An unexpected error occurred while creating the work log."
            });
        }
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
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while getting work log with id {WorkLogId}.", id);

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "An unexpected error occurred while retrieving the work log."
            });
        }
    }
}