import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatButtonModule } from '@angular/material/button';
import {
  MatDialogModule,
  MatDialogRef
} from '@angular/material/dialog';

import { NotificationSettingsService } from '../../../core/services/notification-settings.service';

@Component({
  selector: 'app-notification-settings-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './notification-settings-dialog.html',
  styleUrl: './notification-settings-dialog.scss'
})
export class NotificationSettingsDialog implements OnInit {
  notificationsEnabled = true;
  dailyWorkLogReminderEnabled = true;
  dailyReminderTime = '18:00';
  monthlySummaryReminderEnabled = true;
  monthlyReminderDaysBefore = 1;

  isLoading = false;
  isSaving = false;
  errorMessage = '';

  constructor(
    private notificationSettingsService: NotificationSettingsService,
    private dialogRef: MatDialogRef<NotificationSettingsDialog>,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadSettings();
  }

  loadSettings(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.notificationSettingsService.get().subscribe({
      next: (response) => {
        this.notificationsEnabled = response.notificationsEnabled;
        this.dailyWorkLogReminderEnabled =
          response.dailyWorkLogReminderEnabled;

        this.dailyReminderTime =
          this.formatTimeForInput(response.dailyReminderTime);

        this.monthlySummaryReminderEnabled =
          response.monthlySummaryReminderEnabled;

        this.monthlyReminderDaysBefore =
          response.monthlyReminderDaysBefore;

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Unable to load notification settings', error);
        console.error('Notification settings error body:', error?.error);

        this.errorMessage =
          error?.error?.message ??
          error?.error?.title ??
          (typeof error?.error === 'string' ? error.error : '') ??
          'Unable to load notification settings.';

        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  save(): void {
    this.errorMessage = '';

    if (
      this.monthlyReminderDaysBefore < 0 ||
      this.monthlyReminderDaysBefore > 31
    ) {
      this.errorMessage =
        'Monthly reminder days before must be between 0 and 31.';
      return;
    }

    if (
      this.dailyWorkLogReminderEnabled &&
      !this.dailyReminderTime
    ) {
      this.errorMessage = 'Please select a daily reminder time.';
      return;
    }

    this.isSaving = true;

    const reminderTime = this.normalizeTime(
      this.dailyReminderTime
    );

    this.notificationSettingsService.update({
      notificationsEnabled: this.notificationsEnabled,
      dailyWorkLogReminderEnabled:
        this.dailyWorkLogReminderEnabled,
      dailyReminderTime: reminderTime,
      monthlySummaryReminderEnabled:
        this.monthlySummaryReminderEnabled,
      monthlyReminderDaysBefore:
        Number(this.monthlyReminderDaysBefore)
    }).subscribe({
      next: (response) => {
        this.notificationsEnabled =
          response.notificationsEnabled;

        this.dailyWorkLogReminderEnabled =
          response.dailyWorkLogReminderEnabled;

        this.dailyReminderTime =
          this.formatTimeForInput(response.dailyReminderTime);

        this.monthlySummaryReminderEnabled =
          response.monthlySummaryReminderEnabled;

        this.monthlyReminderDaysBefore =
          response.monthlyReminderDaysBefore;

        this.isSaving = false;
        this.dialogRef.close(true);
      },
      error: (error) => {
        console.error(
          'Unable to update notification settings',
          error
        );

        console.error(
          'Notification settings update error body:',
          error?.error
        );

        this.errorMessage =
          error?.error?.message ??
          error?.error?.title ??
          (typeof error?.error === 'string'
            ? error.error
            : '') ??
          'Unable to update notification settings.';

        this.isSaving = false;
        this.cdr.detectChanges();
      }
    });
  }

  cancel(): void {
    this.dialogRef.close(false);
  }

  private normalizeTime(value: string): string {
    if (!value) {
      return '18:00:00';
    }

    const parts = value.split(':');

    const hours = parts[0]?.padStart(2, '0') ?? '18';
    const minutes = parts[1]?.padStart(2, '0') ?? '00';
    const seconds = parts[2]?.padStart(2, '0') ?? '00';

    return `${hours}:${minutes}:${seconds}`;
  }

  private formatTimeForInput(value: string): string {
    if (!value) {
      return '18:00';
    }

    return value.substring(0, 5);
  }
}
