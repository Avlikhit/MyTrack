import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { NotificationSettings } from '../models/notification-settings.model';
import { UpdateNotificationSettingsRequest } from '../models/update-notification-settings-request.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationSettingsService {
  private readonly apiUrl =
    'https://localhost:7023/api/notification-settings';

  constructor(private http: HttpClient) { }

  get(): Observable<NotificationSettings> {
    return this.http.get<NotificationSettings>(this.apiUrl);
  }

  update(
    request: UpdateNotificationSettingsRequest
  ): Observable<NotificationSettings> {
    return this.http.put<NotificationSettings>(
      this.apiUrl,
      request
    );
  }
}
