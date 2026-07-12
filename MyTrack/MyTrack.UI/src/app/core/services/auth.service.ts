import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { RegisterUserRequest } from '../models/register-user-request.model';
import { UserProfile } from '../models/user-profile.model';
import { UpdateUserProfileRequest } from '../models/update-user-profile-request.model';
import { ChangePasswordRequest } from '../models/change-password-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:7023/api/auth';

  constructor(private http: HttpClient) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request);
  }

  register(request: RegisterUserRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/register`, request);
  }

  getProfile() {
    return this.http.get<UserProfile>(`${this.apiUrl}/profile`);
  }

  updateProfile(request: UpdateUserProfileRequest) {
    return this.http.put<UserProfile>(`${this.apiUrl}/profile`, request);
  }

  changePassword(request: ChangePasswordRequest) {
    return this.http.put<void>(`${this.apiUrl}/change-password`, request);
  }

  saveToken(token: string): void {
    localStorage.setItem('mytrack_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('mytrack_token');
  }

  logout(): void {
    localStorage.removeItem('mytrack_token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
