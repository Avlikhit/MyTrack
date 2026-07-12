export interface NotificationSettings {
  id: number;
  notificationsEnabled: boolean;
  dailyWorkLogReminderEnabled: boolean;
  dailyReminderTime: string;
  monthlySummaryReminderEnabled: boolean;
  monthlyReminderDaysBefore: number;
}
