using MyTrack.Application.Interfaces;
using MyTrack.Contracts.Requests;
using MyTrack.Contracts.Responses;
using MyTrack.Domain.Entities;

namespace MyTrack.Application.Services;

/// <summary>
/// Provides application logic for payroll settings.
/// </summary>
public class PayrollSettingsService : IPayrollSettingsService
{
    private readonly IPayrollSettingsRepository _payrollSettingsRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="PayrollSettingsService"/> class.
    /// </summary>
    public PayrollSettingsService(IPayrollSettingsRepository payrollSettingsRepository)
    {
        _payrollSettingsRepository = payrollSettingsRepository;
    }

    /// <inheritdoc/>
    public async Task<PayrollSettingsResponse> GetAsync()
    {
        var settings = await _payrollSettingsRepository.GetAsync();

        if (settings is null)
        {
            settings = new PayrollSettings
            {
                FederalTaxPercent = 0,
                StateTaxPercent = 0,
                SocialSecurityTaxPercent = 0,
                MedicareTaxPercent = 0,
                CreatedDateTime = DateTime.UtcNow
            };

            settings = await _payrollSettingsRepository.AddAsync(settings);
        }

        return MapToResponse(settings);
    }

    /// <inheritdoc/>
    public async Task<PayrollSettingsResponse> UpdateAsync(UpdatePayrollSettingsRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var settings = await _payrollSettingsRepository.GetAsync();

        if (settings is null)
        {
            settings = new PayrollSettings
            {
                CreatedDateTime = DateTime.UtcNow
            };

            settings = await _payrollSettingsRepository.AddAsync(settings);
        }

        settings.FederalTaxPercent = request.FederalTaxPercent;
        settings.StateTaxPercent = request.StateTaxPercent;
        settings.SocialSecurityTaxPercent = request.SocialSecurityTaxPercent;
        settings.MedicareTaxPercent = request.MedicareTaxPercent;
        settings.ModifiedDateTime = DateTime.UtcNow;

        var updatedSettings = await _payrollSettingsRepository.UpdateAsync(settings);

        return MapToResponse(updatedSettings);
    }

    private static PayrollSettingsResponse MapToResponse(PayrollSettings settings)
    {
        return new PayrollSettingsResponse
        {
            Id = settings.Id,
            FederalTaxPercent = settings.FederalTaxPercent,
            StateTaxPercent = settings.StateTaxPercent,
            SocialSecurityTaxPercent = settings.SocialSecurityTaxPercent,
            MedicareTaxPercent = settings.MedicareTaxPercent,
            CreatedDateTime = settings.CreatedDateTime,
            ModifiedDateTime = settings.ModifiedDateTime
        };
    }
}