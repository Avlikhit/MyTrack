using Microsoft.EntityFrameworkCore;
using MyTrack.Application.Interfaces;
using MyTrack.Domain.Entities;
using MyTrack.Infrastructure.Data;

namespace MyTrack.Infrastructure.Repositories;

/// <summary>
/// Repository for payroll settings.
/// </summary>
public class PayrollSettingsRepository : IPayrollSettingsRepository
{
    private readonly MyTrackDbContext _context;

    public PayrollSettingsRepository(MyTrackDbContext context)
    {
        _context = context;
    }

    public async Task<PayrollSettings?> GetAsync()
    {
        return await _context.PayrollSettings.FirstOrDefaultAsync();
    }

    public async Task<PayrollSettings> AddAsync(PayrollSettings payrollSettings)
    {
        _context.PayrollSettings.Add(payrollSettings);
        await _context.SaveChangesAsync();

        return payrollSettings;
    }

    public async Task<PayrollSettings> UpdateAsync(PayrollSettings payrollSettings)
    {
        _context.PayrollSettings.Update(payrollSettings);
        await _context.SaveChangesAsync();

        return payrollSettings;
    }
}