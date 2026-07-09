namespace MyTrack.API.Models
{
    public class PayInformation
    {
        public int Id { get; set; }

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

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    }
}