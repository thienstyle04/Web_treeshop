import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent),
        title: 'TreeShop - Cây Cảnh Chất Lượng'
    },
    {
        path: 'about',
        loadComponent: () => import('./features/about/about.component').then(m => m.AboutComponent),
        title: 'Về chúng tôi - TreeShop'
    },
    {
        path: 'products',
        loadComponent: () => import('./features/products/product-list.component').then(m => m.ProductListComponent),
        title: 'Sản phẩm - TreeShop'
    },
    {
        path: 'products/:id',
        loadComponent: () => import('./features/products/product-detail.component').then(m => m.ProductDetailComponent),
        title: 'Chi tiết sản phẩm - TreeShop'
    },
    {
        path: 'categories',
        loadComponent: () => import('./features/categories/categories.component').then(m => m.CategoriesComponent),
        title: 'Danh mục - TreeShop'
    },
    {
        path: 'cart',
        loadComponent: () => import('./features/cart/cart.component').then(m => m.CartComponent),
        canActivate: [authGuard],
        title: 'Giỏ hàng - TreeShop'
    },
    {
        path: 'login',
        loadComponent: () => import('./features/auth/login.component').then(m => m.LoginComponent),
        title: 'Đăng nhập - TreeShop'
    },
    {
        path: 'register',
        loadComponent: () => import('./features/auth/register.component').then(m => m.RegisterComponent),
        title: 'Đăng ký - TreeShop'
    },
    {
        path: 'profile',
        loadComponent: () => import('./features/user/profile.component').then(m => m.ProfileComponent),
        canActivate: [authGuard],
        title: 'Thông tin tài khoản - TreeShop'
    },
    {
        path: 'orders',
        loadComponent: () => import('./features/user/user-orders.component').then(m => m.UserOrdersComponent),
        canActivate: [authGuard],
        title: 'Đơn hàng của tôi - TreeShop'
    },
    {
        path: 'checkout',
        loadComponent: () => import('./features/checkout/checkout.component').then(m => m.CheckoutComponent),
        canActivate: [authGuard],
        title: 'Thanh toán - TreeShop'
    },
    {
        path: 'admin',
        loadComponent: () => import('./features/admin/layout/admin-layout.component').then(m => m.AdminLayoutComponent),
        canActivate: [adminGuard],
        title: 'Quản trị - TreeShop',
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            },
            {
                path: 'dashboard',
                loadComponent: () => import('./features/admin/dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent),
                title: 'Dashboard - Admin'
            },
            {
                path: 'products',
                loadComponent: () => import('./features/admin/products/admin-products.component').then(m => m.AdminProductsComponent),
                title: 'Quản lý sản phẩm - Admin'
            },
            {
                path: 'categories',
                loadComponent: () => import('./features/admin/categories/admin-categories.component').then(m => m.AdminCategoriesComponent),
                title: 'Quản lý danh mục - Admin'
            },
            {
                path: 'orders',
                loadComponent: () => import('./features/admin/orders/admin-orders.component').then(m => m.AdminOrdersComponent),
                title: 'Quản lý đơn hàng - Admin'
            },
            {
                path: 'users',
                loadComponent: () => import('./features/admin/users/admin-users.component').then(m => m.AdminUsersComponent),
                title: 'Quản lý người dùng - Admin'
            },
            {
                path: 'discounts',
                loadComponent: () => import('./features/admin/discounts/admin-discounts.component').then(m => m.AdminDiscountsComponent),
                title: 'Quản lý mã giảm giá - Admin'
            }
        ]
    },
    {
        path: '**',
        redirectTo: ''
    }
];

