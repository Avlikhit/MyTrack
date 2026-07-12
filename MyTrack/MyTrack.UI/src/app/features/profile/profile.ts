import { CommonModule } from '@angular/common';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  firstName = '';
  lastName = '';
  email = '';
  role = '';
  contactNumber = '';
  homeAddress = '';
  workAddress = '';

  currentPassword = '';
  newPassword = '';
  confirmPassword = '';

  isLoading = false;
  errorMessage = '';
  successMessage = '';
  passwordErrorMessage = '';
  passwordSuccessMessage = '';

  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;

    this.authService.getProfile().subscribe({
      next: (response) => {
        this.firstName = response.firstName;
        this.lastName = response.lastName;
        this.email = response.email;
        this.role = response.role;
        this.contactNumber = response.contactNumber;
        this.homeAddress = response.homeAddress;
        this.workAddress = response.workAddress;

        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Unable to load profile.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  saveProfile(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.firstName.trim() || !this.lastName.trim()) {
      this.errorMessage = 'First name and last name are required.';
      return;
    }

    const phonePattern = /^[0-9+\-()\s]{7,20}$/;

    if (this.contactNumber.trim() &&
      !phonePattern.test(this.contactNumber.trim())) {
      this.errorMessage = 'Please enter a valid contact number.';
      return;
    }

    this.authService.updateProfile({
      firstName: this.firstName.trim(),
      lastName: this.lastName.trim(),
      role: this.role.trim(),
      contactNumber: this.contactNumber.trim(),
      homeAddress: this.homeAddress.trim(),
      workAddress: this.workAddress.trim()
    }).subscribe({
      next: () => {
        this.successMessage = 'Profile updated successfully.';
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage =
          error?.error?.message ??
          'Unable to update profile.';

        this.cdr.detectChanges();
      }
    });
  }

  changePassword(): void {
    this.passwordErrorMessage = '';
    this.passwordSuccessMessage = '';

    if (!this.currentPassword ||
      !this.newPassword ||
      !this.confirmPassword) {
      this.passwordErrorMessage =
        'Please complete all password fields.';
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.passwordErrorMessage =
        'New password and confirm password do not match.';
      return;
    }

    if (this.currentPassword === this.newPassword) {
      this.passwordErrorMessage =
        'New password must be different from the current password.';
      return;
    }

    const passwordPattern =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

    if (!passwordPattern.test(this.newPassword)) {
      this.passwordErrorMessage =
        'Password must be at least 8 characters and include uppercase, lowercase, number, and special character.';
      return;
    }

    this.authService.changePassword({
      currentPassword: this.currentPassword,
      newPassword: this.newPassword,
      confirmPassword: this.confirmPassword
    }).subscribe({
      next: () => {
        this.passwordSuccessMessage =
          'Password changed successfully.';

        this.currentPassword = '';
        this.newPassword = '';
        this.confirmPassword = '';

        this.cdr.detectChanges();
      },
      error: (error) => {
        this.passwordErrorMessage =
          error?.error?.message ??
          'Unable to change password.';

        this.cdr.detectChanges();
      }
    });
  }
}
