import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    const user = authService.getCurrentUser();

    if (!user) {
        router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }

    if (user.role !== 'Admin') {
        router.navigate(['/']);
        return false;
    }

    return true;
};
