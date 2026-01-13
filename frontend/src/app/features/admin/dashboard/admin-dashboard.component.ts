import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../../core/services/product.service';
import { CategoryService } from '../../../core/services/category.service';
import { OrderService } from '../../../core/services/order.service';
import { Order } from '../../../core/models';

@Component({
    selector: 'app-admin-dashboard',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
    private productService = inject(ProductService);
    private categoryService = inject(CategoryService);
    private orderService = inject(OrderService);

    totalProducts = signal(0);
    totalCategories = signal(0);
    totalOrders = signal(0);
    totalRevenue = signal(0);
    recentProducts = signal<any[]>([]);

    ngOnInit() {
        this.loadStats();
    }

    loadStats() {
        // Load products
        this.productService.getAllProducts().subscribe(products => {
            this.totalProducts.set(products.length);
            this.recentProducts.set(products.slice(0, 5));
        });

        // Load categories
        this.categoryService.getAllCategories().subscribe(categories => {
            this.totalCategories.set(categories.length);
        });

        // Load orders and calculate real revenue
        this.orderService.getAllOrders().subscribe({
            next: (orders: Order[]) => {
                this.totalOrders.set(orders.length);

                // Calculate real revenue from order totals
                const revenue = orders.reduce((sum: number, order: Order) => sum + (order.totalAmount || 0), 0);
                this.totalRevenue.set(revenue);
            },
            error: () => {
                // Fallback to 0 if API fails
                this.totalOrders.set(0);
                this.totalRevenue.set(0);
            }
        });
    }
}
