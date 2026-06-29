using Microsoft.EntityFrameworkCore;
using MyTrack.Application.Interfaces;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Data;

namespace MyTrack.Infrastructure.Repositories;

/// <summary>
/// Provides data access operations for users.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly MyTrackDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(MyTrackDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}