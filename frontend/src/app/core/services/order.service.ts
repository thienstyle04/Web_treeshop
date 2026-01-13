import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Order, CreateOrderRequest } from '../models';

@Injectable({
    providedIn: 'root'
})
export class OrderService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Orders`;

    getAllOrders(): Observable<Order[]> {
        return this.http.get<Order[]>(this.baseUrl);
    }

    getOrderById(id: number): Observable<Order> {
        return this.http.get<Order>(`${this.baseUrl}/${id}`);
    }

    getOrdersByUserId(userId: number): Observable<Order[]> {
        return this.http.get<Order[]>(`${this.baseUrl}/user/${userId}`);
    }

    createOrder(order: CreateOrderRequest): Observable<Order> {
        return this.http.post<Order>(this.baseUrl, order);
    }

    deleteOrder(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

    updateStatus(id: number, status: string): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}/status`, JSON.stringify(status), {
            headers: { 'Content-Type': 'application/json' }
        });
    }
}
