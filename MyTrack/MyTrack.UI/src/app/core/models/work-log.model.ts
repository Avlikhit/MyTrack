export interface WorkLog {
  id: number;
  workDate: string;
  projectId: number;
  projectName: string;
  ticketNumber: string;
  taskType: string;
  description: string;
  hoursWorked: number;
  blockers: string;
  learnings: string;
  nextSteps: string;
  createdDateTime: string;
  modifiedDateTime?: string;
}
