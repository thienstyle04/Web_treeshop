import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Product } from '../../../core/models';
import { CartService } from '../../../core/services/cart.service';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-product-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent {
  @Input() product!: Product;

  private cartService = inject(CartService);
  private authService = inject(AuthService);

  isAdding = false;

  addToCart(event: Event) {
    event.preventDefault();
    event.stopPropagation();

    const userId = this.authService.getUserId();
    if (!userId) {
      // Redirect to login
      return;
    }

    this.isAdding = true;
    this.cartService.addToCart({
      userId,
      productId: this.product.id,
      quantity: 1
    }).subscribe({
      next: () => {
        this.isAdding = false;
      },
      error: () => {
        this.isAdding = false;
      }
    });
  }

  // Fallback images for each category (verified working Unsplash URLs)
  private readonly categoryFallbacks: { [key: string]: string } = {
    'Cây Cảnh Văn Phòng': 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400',
    'Cây Nội Thất': 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400',
    'Tiểu Cảnh Terrarium': 'https://images.unsplash.com/photo-1485955900006-10f4d324d411?w=400',
    'Sen Đá': 'https://images.unsplash.com/photo-1459156212016-c812468e2115?w=400',
    'Xương Rồng': 'https://images.unsplash.com/photo-1459411552884-841db9b3cc2a?w=400',
    'Quà Tặng': 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400'
  };

  private readonly defaultFallback = 'https://images.unsplash.com/photo-1463936575829-25148e1db1b8?w=400';

  onImageError(event: Event) {
    const img = event.target as HTMLImageElement;
    img.src = this.getFallbackImage();
  }

  getFallbackImage(): string {
    const category = this.product?.categoryName || '';
    return this.categoryFallbacks[category] || this.defaultFallback;
  }
}
