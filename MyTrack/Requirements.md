# MyTrack - Functional Requirements

## Document Information

| Item | Value |
|------|-------|
| Project | MyTrack |
| Document | Functional Requirements |
| Version | 1.0 |
| Status | In Progress |

---

# Purpose

This document defines the functional requirements for the MyTrack application.

All application features, database design, APIs, services, frontend pages, and business rules will be implemented based on the requirements documented here.

This document serves as the single source of truth for the project.

---

# Application Overview

MyTrack is a personal enterprise productivity application that allows users to:

- Track daily work activities
- Maintain personal work history
- Calculate payroll information
- Generate automatic summaries
- Receive reminders and notifications
- Learn enterprise software development using modern .NET technologies

---

# Application Flow

```
Run Application
        │
        ▼
   Login Page
        │
        ▼
Summary Dashboard
        │
        ▼
Navigation Menu
    ├── Work Log
    ├── Pay Details Log
    ├── Profile Log
    └── Notification Settings
```

---

# Functional Requirements

## FR-001 - Login

### Description

The application shall require users to authenticate before accessing any feature.

### Requirements

- Display Login page when the application starts.
- Redirect successful login to the Summary Dashboard.
- Prevent unauthorized access to application modules.

---

## FR-002 - Summary Dashboard

### Description

The Summary Dashboard is displayed immediately after a successful login.

### Requirements

Display:

- Current week start date
- Current week end date
- Total hours worked during the current week
- Automatically generated weekly work summary using daily work notes
- Recent work log entries
- Reminder if today's work log has not been completed

### Notes

- Payroll information shall **NOT** be displayed on this page.

---

## FR-003 - Work Log

### Description

Allows users to record and manage daily work activities.

### Requirements

- Default selected date is today's date.
- User can select any date using a calendar.
- Existing work logs can be edited.
- Daily work entries are saved only when the Save button is selected.
- No automatic saving.

Each work log shall contain:

- Project
- Work Date
- Ticket Number
- Task Type
- Description
- Hours Worked
- Blockers
- Learnings
- Next Steps

---

## FR-004 - Pay Details Log

### Description

Allows users to calculate payroll information.

### Requirements

- Default pay rate is loaded from the user's profile.
- Hours are automatically loaded from saved Work Logs.
- Manual hour override is allowed.
- Manual hour override requires a mandatory reason.
- Manual override reason shall be saved.
- Calculate:
  - Gross Pay
  - Estimated Taxes
  - Estimated Take Home Pay
- Payroll information is saved only when the Save button is selected.

---

## FR-005 - Profile Log

### Description

Stores user profile information.

### Requirements

Maintain:

- Personal Information
- Job Information
- Default Hourly Pay Rate
- Tax Configuration
- Notification Preferences

---

## FR-006 - Notification Settings

### Description

Allows users to configure application notifications.

### Requirements

Users can:

- Enable or disable all notifications.
- Enable or disable daily work reminders.
- Enable or disable monthly payroll reminders.
- Configure reminder preferences (future enhancement).

---

## FR-007 - Notifications

### Description

The application shall notify users based on configured preferences.

### Requirements

Display a notification:

- After successfully saving a form.
- If today's work log has not been completed.
- Before the first day of every month informing the user of the estimated gross monthly pay.

Example:

> Estimated gross pay for this month is $XXXX. Please manage your expenses accordingly.

---
## FR-008 - Privacy and Security

### Requirements

- The application shall require user authentication.
- Passwords shall be securely hashed and never stored in plain text.
- Sensitive configuration values shall not be stored in source control.
- All communication shall use HTTPS.
- Data shall be stored locally by default.
- AI-powered features shall be optional and user-controlled.
- Future versions may encrypt sensitive financial information.

# Business Rules

## BR-001

Work Logs are the source of truth for worked hours.

---

## BR-002

Payroll calculations shall use hours from Work Logs by default.

---

## BR-003

Manual hour overrides require a mandatory reason.

---

## BR-004

No data shall be saved until the user explicitly selects the Save button.

---

## BR-005

Summary Dashboard shall not display payroll information.

---

## Future Enhancements

Planned features include:

- AI-generated daily summaries
- AI-generated weekly summaries
- AI-generated monthly summaries
- AI-powered productivity insights
- PDF export
- Excel export
- Calendar integration
- Email notifications
- Mobile responsive interface
- Dark mode
- Multiple jobs
- Multiple payroll profiles
- Dashboard analytics
- Goal tracking
- Learning journal
- File attachments

---
# AI Principle:

MyTrack will use a local AI model by default for all AI-powered features. User data will remain on the local machine and will not be sent to external AI services unless the user explicitly enables a future cloud AI integration.

# Revision History

| Version | Date | Description |
|----------|------|-------------|
| 1.0 | Initial | Initial project requirements |