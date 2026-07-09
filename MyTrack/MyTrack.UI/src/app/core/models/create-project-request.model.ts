export interface CreateProjectRequest {
  name: string;
  description: string;
  colorCode: string;
  displayOrder: number;
  isDefault: boolean;
  hourlyRate: number;
}
