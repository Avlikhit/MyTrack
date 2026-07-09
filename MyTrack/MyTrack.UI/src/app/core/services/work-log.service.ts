import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { WorkLog } from '../models/work-log.model';
import { CreateWorkLogRequest } from '../models/create-work-log-request.model';
import { UpdateWorkLogRequest } from '../models/update-work-log-request.model';

@Injectable({
  providedIn: 'root'
})
export class WorkLogService {
  private readonly apiUrl = 'https://localhost:7023/api/worklogs';

  constructor(private http: HttpClient) { }

  getAll(): Observable<WorkLog[]> {
    return this.http.get<WorkLog[]>(this.apiUrl);
  }

  getById(id: number): Observable<WorkLog> {
    return this.http.get<WorkLog>(`${this.apiUrl}/${id}`);
  }

  getByDate(workDate: string): Observable<WorkLog[]> {
    return this.http.get<WorkLog[]>(`${this.apiUrl}/date/${workDate}`);
  }

  getByDateRange(startDate: string, endDate: string): Observable<WorkLog[]> {
    return this.http.get<WorkLog[]>(
      `${this.apiUrl}/range?startDate=${startDate}&endDate=${endDate}`
    );
  }

  create(request: CreateWorkLogRequest): Observable<WorkLog> {
    return this.http.post<WorkLog>(this.apiUrl, request);
  }

  update(id: number, request: UpdateWorkLogRequest): Observable<WorkLog> {
    return this.http.put<WorkLog>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
