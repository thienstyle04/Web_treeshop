// Product Interfaces
export interface Product {
    id: number;
    name: string;
    scientificName?: string;
    description?: string;
    price: number;
    stockQuantity: number;
    dateAdded: Date;
    categoryId?: number;
    categoryName?: string;
    imageUrls: string[];
    originalPrice?: number;
    discountPercentage?: number;
    isFlashSale?: boolean;
    flashSaleEndTime?: Date;
}

export interface ProductDetails {
    id: number;
    name: string;
    scientificName?: string;
    description?: string;
    price: number;
    stockQuantity: number;
    dateAdded: Date;
    categoryId?: number;
    categoryName?: string;
    imageUrls: string[];
    reviews?: Review[];
    originalPrice?: number;
    discountPercentage?: number;
    isFlashSale?: boolean;
    flashSaleEndTime?: Date;
}

export interface AddProductRequest {
    name: string;
    scientificName?: string;
    description?: string;
    price: number;
    stockQuantity: number;
    categoryId: number;
}

// Category Interfaces
export interface Category {
    id: number;
    name: string;
    urlHandler?: string;
    description?: string;
}

export interface AddCategoryRequest {
    name: string;
    urlHandler?: string;
    description?: string;
}

// Image Interface
export interface Image {
    id: number;
    fileName: string;
    filePath: string;
    isThumbnail: boolean;
    productId: number;
}

// Review Interfaces
export interface Review {
    id: number;
    rating: number;
    comment?: string;
    reviewDate: Date;
    productId: number;
    userId: number;
    userName?: string;
}

export interface AddReviewRequest {
    rating: number;
    comment?: string;
    productId: number;
    userId: number;
}

// User Interfaces
export interface User {
    id: number;
    name: string;
    fullName: string;
    phone?: string;
    role: string;
}

export interface LoginRequest {
    name: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    userId: number;
    name: string;
    fullName: string;
    role: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
    fullName: string;
    phone?: string;
}

// Cart Interfaces
export interface CartItem {
    id: number;
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    imageUrl?: string;
}

export interface AddToCartRequest {
    userId: number;
    productId: number;
    quantity: number;
}

// Order Interfaces
export interface Order {
    id: number;
    userId: number;
    orderDate: Date;
    totalAmount: number;
    orderStatus: string;
    shippingAddressId?: number;
}

export interface OrderItem {
    id: number;
    orderId: number;
    productId: number;
    productName?: string;
    quantity: number;
    unitPrice: number;
}

export interface CreateOrderRequest {
    userId: number;
    shippingAddressId: number;
    items: CreateOrderItemRequest[];
}

export interface CreateOrderItemRequest {
    productId: number;
    quantity: number;
}

// Shipping Address Interface
export interface ShippingAddress {
    id: number;
    userId: number;
    recipientName: string;
    streetAddress: string;
    city: string;
    phone: string;
    isDefault: boolean;
}
