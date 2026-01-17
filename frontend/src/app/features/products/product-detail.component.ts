import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../core/services/product.service';
import { CartService } from '../../core/services/cart.service';
import { AuthService } from '../../core/services/auth.service';
import { ProductDetails, Review } from '../../core/models';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  private productService = inject(ProductService);
  private cartService = inject(CartService);
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  product = signal<ProductDetails | null>(null);
  loading = signal(true);
  selectedImage = signal<string>('');
  isAdding = signal(false);
  quantity = 1;

  ngOnInit() {
    const productId = this.route.snapshot.params['id'];
    if (productId) {
      this.loadProduct(+productId);
    }
  }

  loadProduct(id: number) {
    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.product.set(product);
        if (product.imageUrls && product.imageUrls.length > 0) {
          this.selectedImage.set(product.imageUrls[0]);
        }
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  selectImage(imagePath: string) {
    this.selectedImage.set(imagePath);
  }

  averageRating(): number {
    const reviews = this.product()?.reviews;
    if (!reviews || reviews.length === 0) return 0;
    const sum = reviews.reduce((acc, r) => acc + r.rating, 0);
    return sum / reviews.length;
  }

  decreaseQuantity() {
    if (this.quantity > 1) this.quantity--;
  }

  increaseQuantity() {
    if (this.quantity < (this.product()?.stockQuantity || 0)) {
      this.quantity++;
    }
  }

  addToCart() {
    const userId = this.authService.getUserId();
    if (!userId) {
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }

    this.isAdding.set(true);
    this.cartService.addToCart({
      userId,
      productId: this.product()!.id,
      quantity: this.quantity
    }).subscribe({
      next: () => {
        this.isAdding.set(false);
        this.router.navigate(['/cart']);
      },
      error: () => {
        this.isAdding.set(false);
      }
    });
  }

  trackById(index: number, item: Review): number {
    return item.id;
  }
}
