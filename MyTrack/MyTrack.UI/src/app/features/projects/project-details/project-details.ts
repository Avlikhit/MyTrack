import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ProjectService } from '../../../core/services/project.service';
import { WorkLogService } from '../../../core/services/work-log.service';

import { Project } from '../../../core/models/project.model';
import { WorkLog } from '../../../core/models/work-log.model';

@Component({
  selector: 'app-project-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './project-details.html',
  styleUrl: './project-details.scss'
})
export class ProjectDetails implements OnInit {
  project?: Project;
  workLogs: WorkLog[] = [];

  totalWorkLogs = 0;
  totalHours = 0;
  averageHours = 0;

  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private workLogService: WorkLogService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProjectDetails();
  }

  loadProjectDetails(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'Invalid project id.';
      return;
    }

    this.isLoading = true;

    this.projectService.getById(id).subscribe({
      next: (projectResponse) => {
        this.project = projectResponse;
        this.loadWorkLogsForProject(id);
      },
      error: (error) => {
        console.error('Project details load error:', error);
        this.errorMessage = 'Unable to load project details.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loadWorkLogsForProject(projectId: number): void {
    this.workLogService.getByDateRange('2026-01-01', '2026-12-31').subscribe({
      next: (response) => {
        this.workLogs = response.filter(workLog => workLog.projectId === projectId);

        this.totalWorkLogs = this.workLogs.length;

        this.totalHours = this.workLogs.reduce(
          (sum, workLog) => sum + Number(workLog.hoursWorked),
          0
        );

        this.averageHours =
          this.totalWorkLogs === 0
            ? 0
            : this.totalHours / this.totalWorkLogs;

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Project work logs load error:', error);
        this.errorMessage = 'Unable to load project work logs.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/projects']);
  }

  print(): void {
    window.print();
  }
}
