export interface PayrollSettings {
  id: number;
  federalTaxPercent: number;
  stateTaxPercent: number;
  socialSecurityTaxPercent: number;
  medicareTaxPercent: number;
  createdDateTime: string;
  modifiedDateTime?: string;
}
