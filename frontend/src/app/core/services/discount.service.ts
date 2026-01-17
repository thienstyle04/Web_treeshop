import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface Discount {
    id: number;
    code: string;
    discountType: string; // 'Percent' | 'Fixed'
    value: number;
    minimumOrderAmount: number;
    startDate: string;
    endDate: string;
    appliesToProductId?: number | null;
    usageLimit: number;
    usedCount: number;
    isActive: boolean;
}

export interface AddDiscountRequest {
    code: string;
    discountType: string;
    value: number;
    minimumOrderAmount: number;
    startDate: string;
    endDate: string;
    appliesToProductId?: number | null;
    usageLimit: number;
    isActive: boolean;
}

@Injectable({
    providedIn: 'root'
})
export class DiscountService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Discounts`;

    getAllDiscounts(): Observable<Discount[]> {
        return this.http.get<Discount[]>(this.baseUrl);
    }

    getDiscountById(id: number): Observable<Discount> {
        return this.http.get<Discount>(`${this.baseUrl}/${id}`);
    }

    checkCode(code: string): Observable<Discount> {
        return this.http.get<Discount>(`${this.baseUrl}/check/${code}`);
    }

    createDiscount(discount: AddDiscountRequest): Observable<Discount> {
        return this.http.post<Discount>(this.baseUrl, discount);
    }

    updateDiscount(id: number, discount: AddDiscountRequest): Observable<Discount> {
        return this.http.put<Discount>(`${this.baseUrl}/${id}`, discount);
    }

    deleteDiscount(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}
