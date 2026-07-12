export interface UpdateNotificationSettingsRequest {
  notificationsEnabled: boolean;
  dailyWorkLogReminderEnabled: boolean;
  dailyReminderTime: string;
  monthlySummaryReminderEnabled: boolean;
  monthlyReminderDaysBefore: number;
}
