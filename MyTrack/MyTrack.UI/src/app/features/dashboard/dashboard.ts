import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { WorkLogService } from '../../core/services/work-log.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    FormsModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {

  totalWorkLogs = 0;
  totalHours = 0;
  totalProjects = 0;
  averageHours = 0;

  todayWorkLogs = 0;
  todayHours = 0;

  selectedRange = 'all';
  customStartDate = '';
  customEndDate = '';

  allWorkLogs: any[] = [];
  hoursByProject: { projectName: string; hours: number }[] = [];
  recentWorkLogs: any[] = [];
  hoursByDate: { workDate: string; hours: number }[] = [];

  topProjectName = '';
  topProjectHours = 0;

  userName = '';

  weeklyHours = 0;
  weeklyGoal = 40;
  weeklyProgressPercent = 0;

  constructor(
    private authService: AuthService,
    private router: Router,
    private workLogService: WorkLogService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.userName = localStorage.getItem('userName') ?? '';
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.workLogService.getByDateRange('2026-01-01', '2026-12-31').subscribe({
      next: (response) => {
        this.allWorkLogs = response;
        this.applyDashboardFilter();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Dashboard load error:', error);
      }
    });
  }

  applyDashboardFilter(): void {
    let filteredLogs = this.allWorkLogs;

    const today = new Date();
    const todayString = this.getTodayString();

    if (this.selectedRange === 'today') {
      filteredLogs = this.allWorkLogs.filter(workLog =>
        workLog.workDate === todayString
      );
    }

    if (this.selectedRange === 'week') {
      const weekStart = new Date(today);
      weekStart.setDate(today.getDate() - today.getDay());
      weekStart.setHours(0, 0, 0, 0);

      filteredLogs = this.allWorkLogs.filter(workLog => {
        const workDate = new Date(workLog.workDate);
        return workDate >= weekStart && workDate <= today;
      });
    }

    if (this.selectedRange === 'month') {
      filteredLogs = this.allWorkLogs.filter(workLog => {
        const workDate = new Date(workLog.workDate);
        return workDate.getMonth() === today.getMonth()
          && workDate.getFullYear() === today.getFullYear();
      });
    }

    if (this.selectedRange === 'year') {
      filteredLogs = this.allWorkLogs.filter(workLog => {
        const workDate = new Date(workLog.workDate);
        return workDate.getFullYear() === today.getFullYear();
      });
    }

    if (this.selectedRange === 'custom') {
      if (!this.customStartDate || !this.customEndDate) {
        return;
      }

      filteredLogs = this.allWorkLogs.filter(workLog =>
        workLog.workDate >= this.customStartDate &&
        workLog.workDate <= this.customEndDate
      );
    }

    this.calculateDashboard(filteredLogs);
    this.cdr.detectChanges();
  }

  calculateDashboard(filteredLogs: any[]): void {
    this.totalWorkLogs = filteredLogs.length;

    this.totalHours = filteredLogs.reduce(
      (sum, workLog) => sum + Number(workLog.hoursWorked ?? 0),
      0
    );

    this.calculateWeeklyProgress();

    this.totalProjects = new Set(
      filteredLogs.map(workLog => workLog.projectId)
    ).size;

    this.averageHours =
      this.totalWorkLogs === 0
        ? 0
        : this.totalHours / this.totalWorkLogs;

    const todayString = this.getTodayString();
    const todayLogs = this.allWorkLogs.filter(workLog =>
      workLog.workDate === todayString
    );

    this.todayWorkLogs = todayLogs.length;

    this.todayHours = todayLogs.reduce(
      (sum, workLog) => sum + Number(workLog.hoursWorked ?? 0),
      0
    );

    // These should always use ALL work logs, not filtered logs
    this.buildHoursByProject(this.allWorkLogs);
    this.buildRecentWorkLogs(this.allWorkLogs);
    this.buildHoursByDate(this.allWorkLogs);
    this.setTopProject();
  }

  calculateWeeklyProgress(): void {
    const today = new Date();

    const weekStart = new Date(today);
    weekStart.setDate(today.getDate() - today.getDay());
    weekStart.setHours(0, 0, 0, 0);

    const weekLogs = this.allWorkLogs.filter((workLog: any) => {
      const workDate = new Date(workLog.workDate);
      return workDate >= weekStart && workDate <= today;
    });

    this.weeklyHours = weekLogs.reduce(
      (sum, workLog) => sum + Number(workLog.hoursWorked ?? 0),
      0
    );

    this.weeklyProgressPercent =
      this.weeklyGoal === 0
        ? 0
        : Math.min((this.weeklyHours / this.weeklyGoal) * 100, 100);
  }

  buildHoursByProject(workLogs: any[]): void {
    const projectMap = new Map<string, number>();

    workLogs.forEach(workLog => {
      const projectName = workLog.projectName;
      const hours = Number(workLog.hoursWorked ?? 0);

      projectMap.set(
        projectName,
        (projectMap.get(projectName) ?? 0) + hours
      );
    });

    this.hoursByProject = Array.from(projectMap.entries()).map(
      ([projectName, hours]) => ({
        projectName,
        hours
      })
    );
  }

  buildRecentWorkLogs(workLogs: any[]): void {
    this.recentWorkLogs = [...workLogs]
      .sort((a, b) =>
        new Date(b.workDate).getTime() - new Date(a.workDate).getTime()
      )
      .slice(0, 5);
  }

  buildHoursByDate(workLogs: any[]): void {
    const dateMap = new Map<string, number>();

    workLogs.forEach(workLog => {
      const workDate = workLog.workDate;
      const hours = Number(workLog.hoursWorked ?? 0);

      dateMap.set(
        workDate,
        (dateMap.get(workDate) ?? 0) + hours
      );
    });

    this.hoursByDate = Array.from(dateMap.entries())
      .map(([workDate, hours]) => ({
        workDate,
        hours
      }))
      .sort((a, b) =>
        new Date(b.workDate).getTime() - new Date(a.workDate).getTime()
      );
  }

  setTopProject(): void {
    const topProject = [...this.hoursByProject]
      .sort((a, b) => b.hours - a.hours)[0];

    this.topProjectName = topProject?.projectName ?? '';
    this.topProjectHours = topProject?.hours ?? 0;
  }

  getTodayString(): string {
    const today = new Date();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${today.getFullYear()}-${month}-${day}`;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
