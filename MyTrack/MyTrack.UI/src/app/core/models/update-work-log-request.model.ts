export interface UpdateWorkLogRequest {
  workDate: string;
  projectId: number;
  ticketNumber: string;
  taskType: string;
  description: string;
  hoursWorked: number;
  blockers: string;
  learnings: string;
  nextSteps: string;
}
