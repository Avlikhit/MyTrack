import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { Project } from '../../../core/models/project.model';

export interface WorklogReportFilterResult {
  selectedRange: string;
  selectedProjectId: number;
  customStartDate: string;
  customEndDate: string;
}

@Component({
  selector: 'app-worklog-report-filter',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './worklog-report-filter.html',
  styleUrl: './worklog-report-filter.scss'
})
export class WorklogReportFilter {
  selectedRange = 'week';
  selectedProjectId = 0;
  customStartDate = '';
  customEndDate = '';

  constructor(
    private dialogRef: MatDialogRef<WorklogReportFilter>,
    @Inject(MAT_DIALOG_DATA) public projects: Project[]
  ) { }

  print(): void {
    this.dialogRef.close({
      selectedRange: this.selectedRange,
      selectedProjectId: Number(this.selectedProjectId),
      customStartDate: this.customStartDate,
      customEndDate: this.customEndDate
    });
  }

  cancel(): void {
    this.dialogRef.close(null);
  }
}
