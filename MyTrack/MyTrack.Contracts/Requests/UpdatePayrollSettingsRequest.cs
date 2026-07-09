namespace MyTrack.Contracts.Requests;

/// <summary>
/// Represents a request to update payroll settings.
/// </summary>
public class UpdatePayrollSettingsRequest
{
    /// <summary>
    /// Gets or sets the federal tax percentage.
    /// </summary>
    public decimal FederalTaxPercent { get; set; }

    /// <summary>
    /// Gets or sets the state tax percentage.
    /// </summary>
    public decimal StateTaxPercent { get; set; }

    /// <summary>
    /// Gets or sets the Social Security tax percentage.
    /// </summary>
    public decimal SocialSecurityTaxPercent { get; set; }

    /// <summary>
    /// Gets or sets the Medicare tax percentage.
    /// </summary>
    public decimal MedicareTaxPercent { get; set; }
}