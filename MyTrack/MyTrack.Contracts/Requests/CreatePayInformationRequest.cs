namespace MyTrack.Contracts.Requests;

public class CreatePayInformationRequest
{
    public DateTime PayDate { get; set; }
    public DateTime PayPeriodStart { get; set; }
    public DateTime PayPeriodEnd { get; set; }

    public decimal GrossPay { get; set; }
    public decimal FederalTax { get; set; }
    public decimal StateTax { get; set; }
    public decimal SocialSecurityTax { get; set; }
    public decimal MedicareTax { get; set; }
    public decimal OtherDeductions { get; set; }

    public decimal NetPay { get; set; }
    public string? Notes { get; set; }
}