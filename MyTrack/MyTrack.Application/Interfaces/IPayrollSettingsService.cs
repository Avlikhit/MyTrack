using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;

namespace MyTrack.Application.Interfaces;

/// <summary>
/// Defines application operations for payroll settings.
/// </summary>
public interface IPayrollSettingsService
{
    /// <summary>
    /// Gets the payroll settings.
    /// </summary>
    Task<PayrollSettingsResponse> GetAsync();

    /// <summary>
    /// Updates payroll settings.
    /// </summary>
    Task<PayrollSettingsResponse> UpdateAsync(UpdatePayrollSettingsRequest request);
}