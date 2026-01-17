import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Category, AddCategoryRequest } from '../models';

@Injectable({
    providedIn: 'root'
})
export class CategoryService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Categories`;

    getAllCategories(): Observable<Category[]> {
        return this.http.get<Category[]>(this.baseUrl);
    }

    getCategoryById(id: number): Observable<Category> {
        return this.http.get<Category>(`${this.baseUrl}/${id}`);
    }

    addCategory(category: AddCategoryRequest): Observable<Category> {
        return this.http.post<Category>(this.baseUrl, category);
    }

    updateCategory(id: number, category: AddCategoryRequest): Observable<Category> {
        return this.http.put<Category>(`${this.baseUrl}/${id}`, category);
    }

    deleteCategory(id: number): Observable<Category> {
        return this.http.delete<Category>(`${this.baseUrl}/${id}`);
    }
}
