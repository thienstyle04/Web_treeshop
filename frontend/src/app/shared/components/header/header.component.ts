import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  authService = inject(AuthService);
  cartService = inject(CartService);

  showSearch = false;
  showUserMenu = false;
  showMobileMenu = false;

  toggleSearch() {
    this.showSearch = !this.showSearch;
    this.showUserMenu = false;
  }

  toggleUserMenu() {
    this.showUserMenu = !this.showUserMenu;
  }

  toggleMobileMenu() {
    this.showMobileMenu = !this.showMobileMenu;
  }

  closeMobileMenu() {
    this.showMobileMenu = false;
  }

  search(query: string) {
    if (query.trim()) {
      // Navigate to products with search query
      console.log('Searching for:', query);
    }
  }

  logout() {
    this.authService.logout();
    this.showUserMenu = false;
  }
}
