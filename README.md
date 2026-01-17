# TreeShop - E-commerce Plant Shop

<div align="center">

🌿 **TreeShop** - Cửa hàng cây cảnh trực tuyến

[![Angular](https://img.shields.io/badge/Angular-18-DD0031?logo=angular)](https://angular.io/)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-2019-CC2927?logo=microsoftsqlserver)](https://www.microsoft.com/sql-server)

</div>

## 📋 Mục lục

- [Giới thiệu](#giới-thiệu)
- [Công nghệ](#công-nghệ)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Cài đặt](#cài-đặt)
- [Chạy dự án](#chạy-dự-án)
- [Scripts](#scripts)

## 🌱 Giới thiệu

TreeShop là một trang web thương mại điện tử chuyên bán cây cảnh, cây giống với các tính năng:

- ✅ Xem danh sách sản phẩm và chi tiết
- ✅ Tìm kiếm và lọc theo danh mục
- ✅ Giỏ hàng và thanh toán
- ✅ Đăng ký / Đăng nhập
- ✅ Trang quản trị Admin

## 🛠 Công nghệ

| Layer | Technology |
|-------|------------|
| Frontend | Angular 18, TypeScript, RxJS |
| Backend | .NET 8, ASP.NET Core Web API |
| Database | SQL Server |
| Auth | JWT, BCrypt |

## 📁 Cấu trúc dự án

```
Web_treeshop/
├── frontend/          # Angular application
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/       # Services, Guards, Models
│   │   │   ├── features/   # Feature components
│   │   │   └── shared/     # Shared components
│   │   └── environments/
│   └── package.json
│
├── backend/           # .NET Core API
│   └── backend1/
│       ├── Controllers/
│       ├── Models/
│       ├── Repositories/
│       └── Data/
│
├── package.json       # Root workspace config
└── README.md
```

## 🚀 Cài đặt

### Yêu cầu

- Node.js >= 18.x
- .NET SDK 8.0
- SQL Server

### Bước 1: Clone repository

```bash
git clone <repository-url>
cd Web_treeshop
```

### Bước 2: Cài đặt dependencies

```bash
# Cài đặt tất cả dependencies (root + frontend)
npm install
```

### Bước 3: Cấu hình database

Cập nhật connection string trong `backend/backend1/appsettings.json`

## 🎯 Chạy dự án

### Chạy cả Frontend và Backend cùng lúc

```bash
npm run dev
```

### Chạy riêng từng phần

```bash
# Chỉ Frontend (http://localhost:4200)
npm run frontend

# Chỉ Backend (http://localhost:5089)
npm run backend
```

## 📜 Scripts

| Script | Mô tả |
|--------|-------|
| `npm run dev` | Chạy cả frontend và backend đồng thời |
| `npm run frontend` | Chạy Angular dev server |
| `npm run backend` | Chạy .NET API server |
| `npm run build` | Build frontend production |
| `npm run clean` | Xóa các thư mục build |
| `npm run lint` | Kiểm tra linting frontend |

## 👤 Tài khoản mẫu

| Role | Username | Password |
|------|----------|----------|
| Admin | admin | 123456 |
| Customer | user | 123456 |

---

<div align="center">
Made with 💚 by TreeShop Team
</div>
