import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  email = '';
  password = '';

  errorMessage = '';
  isSubmitting = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  login(): void {
    this.errorMessage = '';

    const email = this.email.trim();

    if (!email || !this.password) {
      this.errorMessage = 'Email and password are required.';
      return;
    }

    this.isSubmitting = true;

    this.authService.login({
      email,
      password: this.password
    }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.authService.saveToken(response.token);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        console.error('Login error:', error);
        console.error('Login error body:', error?.error);

        this.isSubmitting = false;
        this.errorMessage = this.getErrorMessage(error);

        this.cdr.detectChanges();
      }
    });
  }

  private getErrorMessage(error: any): string {
    if (typeof error?.error === 'string' && error.error.trim()) {
      return error.error;
    }

    if (error?.error?.message) {
      return error.error.message;
    }

    if (error?.error?.title) {
      return error.error.title;
    }

    if (error?.message) {
      return error.message;
    }

    return 'Unable to log in. Please try again.';
  }
}
