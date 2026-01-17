import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DiscountService, Discount, AddDiscountRequest } from '../../../core/services/discount.service';
import { ProductService } from '../../../core/services/product.service';
import { Product } from '../../../core/models';

@Component({
    selector: 'app-admin-discounts',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './admin-discounts.component.html',
    styleUrls: ['./admin-discounts.component.css']
})
export class AdminDiscountsComponent implements OnInit {
    discountService = inject(DiscountService);
    productService = inject(ProductService);

    discounts = signal<Discount[]>([]);
    products = signal<Product[]>([]);

    showForm = signal(false);
    isEditing = signal(false);
    editingId = signal<number | null>(null);

    // Form Model
    formModel = {
        code: '',
        discountType: 'Percent', // 'Percent' | 'Fixed'
        value: 0,
        minimumOrderAmount: 0,
        startDate: '',
        endDate: '',
        appliesToProductId: null as number | null,
        usageLimit: 0,
        isActive: true
    };

    ngOnInit() {
        this.loadDiscounts();
        this.loadProducts();
    }

    loadDiscounts() {
        this.discountService.getAllDiscounts().subscribe(data => {
            this.discounts.set(data);
        });
    }

    loadProducts() {
        this.productService.getAllProducts().subscribe(data => {
            this.products.set(data);
        });
    }

    openCreateForm() {
        this.resetForm();
        // Default dates
        const now = new Date();
        const tomorrow = new Date(now);
        tomorrow.setDate(tomorrow.getDate() + 7);

        this.formModel.startDate = now.toISOString().slice(0, 16);
        this.formModel.endDate = tomorrow.toISOString().slice(0, 16);

        this.showForm.set(true);
        this.isEditing.set(false);
    }

    editDiscount(discount: Discount) {
        this.formModel = {
            code: discount.code || '',
            discountType: discount.discountType || 'Percent',
            value: discount.value,
            minimumOrderAmount: discount.minimumOrderAmount,
            startDate: discount.startDate ? discount.startDate.slice(0, 16) : '',
            endDate: discount.endDate ? discount.endDate.slice(0, 16) : '',
            appliesToProductId: discount.appliesToProductId || null,
            usageLimit: discount.usageLimit,
            isActive: discount.isActive
        };
        this.editingId.set(discount.id);
        this.isEditing.set(true);
        this.showForm.set(true);
    }

    deleteDiscount(id: number) {
        if (confirm('Bạn có chắc chắn muốn xóa mã này?')) {
            this.discountService.deleteDiscount(id).subscribe(() => {
                this.loadDiscounts();
            });
        }
    }

    onSubmit() {
        const request: AddDiscountRequest = {
            ...this.formModel,
            startDate: new Date(this.formModel.startDate).toISOString(),
            endDate: new Date(this.formModel.endDate).toISOString()
        };

        // Validate if Flash Sale
        if (this.formModel.appliesToProductId) {
            // If applies to product, code is optional (auto apply), but backend might require it or let it be generic
            // Usually Flash Sales don't need code, so we can set a dummy code or allow empty if backend supports
            if (!request.code) request.code = 'FLASH_' + this.formModel.appliesToProductId + '_' + Date.now().toString().slice(-4);
        }

        if (this.isEditing()) {
            const id = this.editingId();
            if (id) {
                this.discountService.updateDiscount(id, request).subscribe(() => {
                    this.loadDiscounts();
                    this.showForm.set(false);
                });
            }
        } else {
            this.discountService.createDiscount(request).subscribe(() => {
                this.loadDiscounts();
                this.showForm.set(false);
            });
        }
    }

    cancelForm() {
        this.showForm.set(false);
    }

    resetForm() {
        this.formModel = {
            code: '',
            discountType: 'Percent',
            value: 0,
            minimumOrderAmount: 0,
            startDate: '',
            endDate: '',
            appliesToProductId: null,
            usageLimit: 0,
            isActive: true
        };
    }

    getProductName(id: number | null | undefined): string {
        if (!id) return 'Tất cả đơn hàng';
        const p = this.products().find(x => x.id === id);
        return p ? p.name : 'Unknown Product';
    }
}
