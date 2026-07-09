import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PayInformation } from '../models/pay-information.model';
import { CreatePayInformationRequest } from '../models/create-pay-information-request.model';
import { UpdatePayInformationRequest } from '../models/update-pay-information-request.model';

@Injectable({
  providedIn: 'root'
})
export class PayInformationService {
  private readonly apiUrl = 'https://localhost:7023/api/pay-information';

  constructor(private http: HttpClient) { }

  getAll(): Observable<PayInformation[]> {
    return this.http.get<PayInformation[]>(this.apiUrl);
  }

  getById(id: number): Observable<PayInformation> {
    return this.http.get<PayInformation>(`${this.apiUrl}/${id}`);
  }

  getByDateRange(startDate: string, endDate: string): Observable<PayInformation[]> {
    return this.http.get<PayInformation[]>(
      `${this.apiUrl}/range?startDate=${startDate}&endDate=${endDate}`
    );
  }

  create(request: CreatePayInformationRequest): Observable<PayInformation> {
    return this.http.post<PayInformation>(this.apiUrl, request);
  }

  update(id: number, request: UpdatePayInformationRequest): Observable<PayInformation> {
    return this.http.put<PayInformation>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
