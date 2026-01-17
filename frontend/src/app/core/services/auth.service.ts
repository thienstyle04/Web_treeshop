import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User, LoginRequest, LoginResponse, RegisterRequest } from '../models';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Auth`;

    private currentUserSubject = new BehaviorSubject<User | null>(this.loadUserFromStorage());
    currentUser$ = this.currentUserSubject.asObservable();

    private isLoggedInSignal = signal(this.hasToken());
    isLoggedIn = computed(() => this.isLoggedInSignal());

    private loadUserFromStorage(): User | null {
        const userJson = localStorage.getItem('user');
        if (!userJson) return null;
        try {
            return JSON.parse(userJson);
        } catch (e) {
            console.error('Error parsing user from storage', e);
            localStorage.removeItem('user');
            return null;
        }
    }

    private hasToken(): boolean {
        return !!localStorage.getItem('token');
    }

    login(credentials: LoginRequest): Observable<LoginResponse> {
        return this.http.post<LoginResponse>(`${this.baseUrl}/login`, credentials).pipe(
            tap(response => {
                const user: User = {
                    id: response.userId,
                    name: response.name,
                    fullName: response.fullName,
                    role: response.role
                };
                localStorage.setItem('token', response.token);
                localStorage.setItem('user', JSON.stringify(user));
                this.currentUserSubject.next(user);
                this.isLoggedInSignal.set(true);
            })
        );
    }

    register(userData: RegisterRequest): Observable<any> {
        return this.http.post(`${this.baseUrl}/register`, userData);
    }

    logout(): void {
        localStorage.removeItem('token');
        localStorage.removeItem('user');
        this.currentUserSubject.next(null);
        this.isLoggedInSignal.set(false);
    }

    getToken(): string | null {
        return localStorage.getItem('token');
    }

    getCurrentUser(): User | null {
        return this.currentUserSubject.value;
    }

    getUserId(): number | null {
        const user = this.getCurrentUser();
        return user?.id ?? null;
    }
}
