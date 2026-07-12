import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { WorkLogService } from '../../core/services/work-log.service';

import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { NotificationSettingsDialog } from './notification-settings-dialog/notification-settings-dialog';

import { NotificationSettingsService } from '../../core/services/notification-settings.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    RouterLinkActive,
    MatDialogModule
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

  notificationMessages: string[] = [];
  notificationCount = 0;

  showNotificationPanel = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private workLogService: WorkLogService,
    private cdr: ChangeDetectorRef,
    private dialog: MatDialog,
    private notificationSettingsService: NotificationSettingsService
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

  loadDashboardNotifications(): void {
    this.notificationMessages = [];
    this.notificationCount = 0;

    this.notificationSettingsService.get().subscribe({
      next: (settings) => {
        if (!settings.notificationsEnabled) {
          return;
        }

        this.checkDailyWorkLogReminder(settings);
        this.checkMonthlySummaryReminder(settings);

        this.notificationCount = this.notificationMessages.length;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Unable to load dashboard notifications', error);
      }
    });
  }

  checkDailyWorkLogReminder(settings: any): void {
    if (!settings.dailyWorkLogReminderEnabled) {
      return;
    }

    const now = new Date();

    const reminderParts = settings.dailyReminderTime
      .substring(0, 5)
      .split(':');

    const reminderHour = Number(reminderParts[0]);
    const reminderMinute = Number(reminderParts[1]);

    const reminderTime = new Date(now);
    reminderTime.setHours(reminderHour, reminderMinute, 0, 0);

    if (now < reminderTime) {
      return;
    }

    if (this.todayWorkLogs === 0) {
      this.notificationMessages.push(
        'You have not added a work log for today.'
      );
    }
  }

  checkMonthlySummaryReminder(settings: any): void {
    if (!settings.monthlySummaryReminderEnabled) {
      return;
    }

    const today = new Date();

    const nextMonth = new Date(
      today.getFullYear(),
      today.getMonth() + 1,
      1
    );

    const differenceInMilliseconds =
      nextMonth.getTime() - today.getTime();

    const daysUntilNextMonth = Math.ceil(
      differenceInMilliseconds / (1000 * 60 * 60 * 24)
    );

    if (daysUntilNextMonth <= settings.monthlyReminderDaysBefore) {
      this.notificationMessages.push(
        'Your monthly work summary is ready for review.'
      );
    }
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

  openNotificationSettings(): void {
    const dialogRef = this.dialog.open(NotificationSettingsDialog, {
      width: '560px',
      autoFocus: false
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadDashboardNotifications();
      }
    });
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

  toggleNotificationPanel(): void {
    this.showNotificationPanel = !this.showNotificationPanel;
  }

  closeNotificationPanel(): void {
    this.showNotificationPanel = false;
  }

  clearNotifications(): void {
    this.notificationMessages = [];
    this.notificationCount = 0;
    this.showNotificationPanel = false;
  }

  openNotificationSettingsFromPanel(): void {
    this.showNotificationPanel = false;
    this.openNotificationSettings();
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
