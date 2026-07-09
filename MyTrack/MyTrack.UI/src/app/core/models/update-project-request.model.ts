export interface UpdateProjectRequest {
  name: string;
  description: string;
  colorCode: string;
  displayOrder: number;
  isDefault: boolean;
  isActive: boolean;
  hourlyRate: number;
}
