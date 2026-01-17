import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AuthService } from '../../core/services/auth.service';

interface Order {
  id: number;
  orderDate: Date;
  totalAmount: number;
  orderStatus: string;  // Backend returns 'orderStatus' not 'status'
  recipientName?: string;
  orderItems?: OrderItem[];
}

interface OrderItem {
  productId: number;
  productName: string;
  quantity: number;
  price: number;  // Backend returns 'price' not 'unitPrice'
}

@Component({
  selector: 'app-user-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './user-orders.component.html',
  styleUrls: ['./user-orders.component.css']
})
export class UserOrdersComponent implements OnInit {
  private http = inject(HttpClient);
  private authService = inject(AuthService);
  private baseUrl = `${environment.apiUrl}/Orders`;

  orders = signal<Order[]>([]);
  loading = signal(true);

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    const userId = this.authService.getUserId();
    if (!userId) return;

    this.loading.set(true);
    this.http.get<Order[]>(`${this.baseUrl}/user/${userId}`).subscribe({
      next: (orders) => {
        this.orders.set(orders);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  getStatusLabel(status: string): string {
    const labels: { [key: string]: string } = {
      'Pending': 'Chờ xử lý',
      'Processing': 'Đang xử lý',
      'Shipped': 'Đang giao',
      'Delivered': 'Đã giao',
      'Cancelled': 'Đã hủy'
    };
    return labels[status] || status;
  }

  isStepActive(orderStatus: string, step: string): boolean {
    const statuses = ['pending', 'processing', 'shipped', 'delivered'];
    return orderStatus.toLowerCase() === step;
  }

  isStepDone(orderStatus: string, step: string): boolean {
    const statuses = ['pending', 'processing', 'shipped', 'delivered'];
    const orderIndex = statuses.indexOf(orderStatus.toLowerCase());
    const stepIndex = statuses.indexOf(step);
    return orderIndex > stepIndex;
  }

  toggleDetails(orderId: number) {
    // Future: expand order details
    console.log('Show details for order', orderId);
  }
}
