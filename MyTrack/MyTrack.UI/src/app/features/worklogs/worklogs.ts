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

@Component({
  selector: 'app-worklogs',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './worklogs.html',
  styleUrl: './worklogs.scss'
})
export class Worklogs implements OnInit {
  workLogs: WorkLog[] = [];
  filteredWorkLogs: WorkLog[] = [];
  isLoading = false;
  errorMessage = '';

  selectedDate = new Date().toISOString().split('T')[0];

  projects: Project[] = [];
  selectedProjectId = 0;

  searchText = '';

  sortColumn = '';
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(
    private workLogService: WorkLogService,
    private projectService: ProjectService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProjects();
    this.loadTodayWorkLogs();
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
        this.loadWorkLogsByDate();
      },
      error: () => {
        alert('Unable to delete work log.');
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
        workLog.ticketNumber.toLowerCase().includes(search) ||
        workLog.description.toLowerCase().includes(search)
      );
    }

    this.filteredWorkLogs = filtered;
  }

  sortBy(column: keyof WorkLog): void {
    if (this.sortColumn === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortColumn = column;
      this.sortDirection = 'asc';
    }

    this.filteredWorkLogs = [...this.filteredWorkLogs].sort((a, b) => {
      const valueA = a[column];
      const valueB = b[column];

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
  }
}
