import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';
import { DiscountService, Discount } from '../../core/services/discount.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartService = inject(CartService);
  private authService = inject(AuthService);
  private discountService = inject(DiscountService);

  shippingFee = 30000;

  // Discount
  availableDiscounts = signal<Discount[]>([]);
  appliedDiscount = signal<Discount | null>(null);
  showDiscountModal = signal(false);

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get discountAmount(): number {
    const subtotal = this.cartService.cartTotal();
    const discount = this.appliedDiscount();
    if (!discount) return 0;

    if (discount.discountType === 'Percent') {
      return subtotal * (discount.value / 100);
    } else {
      return discount.value;
    }
  }

  get finalTotal(): number {
    const subtotal = this.cartService.cartTotal();
    let total = subtotal - this.discountAmount;
    if (total < 0) total = 0;

    if (subtotal >= 500000) {
      return total;
    }
    return total + this.shippingFee;
  }

  ngOnInit() {
    if (this.isLoggedIn) {
      this.cartService.loadCart();
      this.loadAvailableDiscounts();
    }
  }

  loadAvailableDiscounts() {
    this.discountService.getAllDiscounts().subscribe({
      next: (discounts) => {
        // Filter: chỉ lấy mã đang active, không phải Flash Sale (AppliesToProductId = null), và còn trong thời hạn
        const now = new Date();
        const validDiscounts = discounts.filter(d => {
          const startDate = new Date(d.startDate);
          const endDate = new Date(d.endDate);
          return d.isActive &&
            !d.appliesToProductId &&
            startDate <= now &&
            endDate >= now &&
            (d.usageLimit === 0 || d.usedCount < d.usageLimit);
        });
        this.availableDiscounts.set(validDiscounts);
      }
    });
  }

  openDiscountModal() {
    this.showDiscountModal.set(true);
  }

  closeDiscountModal() {
    this.showDiscountModal.set(false);
  }

  selectDiscount(discount: Discount) {
    // Validate minimum order
    if (this.cartService.cartTotal() < discount.minimumOrderAmount) {
      alert(`Đơn hàng tối thiểu để dùng mã này là ${discount.minimumOrderAmount.toLocaleString()}đ`);
      return;
    }
    this.appliedDiscount.set(discount);
    this.closeDiscountModal();
  }

  removeDiscount() {
    this.appliedDiscount.set(null);
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

