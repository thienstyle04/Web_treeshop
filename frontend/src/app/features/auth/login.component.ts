import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  username = '';
  password = '';
  showPassword = false;
  rememberMe = false;
  loading = signal(false);
  error = signal('');

  onSubmit() {
    if (!this.username || !this.password) {
      this.error.set('Vui lòng nhập đầy đủ thông tin');
      return;
    }

    this.loading.set(true);
    this.error.set('');

    this.authService.login({ name: this.username, password: this.password }).subscribe({
      next: (response) => {
        this.loading.set(false);

        // Redirect based on user role
        if (response.role === 'Admin') {
          this.router.navigate(['/admin']);
        } else {
          const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
          this.router.navigate([returnUrl]);
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(err.error?.message || 'Tên đăng nhập hoặc mật khẩu không chính xác');
      }
    });
  }
}
