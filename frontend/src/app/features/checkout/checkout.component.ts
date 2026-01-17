import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';
import { ProvincesService, Province, District, Ward } from '../../core/services/provinces.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

import { DiscountService, Discount } from '../../core/services/discount.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  cartService = inject(CartService);
  private authService = inject(AuthService);
  private provincesService = inject(ProvincesService);
  private discountService = inject(DiscountService);
  private router = inject(Router);
  private http = inject(HttpClient);

  shippingFee = 30000;

  shippingInfo = {
    fullName: '',
    phone: '',
    address: '',
    note: ''
  };

  // Location selection
  provinces = signal<Province[]>([]);
  districts = signal<District[]>([]);
  wards = signal<Ward[]>([]);

  selectedProvinceCode = '';
  selectedDistrictCode = '';
  selectedWardCode = '';

  orderPlacing = signal(false);
  orderSuccess = signal(false);
  errorMessage = signal('');
  orderId = 0;

  // Discount Logic
  discountCode = '';
  appliedDiscount = signal<Discount | null>(null);
  discountError = signal('');

  get finalTotal(): number {
    const subtotal = this.cartService.cartTotal();
    let total = subtotal;

    // Apply discount
    const discount = this.appliedDiscount();
    if (discount) {
      if (discount.discountType === 'Percent') {
        total = total * (1 - discount.value / 100);
      } else {
        total = total - discount.value;
      }
    }

    if (total < 0) total = 0;

    // Shipping fee logic
    if (subtotal >= 500000) {
      return total;
    }
    return total + this.shippingFee;
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

  applyDiscount() {
    if (!this.discountCode.trim()) return;

    this.discountError.set('');
    this.discountService.checkCode(this.discountCode).subscribe({
      next: (discount) => {
        // Validate minimum order
        if (this.cartService.cartTotal() < discount.minimumOrderAmount) {
          this.discountError.set(`Đơn hàng tối thiểu để dùng mã này là ${discount.minimumOrderAmount.toLocaleString()}đ`);
          this.appliedDiscount.set(null);
          return;
        }
        this.appliedDiscount.set(discount);
      },
      error: (err) => {
        this.discountError.set(err.error || 'Mã giảm giá không hợp lệ');
        this.appliedDiscount.set(null);
      }
    });
  }

  removeDiscount() {
    this.appliedDiscount.set(null);
    this.discountCode = '';
    this.discountError.set('');
  }

  ngOnInit() {
    // Load user info
    const user = this.authService.getCurrentUser();
    if (user) {
      this.shippingInfo.fullName = user.fullName || '';
    }

    // Load provinces
    this.loadProvinces();
  }

  loadProvinces() {
    this.provincesService.getProvinces().subscribe({
      next: (data) => {
        this.provinces.set(data);
      },
      error: (err) => {
        console.error('Error loading provinces', err);
      }
    });
  }

  onProvinceChange() {
    this.districts.set([]);
    this.wards.set([]);
    this.selectedDistrictCode = '';
    this.selectedWardCode = '';

    if (this.selectedProvinceCode) {
      this.provincesService.getProvinceWithDistricts(+this.selectedProvinceCode).subscribe({
        next: (data) => {
          this.districts.set(data.districts || []);
        }
      });
    }
  }

  onDistrictChange() {
    this.wards.set([]);
    this.selectedWardCode = '';

    if (this.selectedDistrictCode) {
      this.provincesService.getDistrictWithWards(+this.selectedDistrictCode).subscribe({
        next: (data) => {
          this.wards.set(data.wards || []);
        }
      });
    }
  }

  getSelectedLocationNames(): { province: string, district: string, ward: string } {
    const province = this.provinces().find(p => p.code === +this.selectedProvinceCode);
    const district = this.districts().find(d => d.code === +this.selectedDistrictCode);
    const ward = this.wards().find(w => w.code === +this.selectedWardCode);

    return {
      province: province?.name || '',
      district: district?.name || '',
      ward: ward?.name || ''
    };
  }

  placeOrder() {
    const location = this.getSelectedLocationNames();

    // Validate
    if (!this.shippingInfo.fullName || !this.shippingInfo.phone ||
      !this.shippingInfo.address || !location.province || !location.district) {
      this.errorMessage.set('Vui lòng điền đầy đủ thông tin giao hàng');
      return;
    }

    if (this.cartService.cartItems().length === 0) {
      this.errorMessage.set('Giỏ hàng trống');
      return;
    }

    this.orderPlacing.set(true);
    this.errorMessage.set('');

    const userId = this.authService.getUserId();
    const fullAddress = `${this.shippingInfo.address}${location.ward ? ', ' + location.ward : ''}, ${location.district}, ${location.province}`;

    const orderData = {
      userId: this.authService.getUserId(),
      shippingAddressId: null, // Logic chọn address có sẵn chưa implement, hiện tại dùng inline
      recipientName: this.shippingInfo.fullName,
      phone: this.shippingInfo.phone,
      streetAddress: this.shippingInfo.address,
      city: this.selectedProvinceCode, // Lưu ý: Nên lưu tên Tỉnh/TP thay vì code nếu backend cần display text
      discountCodeUsed: this.discountCode || null,
      discountAmount: this.discountAmount,
      items: this.cartService.cartItems().map(item => ({
        productId: item.productId,
        quantity: item.quantity
      }))
    };

    this.http.post<any>(`${environment.apiUrl}/Orders`, orderData).subscribe({
      next: (response) => {
        this.orderId = response.id || response.orderId || 1;
        this.orderPlacing.set(false);
        this.orderSuccess.set(true);

        // Clear cart
        this.cartService.cartItems().forEach(item => {
          this.cartService.removeFromCart(item.id).subscribe();
        });
      },
      error: (err) => {
        this.orderPlacing.set(false);
        this.errorMessage.set(err.error?.message || 'Có lỗi xảy ra, vui lòng thử lại');
      }
    });
  }
}
