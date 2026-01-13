import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../core/services/order.service';
import { Order } from '../../../core/models';

import { UserService } from '../../../core/services/user.service';

@Component({
    selector: 'app-admin-orders',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './admin-orders.component.html',
    styleUrls: ['./admin-orders.component.css']
})
export class AdminOrdersComponent implements OnInit {
    private orderService = inject(OrderService);
    private userService = inject(UserService);

    orders = signal<Order[]>([]);
    filteredOrders = signal<Order[]>([]);
    loading = signal(true);
    userMap = signal<Map<number, string>>(new Map());

    statusFilter = '';
    selectedOrder: Order | null = null;
    showDeleteModal = false;
    orderToDelete: Order | null = null;

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.loading.set(true);
        // Load users first or in parallel
        this.userService.getAllUsers().subscribe({
            next: (users) => {
                const map = new Map<number, string>();
                users.forEach(u => map.set(u.id, u.fullName || u.name));
                this.userMap.set(map);

                // Then load orders
                this.loadOrders();
            },
            error: () => {
                console.error('Error loading users');
                this.loadOrders(); // Attempt to load orders anyway
            }
        });
    }

    loadOrders() {
        this.loading.set(true);
        this.orderService.getAllOrders().subscribe({
            next: (orders) => {
                this.orders.set(orders);
                this.filteredOrders.set(orders);
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
            }
        });
    }

    filterOrders() {
        if (!this.statusFilter) {
            this.filteredOrders.set(this.orders());
        } else {
            this.filteredOrders.set(
                this.orders().filter(o => o.status === this.statusFilter)
            );
        }
    }

    getStatusLabel(status: string): string {
        if (!status) return 'Không xác định';
        const labels: { [key: string]: string } = {
            'Pending': 'Chờ xử lý',
            'Processing': 'Đang xử lý',
            'Shipped': 'Đang giao',
            'Delivered': 'Đã giao',
            'Cancelled': 'Đã hủy'
        };
        return labels[status] || status;
    }

    getStatusClass(status: string): string {
        return (status || '').toLowerCase();
    }

    viewOrder(order: Order) {
        this.selectedOrder = order;
    }

    updateStatus(order: Order) {
        this.orderService.updateStatus(order.id, order.status).subscribe();
    }

    confirmDelete(order: Order) {
        this.orderToDelete = order;
        this.showDeleteModal = true;
    }

    deleteOrder() {
        if (!this.orderToDelete) return;

        this.orderService.deleteOrder(this.orderToDelete.id).subscribe({
            next: () => {
                this.showDeleteModal = false;
                this.orderToDelete = null;
                this.loadOrders();
            }
        });
    }
}
