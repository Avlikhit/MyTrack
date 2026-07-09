import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { WorkLogService } from '../../../core/services/work-log.service';
import { ProjectService } from '../../../core/services/project.service';
import { PayrollSettingsService } from '../../../core/services/payroll-settings.service';

import { WorkLog } from '../../../core/models/work-log.model';
import { Project } from '../../../core/models/project.model';
import { PayrollSettings } from '../../../core/models/payroll-settings.model';

interface CalculatedPayDetail {
  id: number;
  workDate: string;
  projectName: string;
  ticketNumber: string;
  taskType: string;
  description: string;
  hoursWorked: number;
  hourlyRate: number;
  grossPay: number;
  federalTax: number;
  stateTax: number;
  socialSecurityTax: number;
  medicareTax: number;
  totalTaxes: number;
  netPay: number;
}

@Component({
  selector: 'app-pay-details',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './pay-details.html',
  styleUrl: './pay-details.scss'
})
export class PayDetails implements OnInit {

  payDetail?: CalculatedPayDetail;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workLogService: WorkLogService,
    private projectService: ProjectService,
    private payrollSettingsService: PayrollSettingsService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadPayDetail();
  }

  loadPayDetail(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'Invalid pay detail id.';
      this.isLoading = false;
      return;
    }

    this.payrollSettingsService.get().subscribe({
      next: (settings) => {
        this.projectService.getAll().subscribe({
          next: (projects) => {
            this.workLogService.getByDateRange('2026-01-01', '2026-12-31').subscribe({
              next: (workLogs) => {
                const workLog = workLogs.find(x => x.id === id);

                if (!workLog) {
                  this.errorMessage = 'Pay detail not found.';
                  this.isLoading = false;
                  this.cdr.detectChanges();
                  return;
                }

                this.payDetail = this.buildPayDetail(workLog, projects, settings);
                this.isLoading = false;
                this.cdr.detectChanges();
              },
              error: (error) => {
                console.error('Unable to load work logs', error);
                this.errorMessage = 'Unable to load pay detail.';
                this.isLoading = false;
                this.cdr.detectChanges();
              }
            });
          },
          error: (error) => {
            console.error('Unable to load projects', error);
            this.errorMessage = 'Unable to load project information.';
            this.isLoading = false;
            this.cdr.detectChanges();
          }
        });
      },
      error: (error) => {
        console.error('Unable to load payroll settings', error);
        this.errorMessage = 'Unable to load payroll settings.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  buildPayDetail(
    workLog: WorkLog,
    projects: Project[],
    settings: PayrollSettings
  ): CalculatedPayDetail {

    const project = projects.find(p => p.id === workLog.projectId);

    const hoursWorked = Number(workLog.hoursWorked ?? 0);
    const hourlyRate = Number(project?.hourlyRate ?? 0);

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
      projectName: project?.name ?? workLog.projectName,
      ticketNumber: workLog.ticketNumber,
      taskType: workLog.taskType,
      description: workLog.description,
      hoursWorked,
      hourlyRate,
      grossPay,
      federalTax,
      stateTax,
      socialSecurityTax,
      medicareTax,
      totalTaxes,
      netPay
    };
  }

  goBack(): void {
    this.router.navigate(['/pay-information']);
  }

  print(): void {
    window.print();
  }
}
