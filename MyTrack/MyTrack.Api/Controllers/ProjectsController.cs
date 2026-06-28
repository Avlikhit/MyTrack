using Microsoft.AspNetCore.Mvc;
using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Api.Controllers;

/// <summary>
/// Handles API requests related to projects.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectsController"/> class.
    /// </summary>
    public ProjectsController(
        IProjectService projectService,
        ILogger<ProjectsController> logger)
    {
        _projectService = projectService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new project.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectResponse>> CreateAsync(CreateProjectRequest request)
    {
        var response = await _projectService.CreateAsync(request);

        return Created($"/api/projects/{response.Id}", response);
    }

    /// <summary>
    /// Gets a project by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectResponse>> GetByIdAsync(int id)
    {
        var response = await _projectService.GetByIdAsync(id);

        if (response is null)
        {
            return NotFound(new { message = $"Project with id {id} was not found." });
        }

        return Ok(response);
    }

    /// <summary>
    /// Gets all projects.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetAllAsync()
    {
        var response = await _projectService.GetAllAsync();

        return Ok(response);
    }

    /// <summary>
    /// Gets active projects.
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetActiveAsync()
    {
        var response = await _projectService.GetActiveAsync();

        return Ok(response);
    }

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectResponse>> UpdateAsync(int id, UpdateProjectRequest request)
    {
        var response = await _projectService.UpdateAsync(id, request);

        if (response is null)
        {
            return NotFound(new { message = $"Project with id {id} was not found." });
        }

        return Ok(response);
    }

    /// <summary>
    /// Deletes an existing project.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var isDeleted = await _projectService.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound(new { message = $"Project with id {id} was not found." });
        }

        return NoContent();
    }
}