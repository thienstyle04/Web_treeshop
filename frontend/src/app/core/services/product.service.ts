import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Product, ProductDetails, AddProductRequest } from '../models';

@Injectable({
    providedIn: 'root'
})
export class ProductService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Products`;

    getAllProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(`${this.baseUrl}/get-all-products`);
    }

    getProductById(id: number): Observable<ProductDetails> {
        return this.http.get<ProductDetails>(`${this.baseUrl}/get-product-by-id/${id}`);
    }

    addProduct(product: AddProductRequest): Observable<Product> {
        return this.http.post<Product>(`${this.baseUrl}/add-product`, product);
    }

    updateProduct(id: number, product: AddProductRequest): Observable<Product> {
        return this.http.put<Product>(`${this.baseUrl}/update-product-by-id/${id}`, product);
    }

    deleteProduct(id: number): Observable<Product> {
        return this.http.delete<Product>(`${this.baseUrl}/delete-product-by-id/${id}`);
    }

    searchProducts(term: string): Observable<Product[]> {
        if (!term.trim()) {
            return this.getAllProducts();
        }
        return this.http.get<Product[]>(`${this.baseUrl}/search?term=${encodeURIComponent(term)}`);
    }
}
