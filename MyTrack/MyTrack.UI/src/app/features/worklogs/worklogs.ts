import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WorkLogService } from '../../core/services/work-log.service';
import { WorkLog } from '../../core/models/work-log.model';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { AddWorkLog } from './add-work-log/add-work-log';

import { FormsModule } from '@angular/forms';

import { ProjectService } from '../../core/services/project.service';
import { Project } from '../../core/models/project.model';

import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { WorklogReportFilter, WorklogReportFilterResult } from './worklog-report-filter/worklog-report-filter';

import { Router } from '@angular/router';

@Component({
  selector: 'app-worklogs',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatPaginatorModule,
    MatSnackBarModule
  ],
  templateUrl: './worklogs.html',
  styleUrl: './worklogs.scss'
})
export class Worklogs implements OnInit {
  workLogs: WorkLog[] = [];
  filteredWorkLogs: WorkLog[] = [];
  pagedWorkLogs: WorkLog[] = [];

  pageSize = 5;
  pageIndex = 0;

  isLoading = false;
  errorMessage = '';

  selectedDate = new Date().toISOString().split('T')[0];

  projects: Project[] = [];
  selectedProjectId = 0;

  searchText = '';

  sortColumn = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  reportTitle = 'Work Log Report';
  reportRangeText = '';
  reportProjectText = '';

  constructor(
    private workLogService: WorkLogService,
    private projectService: ProjectService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadProjects();
    this.loadTodayWorkLogs();
  }

  view(id: number): void {
    this.router.navigate(['/worklogs', id]);
  }

  loadTodayWorkLogs(): void {
    this.selectedDate = new Date().toISOString().split('T')[0];
    this.loadWorkLogsByDate();
  }

  loadWorkLogsByDate(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.workLogService.getByDate(this.selectedDate).subscribe({
      next: (response) => {
        this.workLogs = response;
        this.applyProjectFilter();
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Work logs load error:', error);
        this.errorMessage = 'Unable to load work logs.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  clearDateFilter(): void {
    this.selectedDate = new Date().toISOString().split('T')[0];
    this.selectedProjectId = 0;
    this.searchText = '';

    this.loadWorkLogsByDate();
  }

  openAddWorkLog(): void {
    const dialogRef = this.dialog.open(AddWorkLog, {
      width: '560px',
      maxHeight: 'none',
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadWorkLogsByDate();
      }
    });
  }

  openEditWorkLog(workLog: WorkLog): void {
    const dialogRef = this.dialog.open(AddWorkLog, {
      width: '560px',
      maxHeight: 'none',
      autoFocus: false,
      data: workLog
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadWorkLogsByDate();
      }
    });
  }

  deleteWorkLog(workLog: WorkLog): void {
    if (!confirm(`Delete work log for "${workLog.projectName}"?`)) {
      return;
    }

    this.workLogService.delete(workLog.id).subscribe({
      next: () => {
        this.snackBar.open('Work log deleted successfully.', 'Close', {
          duration: 3000
        });

        this.loadWorkLogsByDate();
      },
      error: () => {
        this.snackBar.open('Unable to delete work log.', 'Close', {
          duration: 3000
        });
      }
    });
  }

  loadProjects(): void {
    this.projectService.getActive().subscribe({
      next: (response) => {
        this.projects = response;
      },
      error: (error) => {
        console.error('Unable to load projects', error);
      }
    });
  }

  applyProjectFilter(): void {
    let filtered = this.workLogs;

    if (this.selectedProjectId && this.selectedProjectId !== 0) {
      filtered = filtered.filter(
        workLog => workLog.projectId === Number(this.selectedProjectId)
      );
    }

    if (this.searchText.trim()) {
      const search = this.searchText.toLowerCase();

      filtered = filtered.filter(workLog =>
        String(workLog.ticketNumber ?? '').toLowerCase().includes(search) ||
        String(workLog.description ?? '').toLowerCase().includes(search)
      );
    }

    this.filteredWorkLogs = filtered;
    this.pageIndex = 0;
    this.updatePagedWorkLogs();
    this.cdr.detectChanges();
  }

  sortBy(column: keyof WorkLog): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.filteredWorkLogs = [...this.filteredWorkLogs].sort((a, b) => {
      let valueA: any = a[column];
      let valueB: any = b[column];

      if (column === 'ticketNumber' || column === 'hoursWorked') {
        valueA = Number(valueA);
        valueB = Number(valueB);
      }

      if (valueA === valueB) {
        return 0;
      }

      if (valueA == null) {
        return 1;
      }

      if (valueB == null) {
        return -1;
      }

      const result = valueA > valueB ? 1 : -1;

      return this.sortDirection === 'asc' ? result : -result;
    });

    this.pageIndex = 0;
    this.updatePagedWorkLogs();
    this.cdr.detectChanges();
  }

