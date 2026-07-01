import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WorkLogService } from '../../core/services/work-log.service';
import { WorkLog } from '../../core/models/work-log.model';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { AddWorkLog } from './add-work-log/add-work-log';

@Component({
  selector: 'app-worklogs',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './worklogs.html',
  styleUrl: './worklogs.scss'
})
export class Worklogs implements OnInit {
  workLogs: WorkLog[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private workLogService: WorkLogService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadTodayWorkLogs();
  }

  loadTodayWorkLogs(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const today = new Date().toISOString().split('T')[0];

    this.workLogService.getByDate(today).subscribe({
      next: (response) => {
        this.workLogs = response;
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

  openAddWorkLog(): void {
    const dialogRef = this.dialog.open(AddWorkLog, {
      width: '560px',
      maxHeight: 'none',
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadTodayWorkLogs();
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
        this.loadTodayWorkLogs();
      }
    });
  }

  deleteWorkLog(workLog: WorkLog): void {
    if (!confirm(`Delete work log for "${workLog.projectName}"?`)) {
      return;
    }

    this.workLogService.delete(workLog.id).subscribe({
      next: () => {
        this.loadTodayWorkLogs();
      },
      error: () => {
        alert('Unable to delete work log.');
      }
    });
  }
}
