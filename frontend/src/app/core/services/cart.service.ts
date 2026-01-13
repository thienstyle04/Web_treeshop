import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CartItem, AddToCartRequest } from '../models';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})
export class CartService {
    private http = inject(HttpClient);
    private authService = inject(AuthService);
    private baseUrl = `${environment.apiUrl}/ShoppingCart`;

    constructor() {
        // Automatically load cart when user changes
        this.authService.currentUser$.subscribe(user => {
            if (user) {
                this.loadCart();
            } else {
                this.clearLocalCart();
            }
        });
    }

    private cartItemsSignal = signal<CartItem[]>([]);
    cartItems = computed(() => this.cartItemsSignal());

    cartCount = computed(() =>
        this.cartItemsSignal().reduce((sum, item) => sum + item.quantity, 0)
    );

    cartTotal = computed(() =>
        this.cartItemsSignal().reduce((sum, item) => sum + (item.price * item.quantity), 0)
    );

    loadCart(): void {
        const userId = this.authService.getUserId();
        if (userId) {
            this.getCartByUserId(userId).subscribe(items => {
                this.cartItemsSignal.set(items);
            });
        }
    }

    getCartByUserId(userId: number): Observable<CartItem[]> {
        return this.http.get<CartItem[]>(`${this.baseUrl}/${userId}`);
    }

    addToCart(request: AddToCartRequest): Observable<any> {
        return this.http.post(`${this.baseUrl}/add`, request).pipe(
            tap(() => this.loadCart())
        );
    }

    updateQuantity(cartItemId: number, quantity: number): Observable<any> {
        return this.http.put(`${this.baseUrl}/${cartItemId}`, { quantity }).pipe(
            tap(() => this.loadCart())
        );
    }

    removeFromCart(cartItemId: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/remove/${cartItemId}`).pipe(
            tap(() => this.loadCart())
        );
    }

    clearLocalCart(): void {
        this.cartItemsSignal.set([]);
    }
}
