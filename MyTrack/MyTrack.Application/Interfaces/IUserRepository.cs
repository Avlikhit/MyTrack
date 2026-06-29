using MyTrack.Domain.Entities;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines data access operations for users.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Adds a new user.
    /// </summary>
    Task<User> AddAsync(User user);

    /// <summary>
    /// Gets a user by email address.
    /// </summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Gets a user by id.
    /// </summary>
    Task<User?> GetByIdAsync(int id);
}