namespace MyTrack.Domain.Entities;

/// <summary>
/// Represents payroll tax settings used to calculate take-home pay.
/// </summary>
public class PayrollSettings
{
    /// <summary>
    /// Gets or sets the payroll settings id.
    /// </summary>
    public int Id { get; set; }

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

    /// <summary>
    /// Gets or sets the created date and time.
    /// </summary>
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the modified date and time.
    /// </summary>
    public DateTime? ModifiedDateTime { get; set; }
}