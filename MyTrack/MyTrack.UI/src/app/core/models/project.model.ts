export interface Project {
  id: number;
  name: string;
  description: string;
  colorCode: string;
  displayOrder: number;
  isDefault: boolean;
  isActive: boolean;
  createdDateTime: string;
  modifiedDateTime?: string;
  hourlyRate: number;
}
