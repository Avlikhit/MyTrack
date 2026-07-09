import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PayrollSettings } from '../models/payroll-settings.model';
import { UpdatePayrollSettingsRequest } from '../models/update-payroll-settings-request.model';

@Injectable({
  providedIn: 'root'
})
export class PayrollSettingsService {
  private readonly apiUrl = 'https://localhost:7023/api/payroll-settings';

  constructor(private http: HttpClient) { }

  get(): Observable<PayrollSettings> {
    return this.http.get<PayrollSettings>(this.apiUrl);
  }

  update(request: UpdatePayrollSettingsRequest): Observable<PayrollSettings> {
    return this.http.put<PayrollSettings>(this.apiUrl, request);
  }
}
