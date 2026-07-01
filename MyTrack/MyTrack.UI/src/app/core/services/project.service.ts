import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Project } from '../models/project.model';
import { CreateProjectRequest } from '../models/create-project-request.model';
import { UpdateProjectRequest } from '../models/update-project-request.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly apiUrl = 'https://localhost:7023/api/projects';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Project[]> {
    return this.http.get<Project[]>(this.apiUrl);
  }

  getActive(): Observable<Project[]> {
    return this.http.get<Project[]>(`${this.apiUrl}/active`);
  }

  getById(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateProjectRequest): Observable<Project> {
    return this.http.post<Project>(this.apiUrl, request);
  }

  update(id: number, request: UpdateProjectRequest): Observable<Project> {
    return this.http.put<Project>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
