import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

import { ProjectService } from '../../core/services/project.service';
import { Project } from '../../core/models/project.model';
import { AddProject } from './add-project/add-project';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './projects.html',
  styleUrl: './projects.scss'
})
export class Projects implements OnInit {

  projects: Project[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private projectService: ProjectService,
    private dialog: MatDialog,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    console.log('1. loadProjects started');

    this.isLoading = true;
    this.errorMessage = '';

    console.log('2. before API call');

    this.projectService.getActive().subscribe({
      next: (response) => {
        console.log('3. API response:', response);

        this.projects = response;
        this.isLoading = false;

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('4. API error:', error);

        this.errorMessage = 'Unable to load projects.';
        this.isLoading = false;

        this.cdr.detectChanges();
      },
      complete: () => {
        console.log('5. API complete');
      }
    });
  }

  openAddProject(): void {
    const dialogRef = this.dialog.open(AddProject, {
      width: '500px',
      maxHeight: 'none',
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadProjects();
      }
    });
  }

  openEditProject(project: Project): void {
    const dialogRef = this.dialog.open(AddProject, {
      width: '500px',
      maxHeight: 'none',
      autoFocus: false,
      data: project
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadProjects();
      }
    });
  }

  deleteProject(project: Project): void {
    if (!confirm(`Delete "${project.name}"?`)) {
      return;
    }

    this.projectService.delete(project.id).subscribe({
      next: () => this.loadProjects(),
      error: () => alert('Unable to delete project.')
    });
  }
}
