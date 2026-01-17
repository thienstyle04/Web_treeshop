import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../core/services/product.service';
import { CategoryService } from '../../core/services/category.service';
import { Product, Category } from '../../core/models';
import { ProductCardComponent } from '../../shared/components/product-card/product-card.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, ProductCardComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private productService = inject(ProductService);
  private categoryService = inject(CategoryService);

  featuredProducts = signal<Product[]>([]);
  categories = signal<Category[]>([]);
  productsLoading = signal(true);
  categoriesLoading = signal(true);

  ngOnInit() {
    this.loadProducts();
    this.loadCategories();
  }

  loadProducts() {
    this.productService.getAllProducts().subscribe({
      next: (products) => {
        // Filter out products from "Chậu Cây Cảnh" category
        const filteredProducts = products.filter(p => p.categoryName !== 'Chậu Cây Cảnh');
        this.featuredProducts.set(filteredProducts.slice(0, 8));
        this.productsLoading.set(false);
      },
      error: () => {
        this.productsLoading.set(false);
      }
    });
  }

  loadCategories() {
    this.categoryService.getAllCategories().subscribe({
      next: (categories) => {
        // Filter out "Chậu Cây Cảnh" category
        const filteredCategories = categories.filter(c => c.name !== 'Chậu Cây Cảnh');
        this.categories.set(filteredCategories.slice(0, 4));
        this.categoriesLoading.set(false);
      },
      error: () => {
        this.categoriesLoading.set(false);
      }
    });
  }

  subscribeNewsletter(event: Event) {
    event.preventDefault();
    // Handle newsletter subscription
    console.log('Newsletter subscription');
  }
}
