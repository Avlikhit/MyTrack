using MyTrack.Domain.Entities;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines data access operations for projects.
/// </summary>
public interface IProjectRepository
{
    /// <summary>
    /// Adds a new project.
    /// </summary>
    Task<Project> AddAsync(Project project);

    /// <summary>
    /// Updates an existing project.
    /// </summary>
    Task<Project> UpdateAsync(Project project);

    /// <summary>
    /// Gets a project by id.
    /// </summary>
    Task<Project?> GetByIdAsync(int id);

    /// <summary>
    /// Gets a project by id and user id.
    /// </summary>
    Task<Project?> GetByIdAsync(int id, int userId);

    /// <summary>
    /// Gets all projects.
    /// </summary>
    Task<IEnumerable<Project>> GetAllAsync(int userId);

    /// <summary>
    /// Gets all active projects.
    /// </summary>
    Task<IEnumerable<Project>> GetActiveAsync(int userId);

    /// <summary>
    /// Deletes an existing project.
    /// </summary>
    Task DeleteAsync(Project project);
}