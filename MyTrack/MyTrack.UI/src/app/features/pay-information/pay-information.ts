import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { WorkLogService } from '../../core/services/work-log.service';
import { ProjectService } from '../../core/services/project.service';
import { PayrollSettingsService } from '../../core/services/payroll-settings.service';

import { WorkLog } from '../../core/models/work-log.model';
import { Project } from '../../core/models/project.model';
import { PayrollSettings } from '../../core/models/payroll-settings.model';

import { PayrollSettings as PayrollSettingsDialog } from '../payroll-settings/payroll-settings';

interface CalculatedPayInformation {
  id: number;
  workDate: string;
  projectId: number;
  projectName: string;
  hoursWorked: number;
  hourlyRate: number;
  grossPay: number;
  totalTaxes: number;
  netPay: number;
}

@Component({
  selector: 'app-pay-information',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule
  ],
  templateUrl: './pay-information.html',
  styleUrl: './pay-information.scss'
})
export class PayInformation implements OnInit {

  allPayInformation: CalculatedPayInformation[] = [];
  payInformation: CalculatedPayInformation[] = [];

  workLogs: WorkLog[] = [];
  projects: Project[] = [];
  payrollSettings?: PayrollSettings;

  selectedRange = 'all';
  selectedProjectId = 0;
  customStartDate = '';
  customEndDate = '';

  totalHours = 0;
  totalGrossPay = 0;
  totalNetPay = 0;
  totalTaxes = 0;

  constructor(
    private workLogService: WorkLogService,
    private projectService: ProjectService,
    private payrollSettingsService: PayrollSettingsService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadPayInformation();
  }

  loadPayInformation(): void {
    this.payrollSettingsService.get().subscribe({
      next: (settings) => {
        this.payrollSettings = settings;
        this.loadProjectsAndWorkLogs();
      },
      error: (error) => {
        console.error('Unable to load payroll settings', error);
      }
    });
  }

  loadProjectsAndWorkLogs(): void {
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects = projects;

        this.workLogService.getByDateRange('2026-01-01', '2026-12-31').subscribe({
          next: (workLogs) => {
            this.workLogs = workLogs;
            this.buildCalculatedPay();
            this.applyFilter();
            this.cdr.detectChanges();
          },
          error: (error) => {
            console.error('Unable to load work logs for pay calculation', error);
          }
        });
      },
      error: (error) => {
        console.error('Unable to load projects for pay calculation', error);
      }
    });
  }

  buildCalculatedPay(): void {
    const settings = this.payrollSettings;

    if (!settings) {
      this.allPayInformation = [];
      return;
    }

    this.allPayInformation = this.workLogs.map(workLog => {
      const project = this.projects.find(p => p.id === workLog.projectId);

      const hourlyRate = Number(project?.hourlyRate ?? 0);
      const hoursWorked = Number(workLog.hoursWorked ?? 0);

      const grossPay = hoursWorked * hourlyRate;

      const federalTax = grossPay * (Number(settings.federalTaxPercent) / 100);
      const stateTax = grossPay * (Number(settings.stateTaxPercent) / 100);
      const socialSecurityTax = grossPay * (Number(settings.socialSecurityTaxPercent) / 100);
      const medicareTax = grossPay * (Number(settings.medicareTaxPercent) / 100);

      const totalTaxes = federalTax + stateTax + socialSecurityTax + medicareTax;
      const netPay = grossPay - totalTaxes;

      return {
        id: workLog.id,
        workDate: workLog.workDate,
        projectId: workLog.projectId,
        projectName: workLog.projectName,
        hoursWorked,
        hourlyRate,
        grossPay,
        totalTaxes,
        netPay
      };
    });
  }

  applyFilter(): void {
    let filtered = this.allPayInformation;

    const today = new Date();
    const todayString = this.getDateString(today);

    if (this.selectedRange === 'today') {
      filtered = filtered.filter(pay =>
        pay.workDate.split('T')[0] === todayString
      );
    }

    if (this.selectedRange === 'week') {
      const weekStart = new Date(today);
      weekStart.setDate(today.getDate() - today.getDay());
      weekStart.setHours(0, 0, 0, 0);

      filtered = filtered.filter(pay => {
        const payDate = new Date(pay.workDate);
        return payDate >= weekStart && payDate <= today;
      });
    }

    if (this.selectedRange === '15days') {
      const startDate = new Date(today);
      startDate.setDate(today.getDate() - 15);
      startDate.setHours(0, 0, 0, 0);

      filtered = filtered.filter(pay => {
        const payDate = new Date(pay.workDate);
        return payDate >= startDate && payDate <= today;
      });
    }

    if (this.selectedRange === 'month') {
      filtered = filtered.filter(pay => {
        const payDate = new Date(pay.workDate);
        return payDate.getMonth() === today.getMonth()
          && payDate.getFullYear() === today.getFullYear();
      });
    }

    if (this.selectedRange === 'year') {
      filtered = filtered.filter(pay => {
        const payDate = new Date(pay.workDate);
        return payDate.getFullYear() === today.getFullYear();
      });
    }

    if (this.selectedRange === 'custom' && this.customStartDate && this.customEndDate) {
      filtered = filtered.filter(pay => {
        const payDate = pay.workDate.split('T')[0];
        return payDate >= this.customStartDate && payDate <= this.customEndDate;
      });
    }

    if (this.selectedProjectId && Number(this.selectedProjectId) !== 0) {
      filtered = filtered.filter(pay =>
        pay.projectId === Number(this.selectedProjectId)
      );
    }

    this.payInformation = filtered;
    this.calculateTotals();
    this.cdr.detectChanges();
  }

  calculateTotals(): void {
    this.totalHours = this.payInformation.reduce(
      (sum, pay) => sum + Number(pay.hoursWorked ?? 0),
      0
    );

    this.totalGrossPay = this.payInformation.reduce(
      (sum, pay) => sum + Number(pay.grossPay ?? 0),
      0
    );

    this.totalNetPay = this.payInformation.reduce(
      (sum, pay) => sum + Number(pay.netPay ?? 0),
      0
    );

    this.totalTaxes = this.payInformation.reduce(
      (sum, pay) => sum + Number(pay.totalTaxes ?? 0),
      0
    );
  }

  getDateString(date: Date): string {
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${date.getFullYear()}-${month}-${day}`;
  }

  openPayrollSettings(): void {
    const dialogRef = this.dialog.open(PayrollSettingsDialog, {
      width: '650px',
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadPayInformation();
      }
    });
  }

  printReport(): void {
    window.print();
  }

  exportCsv(): void {
    if (!this.payInformation || this.payInformation.length === 0) {
      alert('No pay records available to export.');
      return;
    }

    const headers = [
      'Date',
      'Project',
      'Hours',
      'Hourly Rate',
      'Gross Pay',
      'Taxes',
      'Take Home'
    ];

    const rows = this.payInformation.map(pay => [
      pay.workDate,
      pay.projectName,
      pay.hoursWorked,
      pay.hourlyRate,
      pay.grossPay,
      pay.totalTaxes,
      pay.netPay
    ]);

    const csvContent = [
      headers.join(','),
      ...rows.map(row =>
        row.map(value => `"${String(value ?? '').replace(/"/g, '""')}"`).join(',')
      )
    ].join('\n');

    const blob = new Blob([csvContent], {
      type: 'text/csv;charset=utf-8;'
    });

    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');

    link.href = url;
    link.download = `pay-summary-${this.selectedRange}.csv`;
    link.click();

    window.URL.revokeObjectURL(url);
  }

  view(id: number): void {
    this.router.navigate(['/pay-information', id]);
  }
}
