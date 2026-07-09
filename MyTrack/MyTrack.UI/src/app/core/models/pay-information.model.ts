export interface PayInformation {
  id: number;

  payDate: string;
  payPeriodStart: string;
  payPeriodEnd: string;

  grossPay: number;
  federalTax: number;
  stateTax: number;
  socialSecurityTax: number;
  medicareTax: number;
  otherDeductions: number;

  totalTaxes: number;
  totalDeductions: number;
  netPay: number;

  notes?: string;
  createdDateTime: string;
}
