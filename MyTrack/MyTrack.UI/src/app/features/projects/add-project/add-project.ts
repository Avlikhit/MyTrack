import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit, Optional } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-add-project',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule
  ],
  templateUrl: './add-project.html',
  styleUrl: './add-project.scss'
})
export class AddProject implements OnInit {
  projectId?: number;
  isEditMode = false;

  name = '';
  description = '';
  colorCode = '#2563EB';
  displayOrder = 1;
  isDefault = false;
  isActive = true;
  errorMessage = '';

  constructor(
    private projectService: ProjectService,
    private dialogRef: MatDialogRef<AddProject>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: Project | null
  ) { }

  ngOnInit(): void {
    if (this.data) {
      this.isEditMode = true;
      this.projectId = this.data.id;
      this.name = this.data.name;
      this.description = this.data.description;
      this.colorCode = this.data.colorCode;
      this.displayOrder = this.data.displayOrder;
      this.isDefault = this.data.isDefault;
      this.isActive = this.data.isActive;
    }
  }

  saveProject(): void {
    this.errorMessage = '';

    if (this.isEditMode && this.projectId) {
      this.projectService.update(this.projectId, {
        name: this.name,
        description: this.description,
        colorCode: this.colorCode,
        displayOrder: this.displayOrder,
        isDefault: this.isDefault,
        isActive: this.isActive
      }).subscribe({
        next: () => this.dialogRef.close(true),
        error: () => this.errorMessage = 'Unable to update project.'
      });

      return;
    }

    this.projectService.create({
      name: this.name,
      description: this.description,
      colorCode: this.colorCode,
      displayOrder: this.displayOrder,
      isDefault: this.isDefault
    }).subscribe({
      next: () => this.dialogRef.close(true),
      error: () => this.errorMessage = 'Unable to create project.'
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }
}
