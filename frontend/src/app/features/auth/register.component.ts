import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  fullName = '';
  email = '';
  phone = '';
  password = '';
  confirmPassword = '';
  showPassword = false;
  agreeTerms = false;
  loading = signal(false);
  error = signal('');
  success = signal('');

  onSubmit() {
    this.error.set('');
    this.success.set('');

    if (!this.fullName || !this.email || !this.password || !this.confirmPassword) {
      this.error.set('Vui lòng nhập đầy đủ thông tin');
      return;
    }

    if (this.password.length < 6) {
      this.error.set('Mật khẩu phải có ít nhất 6 ký tự');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error.set('Mật khẩu xác nhận không khớp');
      return;
    }

    if (!this.agreeTerms) {
      this.error.set('Vui lòng đồng ý với điều khoản dịch vụ');
      return;
    }

    this.loading.set(true);

    this.authService.register({
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      phone: this.phone
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.success.set('Đăng ký thành công! Đang chuyển đến trang đăng nhập...');
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(err.error?.message || 'Đăng ký thất bại. Vui lòng thử lại.');
      }
    });
  }
}
