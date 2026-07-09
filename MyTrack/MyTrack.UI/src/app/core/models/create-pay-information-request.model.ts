export interface CreatePayInformationRequest {
  payDate: string;
  payPeriodStart: string;
  payPeriodEnd: string;

  grossPay: number;
  federalTax: number;
  stateTax: number;
  socialSecurityTax: number;
  medicareTax: number;
  otherDeductions: number;

  netPay: number;
  notes?: string;
}
