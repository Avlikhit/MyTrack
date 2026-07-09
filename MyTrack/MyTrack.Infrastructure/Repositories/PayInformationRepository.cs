using Microsoft.EntityFrameworkCore;
using MyTrack.API.Models;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Data;

namespace MyTrack.Infrastructure.Repositories;

public class PayInformationRepository : IPayInformationRepository
{
    private readonly MyTrackDbContext _context;

    public PayInformationRepository(MyTrackDbContext context)
    {
        _context = context;
    }

    public async Task<List<PayInformation>> GetAllAsync()
    {
        return await _context.PayInformations
            .OrderByDescending(x => x.PayDate)
            .ToListAsync();
    }

    public async Task<PayInformation?> GetByIdAsync(int id)
    {
        return await _context.PayInformations
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(PayInformation payInformation)
    {
        _context.PayInformations.Add(payInformation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(PayInformation payInformation)
    {
        _context.PayInformations.Update(payInformation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pay = await _context.PayInformations.FindAsync(id);

        if (pay == null)
            return;

        _context.PayInformations.Remove(pay);
        await _context.SaveChangesAsync();
    }
}