  updatePagedWorkLogs(): void {
    const startIndex = this.pageIndex * this.pageSize;
    const endIndex = startIndex + this.pageSize;

    this.pagedWorkLogs = this.filteredWorkLogs.slice(startIndex, endIndex);
  }

  printReport(): void {
    const dialogRef = this.dialog.open(WorklogReportFilter, {
      width: '520px',
      autoFocus: false,
      data: this.projects
    });

    dialogRef.afterClosed().subscribe((result: WorklogReportFilterResult | null) => {
      if (!result) {
        return;
      }

      this.loadReportAndPrint(result);
    });
  }

  loadReportAndPrint(filter: WorklogReportFilterResult): void {
    const dateRange = this.getReportDateRange(
      filter.selectedRange,
      filter.customStartDate,
      filter.customEndDate
    );

    this.workLogService.getByDateRange(dateRange.startDate, dateRange.endDate).subscribe({
      next: (response) => {
        let reportLogs = response;

        if (filter.selectedProjectId && filter.selectedProjectId !== 0) {
          reportLogs = reportLogs.filter(workLog =>
            workLog.projectId === Number(filter.selectedProjectId)
          );
        }

        this.reportRangeText = this.getReportRangeText(
          filter.selectedRange,
          filter.customStartDate,
          filter.customEndDate
        );

        const selectedProject = this.projects.find(
          project => project.id === Number(filter.selectedProjectId)
        );

        this.reportProjectText = selectedProject ? selectedProject.name : 'All Projects';

        this.filteredWorkLogs = reportLogs;
        this.pagedWorkLogs = reportLogs;

        this.cdr.detectChanges();

        window.onafterprint = () => {
          this.applyProjectFilter();
          window.onafterprint = null;
        };

        setTimeout(() => {
          window.print();
        }, 300);
      },
      error: () => {
        this.snackBar.open('Unable to load work log report.', 'Close', {
          duration: 3000
        });
      }
    });
  }

  getReportDateRange(range: string, customStartDate: string, customEndDate: string): { startDate: string; endDate: string } {
    const today = new Date();

    let startDate = new Date(today);
    let endDate = new Date(today);

    if (range === 'today') {
      startDate = today;
      endDate = today;
    }

    if (range === 'week') {
      startDate = new Date(today);
      startDate.setDate(today.getDate() - today.getDay());
      endDate = today;
    }

    if (range === '15days') {
      startDate = new Date(today);
      startDate.setDate(today.getDate() - 15);
      endDate = today;
    }

    if (range === 'month') {
      startDate = new Date(today.getFullYear(), today.getMonth(), 1);
      endDate = today;
    }

    if (range === 'year') {
      startDate = new Date(today.getFullYear(), 0, 1);
      endDate = today;
    }

    if (range === 'custom') {
      return {
        startDate: customStartDate,
        endDate: customEndDate
      };
    }

    if (range === 'all') {
      return {
        startDate: '2026-01-01',
        endDate: '2026-12-31'
      };
    }

    return {
      startDate: this.getDateString(startDate),
      endDate: this.getDateString(endDate)
    };
  }

  getReportRangeText(range: string, customStartDate: string, customEndDate: string): string {
    if (range === 'today') return 'Today';
    if (range === 'week') return 'This Week';
    if (range === '15days') return 'Last 15 Days';
    if (range === 'month') return 'This Month';
    if (range === 'year') return 'This Year';
    if (range === 'custom') return `${customStartDate} to ${customEndDate}`;
    if (range === 'all') return 'All Work Logs';

    return '';
  }

  getDateString(date: Date): string {
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${date.getFullYear()}-${month}-${day}`;
  }

  exportCsv(): void {
    if (!this.filteredWorkLogs || this.filteredWorkLogs.length === 0) {
      this.snackBar.open('No work logs available to export.', 'Close', {
        duration: 3000
      });
      return;
    }

    const headers = [
      'Date',
      'Project',
      'Ticket',
      'Task Type',
      'Description',
      'Hours'
    ];

    const rows = this.filteredWorkLogs.map(workLog => [
      workLog.workDate,
      workLog.projectName,
      workLog.ticketNumber,
      workLog.taskType,
      workLog.description,
      workLog.hoursWorked
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
    link.download = `worklogs-${this.selectedDate}.csv`;
    link.click();

    window.URL.revokeObjectURL(url);

    this.snackBar.open('Work logs exported successfully.', 'Close', {
      duration: 3000
    });
  }
}
