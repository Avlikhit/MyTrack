using MyTrack.Domain.Entities;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines repository operations for payroll settings.
/// </summary>
public interface IPayrollSettingsRepository
{
    /// <summary>
    /// Gets the payroll settings.
    /// </summary>
    Task<PayrollSettings?> GetAsync();

    /// <summary>
    /// Creates payroll settings.
    /// </summary>
    Task<PayrollSettings> AddAsync(PayrollSettings payrollSettings);

    /// <summary>
    /// Updates payroll settings.
    /// </summary>
    Task<PayrollSettings> UpdateAsync(PayrollSettings payrollSettings);
}