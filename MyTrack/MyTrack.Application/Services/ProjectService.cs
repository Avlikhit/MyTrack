using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using MyTrack.Domain.Entities;

namespace MyTrack.Application.Services;

/// <summary>
/// Provides application logic for managing projects.
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectService"/> class.
    /// </summary>
    /// <param name="projectRepository">The project repository.</param>
    /// <param name="currentUserService">The current user service.</param>
    public ProjectService(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    /// <inheritdoc/>
    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var project = new Project
        {
            UserId = _currentUserService.UserId,
            Name = request.Name,
            Description = request.Description,
            ColorCode = request.ColorCode,
            DisplayOrder = request.DisplayOrder,
            IsDefault = request.IsDefault,
            IsActive = true,
            CreatedDateTime = DateTime.UtcNow
        };

        var savedProject = await _projectRepository.AddAsync(project);

        return MapToResponse(savedProject);
    }

    /// <inheritdoc/>
    public async Task<ProjectResponse?> UpdateAsync(int id, UpdateProjectRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (id <= 0)
        {
            throw new ArgumentException("Project id is required.", nameof(id));
        }

        var existingProject = await _projectRepository.GetByIdAsync(
            id,
            _currentUserService.UserId);

        if (existingProject is null)
        {
            return null;
        }

        existingProject.Name = request.Name;
        existingProject.Description = request.Description;
        existingProject.ColorCode = request.ColorCode;
        existingProject.DisplayOrder = request.DisplayOrder;
        existingProject.IsDefault = request.IsDefault;
        existingProject.IsActive = request.IsActive;
        existingProject.ModifiedDateTime = DateTime.UtcNow;

        var updatedProject = await _projectRepository.UpdateAsync(existingProject);

        return MapToResponse(updatedProject);
    }

    /// <inheritdoc/>
    public async Task<ProjectResponse?> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(
            id,
            _currentUserService.UserId);

        return project is null ? null : MapToResponse(project);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
    {
        var projects = await _projectRepository.GetAllAsync(
            _currentUserService.UserId);

        return projects.Select(MapToResponse);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProjectResponse>> GetActiveAsync()
    {
        var projects = await _projectRepository.GetActiveAsync(
            _currentUserService.UserId);

        return projects.Select(MapToResponse);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Project id is required.", nameof(id));
        }

        var existingProject = await _projectRepository.GetByIdAsync(id);

        if (existingProject is null)
        {
            return false;
        }

        existingProject.IsActive = false;
        existingProject.ModifiedDateTime = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(existingProject);

        return true;
    }

    /// <summary>
    /// Maps a Project domain entity to a ProjectResponse contract.
    /// </summary>
    /// <param name="project">The project entity.</param>
    /// <returns>The project response.</returns>
    private static ProjectResponse MapToResponse(Project project)
    {
        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            ColorCode = project.ColorCode,
            DisplayOrder = project.DisplayOrder,
            IsDefault = project.IsDefault,
            IsActive = project.IsActive,
            CreatedDateTime = project.CreatedDateTime,
            ModifiedDateTime = project.ModifiedDateTime
        };
    }
}