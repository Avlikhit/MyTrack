import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { RegisterUserRequest } from '../models/register-user-request.model';

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
