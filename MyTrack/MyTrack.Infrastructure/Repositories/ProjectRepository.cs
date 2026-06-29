using Microsoft.EntityFrameworkCore;
using MyTrack.Application.Interfaces;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Data;

namespace MyTrack.Infrastructure.Repositories;

/// <summary>
/// Provides data access operations for projects.
/// </summary>
public class ProjectRepository : IProjectRepository
{
    private readonly MyTrackDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ProjectRepository(MyTrackDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Project> AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        return project;
    }

    /// <inheritdoc/>
    public async Task<Project> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        return project;
    }

    /// <inheritdoc/>
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public async Task<Project?> GetByIdAsync(int id, int userId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Project>> GetAllAsync(int userId)
    {
        return await _context.Projects
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Project>> GetActiveAsync(int userId)
    {
        return await _context.Projects
            .Where(x => x.UserId == userId && x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}