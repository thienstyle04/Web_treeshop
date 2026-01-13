import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartService = inject(CartService);
  private authService = inject(AuthService);

  shippingFee = 30000;

  get finalTotal(): number {
    const subtotal = this.cartService.cartTotal();
    if (subtotal >= 500000) {
      return subtotal;
    }
    return subtotal + this.shippingFee;
  }

  ngOnInit() {
    this.cartService.loadCart();
  }

  updateQuantity(itemId: number, quantity: number) {
    if (quantity <= 0) {
      this.removeItem(itemId);
    } else {
      this.cartService.updateQuantity(itemId, quantity).subscribe();
    }
  }

  removeItem(itemId: number) {
    this.cartService.removeFromCart(itemId).subscribe();
  }
}
