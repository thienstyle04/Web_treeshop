import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { CategoryService } from '../../core/services/category.service';
import { Product, Category } from '../../core/models';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ProductCardComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);
  private route = inject(ActivatedRoute);

  products = signal<Product[]>([]);
  filteredProducts = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  loading = signal(true);
  selectedCategory = signal<number | null>(null);

  sortBy = 'newest';
  viewMode: 'grid' | 'list' = 'grid';

  ngOnInit() {
    this.loadCategories();
    this.loadProducts();

    // Check for category query param
    this.route.queryParams.subscribe(params => {
      if (params['category']) {
        this.selectedCategory.set(+params['category']);
        this.applyFilters();
      }
    });
  }

  loadProducts() {
    this.productService.getAllProducts().subscribe({
      next: (products) => {
        // Filter out products from excluded categories
        const filteredProducts = products.filter(p => !this.excludedCategories.includes(p.categoryName || ''));
        this.products.set(filteredProducts);
        this.applyFilters();
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  // Categories to exclude from display
  private excludedCategories = ['Chậu Cây Cảnh', 'Tiểu Cảnh Terrarium', 'Xương Rồng', 'Quà Tặng'];

  loadCategories() {
    this.categoryService.getAllCategories().subscribe({
      next: (categories) => {
        // Filter out excluded categories
        const filteredCategories = categories.filter(c => !this.excludedCategories.includes(c.name));
        this.categories.set(filteredCategories);
      }
    });
  }

  selectCategory(categoryId: number | null) {
    this.selectedCategory.set(categoryId);
    this.applyFilters();
  }

  applyFilters() {
    let filtered = [...this.products()];

    // Filter by category
    if (this.selectedCategory()) {
      filtered = filtered.filter(p => p.categoryId === this.selectedCategory());
      // Limit to 10 products when a specific category is selected
      filtered = filtered.slice(0, 10);
    } else {
      // "Tất cả" - show ALL products from allowed categories (Cây cảnh văn phòng, Cây nội thất, Sen đá)
      const allowedCategories = ['Cây Cảnh Văn Phòng', 'Cây Nội Thất', 'Sen Đá'];
      filtered = filtered.filter(p => allowedCategories.includes(p.categoryName || ''));
    }



    // Sort
    switch (this.sortBy) {
      case 'price-asc':
        filtered.sort((a, b) => a.price - b.price);
        break;
      case 'price-desc':
        filtered.sort((a, b) => b.price - a.price);
        break;
      case 'name':
        filtered.sort((a, b) => (a.name || '').localeCompare(b.name || ''));
        break;
      case 'newest':
      default:
        filtered.sort((a, b) => new Date(b.dateAdded).getTime() - new Date(a.dateAdded).getTime());
    }

    this.filteredProducts.set(filtered);
  }

  resetFilters() {
    this.selectedCategory.set(null);
    this.sortBy = 'newest';
    this.applyFilters();
  }
}
