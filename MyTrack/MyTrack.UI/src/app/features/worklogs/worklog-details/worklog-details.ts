import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { WorkLogService } from '../../../core/services/work-log.service';
import { WorkLog } from '../../../core/models/work-log.model';

@Component({
  selector: 'app-worklog-details',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './worklog-details.html',
  styleUrl: './worklog-details.scss'
})
export class WorklogDetails implements OnInit {

  workLog?: WorkLog;
  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workLogService: WorkLogService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadWorkLog();
  }

  loadWorkLog(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!id) {
      this.errorMessage = 'Invalid work log id.';
      return;
    }

    this.isLoading = true;

    this.workLogService.getById(id).subscribe({

      next: (response) => {
        console.log('Work Log:', response);

        this.workLog = response;
        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Work log details load error:', error);

        this.errorMessage = 'Unable to load work log details.';
        this.isLoading = false;

        this.cdr.detectChanges();
      }

    });
  }

  goBack(): void {
    this.router.navigate(['/worklogs']);
  }

  print(): void {
    window.print();
  }

}
