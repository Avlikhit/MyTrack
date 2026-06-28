using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines application operations for managing projects.
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// Creates a new project.
    /// </summary>
    Task<ProjectResponse> CreateAsync(CreateProjectRequest request);

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    Task<ProjectResponse?> UpdateAsync(int id, UpdateProjectRequest request);

    /// <summary>
    /// Gets a project by id.
    /// </summary>
    Task<ProjectResponse?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all projects.
    /// </summary>
    Task<IEnumerable<ProjectResponse>> GetAllAsync();

    /// <summary>
    /// Gets all active projects.
    /// </summary>
    Task<IEnumerable<ProjectResponse>> GetActiveAsync();

    /// <summary>
    /// Deletes a project.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}