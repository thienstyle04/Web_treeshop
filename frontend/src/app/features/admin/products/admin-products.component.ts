import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';
import { CategoryService } from '../../../core/services/category.service';
import { Product, Category } from '../../../core/models';

@Component({
    selector: 'app-admin-products',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './admin-products.component.html',
    styleUrls: ['./admin-products.component.css']
})
export class AdminProductsComponent implements OnInit {
    private productService = inject(ProductService);
    private categoryService = inject(CategoryService);

    products = signal<Product[]>([]);
    filteredProducts = signal<Product[]>([]);
    categories = signal<Category[]>([]);
    loading = signal(true);
    saving = signal(false);

    searchTerm = '';
    selectedCategoryId: number | null = null;

    showAddModal = false;
    showEditModal = false;
    showDeleteModal = false;

    formData = {
        id: 0,
        name: '',
        scientificName: '',
        description: '',
        price: 0,
        stockQuantity: 0,
        categoryId: null as number | null,
        imageUrl: ''
    };

    productToDelete: Product | null = null;

    ngOnInit() {
        this.loadProducts();
        this.loadCategories();
    }

    loadProducts() {
        this.loading.set(true);
        this.productService.getAllProducts().subscribe({
            next: (products) => {
                this.products.set(products);
                this.filteredProducts.set(products);
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
            }
        });
    }

    loadCategories() {
        this.categoryService.getAllCategories().subscribe(categories => {
            this.categories.set(categories);
        });
    }

    filterProducts() {
        let filtered = [...this.products()];

        if (this.searchTerm) {
            const term = this.searchTerm.toLowerCase();
            filtered = filtered.filter(p =>
                p.name.toLowerCase().includes(term) ||
                p.scientificName?.toLowerCase().includes(term)
            );
        }

        if (this.selectedCategoryId) {
            filtered = filtered.filter(p => p.categoryId === +this.selectedCategoryId!);
        }

        this.filteredProducts.set(filtered);
    }

    editProduct(product: Product) {
        this.formData = {
            id: product.id,
            name: product.name,
            scientificName: product.scientificName || '',
            description: product.description || '',
            price: product.price,
            stockQuantity: product.stockQuantity,
            categoryId: product.categoryId || null,
            imageUrl: product.imageUrls?.[0] || ''
        };
        this.showEditModal = true;
    }

    confirmDelete(product: Product) {
        this.productToDelete = product;
        this.showDeleteModal = true;
    }

    closeModal() {
        this.showAddModal = false;
        this.showEditModal = false;
        this.resetForm();
    }

    resetForm() {
        this.formData = {
            id: 0,
            name: '',
            scientificName: '',
            description: '',
            price: 0,
            stockQuantity: 0,
            categoryId: null,
            imageUrl: ''
        };
    }

    saveProduct() {
        if (!this.formData.name || !this.formData.price || !this.formData.categoryId) {
            return;
        }

        this.saving.set(true);

        const productData = {
            name: this.formData.name,
            scientificName: this.formData.scientificName,
            description: this.formData.description,
            price: this.formData.price,
            stockQuantity: this.formData.stockQuantity,
            categoryId: this.formData.categoryId,
            imageUrl: this.formData.imageUrl
        };

        if (this.showEditModal) {
            this.productService.updateProduct(this.formData.id, productData).subscribe({
                next: () => {
                    this.saving.set(false);
                    this.closeModal();
                    this.loadProducts();
                },
                error: () => {
                    this.saving.set(false);
                }
            });
        } else {
            this.productService.addProduct(productData).subscribe({
                next: () => {
                    this.saving.set(false);
                    this.closeModal();
                    this.loadProducts();
                },
                error: () => {
                    this.saving.set(false);
                }
            });
        }
    }

    deleteProduct() {
        if (!this.productToDelete) return;

        this.productService.deleteProduct(this.productToDelete.id).subscribe({
            next: () => {
                this.showDeleteModal = false;
                this.productToDelete = null;
                this.loadProducts();
            }
        });
    }
}
