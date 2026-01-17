import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

interface User {
    id: number;
    name: string;
    fullName: string;
    description?: string;
    role: string;
}

@Component({
    selector: 'app-admin-users',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './admin-users.component.html',
    styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {
    private http = inject(HttpClient);
    private baseUrl = `${environment.apiUrl}/Users`;

    users = signal<User[]>([]);
    loading = signal(true);

    adminCount = signal(0);
    customerCount = signal(0);

    ngOnInit() {
        this.loadUsers();
    }

    loadUsers() {
        this.loading.set(true);
        this.http.get<User[]>(this.baseUrl).subscribe({
            next: (users) => {
                this.users.set(users);
                this.adminCount.set(users.filter(u => u.role === 'Admin').length);
                this.customerCount.set(users.filter(u => u.role === 'Customer').length);
                this.loading.set(false);
            },
            error: () => {
                this.loading.set(false);
            }
        });
    }

    updateRole(user: User) {
        this.http.put(`${this.baseUrl}/${user.id}`, { role: user.role }).subscribe({
            next: () => {
                this.loadUsers();
            }
        });
    }
}
