import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CategoryService } from '../../../core/services/category.service';
import { ProductService } from '../../../core/services/product.service';
import { Category, Product } from '../../../core/models';

@Component({
    selector: 'app-admin-categories',
    standalone: true,
    imports: [CommonModule, FormsModule, RouterModule],
    templateUrl: './admin-categories.component.html',
    styleUrls: ['./admin-categories.component.css']
})
export class AdminCategoriesComponent implements OnInit {
    private categoryService = inject(CategoryService);
    private productService = inject(ProductService);

    categories = signal<Category[]>([]);
    products = signal<Product[]>([]);
    categoryImages = signal<Map<number, string>>(new Map());
    loading = signal(true);

    showAddModal = false;
    showEditModal = false;
    showDeleteModal = false;

    formData = {
        id: 0,
        name: '',
        urlHandler: '',
        description: ''
    };

    categoryToDelete: Category | null = null;

    ngOnInit() {
        this.loadData();
    }

    loadData() {
        this.loading.set(true);

        // Load products first to get images
        this.productService.getAllProducts().subscribe({
            next: (products) => {
                this.products.set(products);

                // Build category image map - first product image for each category
                const imageMap = new Map<number, string>();
                products.forEach(p => {
                    if (p.categoryId && !imageMap.has(p.categoryId) && p.imageUrls?.length > 0) {
                        imageMap.set(p.categoryId, p.imageUrls[0]);
                    }
                });
                this.categoryImages.set(imageMap);

                // Then load categories
                this.loadCategories();
            },
            error: () => {
                this.loadCategories();
            }
        });
    }

    loadCategories() {
        this.categoryService.getAllCategories().subscribe({
            next: (categories) => {
                this.categories.set(categories);
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
            }
        });
    }

    getCategoryImage(categoryId: number): string {
        return this.categoryImages().get(categoryId) || 'assets/images/placeholder-plant.jpg';
    }

    editCategory(category: Category, event: Event) {
        event.preventDefault();
        event.stopPropagation();
        this.formData = {
            id: category.id,
            name: category.name,
            urlHandler: category.urlHandler || '',
            description: category.description || ''
        };
        this.showEditModal = true;
    }

    confirmDelete(category: Category, event: Event) {
        event.preventDefault();
        event.stopPropagation();
        this.categoryToDelete = category;
        this.showDeleteModal = true;
    }

    closeModal() {
        this.showAddModal = false;
        this.showEditModal = false;
        this.formData = { id: 0, name: '', urlHandler: '', description: '' };
    }

    saveCategory() {
        if (!this.formData.name) return;

        if (this.showEditModal) {
            this.categoryService.updateCategory(this.formData.id, this.formData).subscribe({
                next: () => {
                    this.closeModal();
                    this.loadCategories();
                }
            });
        } else {
            this.categoryService.addCategory(this.formData).subscribe({
                next: () => {
                    this.closeModal();
                    this.loadCategories();
                }
            });
        }
    }

    deleteCategory() {
        if (!this.categoryToDelete) return;

        this.categoryService.deleteCategory(this.categoryToDelete.id).subscribe({
            next: () => {
                this.showDeleteModal = false;
                this.categoryToDelete = null;
                this.loadCategories();
            }
        });
    }
}
