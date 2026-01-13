import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  private authService = inject(AuthService);
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Users`;

  formData = {
    id: 0,
    name: '',
    fullName: '',
    description: '',
    role: ''
  };

  passwordData = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  saving = signal(false);
  successMessage = signal('');
  errorMessage = signal('');
  showPasswordModal = false;

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.formData.id = user.id;
      this.formData.name = user.name;
      this.formData.fullName = user.fullName || '';
      this.formData.role = user.role;

      // Load full profile from API
      this.http.get<any>(`${this.baseUrl}/${user.id}`).subscribe({
        next: (data) => {
          this.formData.description = data.description || '';
          this.formData.fullName = data.fullName || '';
        }
      });
    }
  }

  getInitials(): string {
    const name = this.formData.fullName || this.formData.name || '';
    return name.charAt(0).toUpperCase();
  }

  saveProfile() {
    this.saving.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    this.http.put(`${this.baseUrl}/${this.formData.id}`, {
      fullName: this.formData.fullName,
      description: this.formData.description
    }).subscribe({
      next: () => {
        this.saving.set(false);
        this.successMessage.set('Thông tin đã được cập nhật!');

        // Update local user info
        const user = this.authService.getCurrentUser();
        if (user) {
          user.fullName = this.formData.fullName;
          localStorage.setItem('user', JSON.stringify(user));
        }
      },
      error: (err) => {
        this.saving.set(false);
        this.errorMessage.set(err.error?.message || 'Có lỗi xảy ra, vui lòng thử lại');
      }
    });
  }

  changePassword() {
    if (this.passwordData.newPassword !== this.passwordData.confirmPassword) {
      this.errorMessage.set('Mật khẩu xác nhận không khớp');
      return;
    }

    this.http.put(`${this.baseUrl}/${this.formData.id}/password`, {
      currentPassword: this.passwordData.currentPassword,
      newPassword: this.passwordData.newPassword
    }).subscribe({
      next: () => {
        this.showPasswordModal = false;
        this.successMessage.set('Đổi mật khẩu thành công!');
        this.passwordData = { currentPassword: '', newPassword: '', confirmPassword: '' };
      },
      error: (err) => {
        this.errorMessage.set(err.error?.message || 'Mật khẩu hiện tại không đúng');
      }
    });
  }
}
