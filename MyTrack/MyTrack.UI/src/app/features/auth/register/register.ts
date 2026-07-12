import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register {
  firstName = '';
  lastName = '';
  email = '';
  role = '';
  contactNumber = '';
  homeAddress = '';
  workAddress = '';
  password = '';

  errorMessage = '';

  confirmPassword = '';

  isSubmitting = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  register(): void {
    this.errorMessage = '';

    if (!this.firstName.trim() ||
      !this.lastName.trim() ||
      !this.email.trim() ||
      !this.password ||
      !this.confirmPassword) {
      this.showError('Please complete all required fields.');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.showError('Password and confirm password do not match.');
      return;
    }

    const passwordPattern =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

    if (!passwordPattern.test(this.password)) {
      this.showError(
        'Password must be at least 8 characters and include uppercase, lowercase, a number, and a special character.'
      );
      return;
    }

    this.isSubmitting = true;

    this.authService.register({
      firstName: this.firstName.trim(),
      lastName: this.lastName.trim(),
      email: this.email.trim(),
      role: this.role.trim(),
      contactNumber: this.contactNumber.trim(),
      homeAddress: this.homeAddress.trim(),
      workAddress: this.workAddress.trim(),
      password: this.password
    }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.authService.saveToken(response.token);
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isSubmitting = false;

        console.error('Registration error:', error);

        const message =
          error?.error?.message ||
          error?.error?.title ||
          'Unable to register. Please check your details.';

        this.showError(message);
      }
    });
  }

  private showError(message: string): void {
    this.errorMessage = message;

    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }
}
