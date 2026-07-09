import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { PayrollSettingsService } from '../../core/services/payroll-settings.service';

@Component({
  selector: 'app-payroll-settings',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './payroll-settings.html',
  styleUrl: './payroll-settings.scss'
})
export class PayrollSettings implements OnInit {
  federalTaxPercent = 0;
  stateTaxPercent = 0;
  socialSecurityTaxPercent = 0;
  medicareTaxPercent = 0;

  isLoading = false;
  errorMessage = '';

  constructor(
    private payrollSettingsService: PayrollSettingsService,
    private dialogRef: MatDialogRef<PayrollSettings>,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadSettings();
  }

  loadSettings(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.payrollSettingsService.get().subscribe({
      next: (response) => {
        this.federalTaxPercent = response.federalTaxPercent;
        this.stateTaxPercent = response.stateTaxPercent;
        this.socialSecurityTaxPercent = response.socialSecurityTaxPercent;
        this.medicareTaxPercent = response.medicareTaxPercent;

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Payroll settings load error:', error);
        this.errorMessage = 'Unable to load payroll settings.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  save(): void {
    this.errorMessage = '';

    this.payrollSettingsService.update({
      federalTaxPercent: Number(this.federalTaxPercent),
      stateTaxPercent: Number(this.stateTaxPercent),
      socialSecurityTaxPercent: Number(this.socialSecurityTaxPercent),
      medicareTaxPercent: Number(this.medicareTaxPercent)
    }).subscribe({
      next: () => {
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error('Payroll settings save error:', error);
        this.errorMessage = 'Unable to update payroll settings.';
        this.cdr.detectChanges();
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
