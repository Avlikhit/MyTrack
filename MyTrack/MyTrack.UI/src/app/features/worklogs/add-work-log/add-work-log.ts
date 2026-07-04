import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { WorkLogService } from '../../../core/services/work-log.service';
import { ProjectService } from '../../../core/services/project.service';

import { CreateWorkLogRequest } from '../../../core/models/create-work-log-request.model';
import { Project } from '../../../core/models/project.model';
import { WorkLog } from '../../../core/models/work-log.model';

@Component({
  selector: 'app-add-work-log',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule
  ],
  templateUrl: './add-work-log.html',
  styleUrl: './add-work-log.scss'
})
export class AddWorkLog implements OnInit {
  workLogId?: number;
  isEditMode = false;

  projects: Project[] = [];

  workDate = new Date().toISOString().split('T')[0];
  projectId = 0;
  ticketNumber = '';
  taskType = '';
  description = '';
  hoursWorked = 0;
  blockers = '';
  learnings = '';
  nextSteps = '';

  validationErrors: string[] = [];

  constructor(
    private workLogService: WorkLogService,
    private projectService: ProjectService,
    private dialogRef: MatDialogRef<AddWorkLog>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: WorkLog | null
  ) { }

  ngOnInit(): void {
    this.loadProjects();

    if (this.data) {
      this.isEditMode = true;
      this.workLogId = this.data.id;
      this.workDate = this.data.workDate;
      this.projectId = this.data.projectId;
      this.ticketNumber = this.data.ticketNumber;
      this.taskType = this.data.taskType;
      this.description = this.data.description;
      this.hoursWorked = this.data.hoursWorked;
      this.blockers = this.data.blockers;
      this.learnings = this.data.learnings;
      this.nextSteps = this.data.nextSteps;
    }
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

  save(): void {
    this.validationErrors = [];

    if (!this.workDate) {
      this.validationErrors.push('Work date is required.');
    }

    if (!this.projectId || this.projectId === 0) {
      this.validationErrors.push('Project is required.');
    }

    if (!this.ticketNumber.trim()) {
      this.validationErrors.push('Ticket number is required.');
    }

    if (!this.taskType.trim()) {
      this.validationErrors.push('Task type is required.');
    }

    if (!this.description.trim()) {
      this.validationErrors.push('Description is required.');
    }

    if (!this.hoursWorked || this.hoursWorked <= 0) {
      this.validationErrors.push('Hours worked must be greater than 0.');
    }

    if (this.hoursWorked > 24) {
      this.validationErrors.push('Hours worked cannot be more than 24.');
    }

    if (this.validationErrors.length > 0) {
      return;
    }

    const request: CreateWorkLogRequest = {
      workDate: this.workDate,
      projectId: this.projectId,
      ticketNumber: this.ticketNumber,
      taskType: this.taskType,
      description: this.description,
      hoursWorked: this.hoursWorked,
      blockers: this.blockers,
      learnings: this.learnings,
      nextSteps: this.nextSteps
    };

    if (this.isEditMode && this.workLogId) {
      this.workLogService.update(this.workLogId, request).subscribe({
        next: () => this.dialogRef.close(true),
        error: (error) => {
          console.error(error);
          alert('Unable to update work log.');
        }
      });

      return;
    }

    this.workLogService.create(request).subscribe({
      next: () => this.dialogRef.close(true),
      error: (error) => {
        console.error(error);
        alert('Unable to save work log.');
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
