# API Backup - Các file đã sửa đổi

> Backup ngày: 2025-12-25

---

## 1. IDiscountRepository.cs

```csharp
using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllDiscountsAsync();
        Task<Discount?> GetDiscountByIdAsync(int id);
        Task<Discount?> GetDiscountByCodeAsync(string code);
        Task<Discount> CreateDiscountAsync(AddDiscountRequestDTO request);
        Task<Discount?> UpdateDiscountAsync(int id, AddDiscountRequestDTO request);
        Task<Discount?> DeleteDiscountAsync(int id);
    }
}
```

---

## 2. SQLDiscountRepository.cs

```csharp
using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLDiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLDiscountRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<Discount>> GetAllDiscountsAsync() => await _dbContext.Discounts.ToListAsync();

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _dbContext.Discounts.FindAsync(id);
        }

        public async Task<Discount?> GetDiscountByCodeAsync(string code)
        {
            return await _dbContext.Discounts.FirstOrDefaultAsync(d => d.Code == code && d.IsActive && d.ExpiryDate > DateTime.Now);
        }

        public async Task<Discount> CreateDiscountAsync(AddDiscountRequestDTO request)
        {
            var discount = new Discount
            {
                Code = request.Code,
                DiscountType = request.DiscountType,
                Value = request.Value,
                MinimumOrderAmount = request.MinimumOrderAmount,
                ExpiryDate = request.ExpiryDate,
                IsActive = request.IsActive
            };
            await _dbContext.Discounts.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount?> UpdateDiscountAsync(int id, AddDiscountRequestDTO request)
        {
            var discount = await _dbContext.Discounts.FindAsync(id);
            if (discount == null) return null;

            discount.Code = request.Code;
            discount.DiscountType = request.DiscountType;
            discount.Value = request.Value;
            discount.MinimumOrderAmount = request.MinimumOrderAmount;
            discount.ExpiryDate = request.ExpiryDate;
            discount.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount?> DeleteDiscountAsync(int id)
        {
            var discount = await _dbContext.Discounts.FindAsync(id);
            if (discount == null) return null;
            _dbContext.Discounts.Remove(discount);
            await _dbContext.SaveChangesAsync();
            return discount;
        }
    }
}
```

---

## 3. DiscountsController.cs

```csharp
using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountsController(IDiscountRepository discountRepository) { _discountRepository = discountRepository; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _discountRepository.GetAllDiscountsAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountRepository.GetDiscountByIdAsync(id);
            return discount == null ? NotFound() : Ok(discount);
        }

        [HttpGet("check/{code}")]
        public async Task<IActionResult> CheckCode(string code)
        {
            var discount = await _discountRepository.GetDiscountByCodeAsync(code);
            return discount == null ? NotFound("Mã không hợp lệ hoặc đã hết hạn") : Ok(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDiscountRequestDTO request)
        {
            var discount = await _discountRepository.CreateDiscountAsync(request);
            return Ok(discount);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddDiscountRequestDTO request)
        {
            var discount = await _discountRepository.UpdateDiscountAsync(id, request);
            return discount == null ? NotFound() : Ok(discount);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _discountRepository.DeleteDiscountAsync(id);
            return res == null ? NotFound() : Ok(res);
        }
    }
}
```

---

## 4. IShippingAddressRepository.cs

```csharp
using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IShippingAddressRepository
    {
        Task<List<ShippingAddress>> GetAddressesByUserIdAsync(int userId);
        Task<ShippingAddress> AddAddressAsync(AddShippingAddressDTO request);
        Task<ShippingAddress?> UpdateAddressAsync(int id, AddShippingAddressDTO request);
        Task<ShippingAddress?> DeleteAddressAsync(int id);
    }
}
```

---

## 5. SQLShippingAddressRepository.cs

```csharp
using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLShippingAddressRepository : IShippingAddressRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLShippingAddressRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<ShippingAddress>> GetAddressesByUserIdAsync(int userId)
        {
            return await _dbContext.ShippingAddresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<ShippingAddress> AddAddressAsync(AddShippingAddressDTO request)
        {
            // Nếu set mặc định, bỏ mặc định cũ
            if (request.IsDefault)
            {
                var defaults = await _dbContext.ShippingAddresses
                    .Where(a => a.UserId == request.UserId && a.IsDefault)
                    .ToListAsync();
                foreach (var d in defaults) d.IsDefault = false;
            }

            var address = new ShippingAddress
            {
                UserId = request.UserId,
                RecipientName = request.RecipientName,
                StreetAddress = request.StreetAddress,
                City = request.City,
                Phone = request.Phone,
                IsDefault = request.IsDefault
            };
            await _dbContext.ShippingAddresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<ShippingAddress?> UpdateAddressAsync(int id, AddShippingAddressDTO request)
        {
            var address = await _dbContext.ShippingAddresses.FindAsync(id);
            if (address == null) return null;

            // Nếu set mặc định, bỏ mặc định cũ
            if (request.IsDefault && !address.IsDefault)
            {
                var defaults = await _dbContext.ShippingAddresses
                    .Where(a => a.UserId == request.UserId && a.IsDefault)
                    .ToListAsync();
                foreach (var d in defaults) d.IsDefault = false;
            }

            address.RecipientName = request.RecipientName;
            address.StreetAddress = request.StreetAddress;
            address.City = request.City;
            address.Phone = request.Phone;
            address.IsDefault = request.IsDefault;

            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<ShippingAddress?> DeleteAddressAsync(int id)
        {
            var address = await _dbContext.ShippingAddresses.FindAsync(id);
            if (address == null) return null;
            _dbContext.ShippingAddresses.Remove(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }
    }
}
```

---

## 6. ShippingAddressesController.cs

```csharp
using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly IShippingAddressRepository _repo;
        public ShippingAddressesController(IShippingAddressRepository repo) { _repo = repo; }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId) => Ok(await _repo.GetAddressesByUserIdAsync(userId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddShippingAddressDTO request)
        {
            var addr = await _repo.AddAddressAsync(request);
            return Ok(addr);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddShippingAddressDTO request)
        {
            var addr = await _repo.UpdateAddressAsync(id, request);
            return addr == null ? NotFound() : Ok(addr);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _repo.DeleteAddressAsync(id);
            return res == null ? NotFound() : Ok(res);
        }
    }
}
```

---

## 7. IReviewRepository.cs

```csharp
using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IReviewRepository
    {
        Task<List<ReviewDTO>> GetReviewsByProductIdAsync(int productId);
        Task<Review> AddReviewAsync(AddReviewDTO addReviewDTO);
        Task<Review?> UpdateReviewAsync(int id, AddReviewDTO request);
        Task<Review?> DeleteReviewAsync(int id);
    }
}
```

---

## 8. SQLReviewRepository.cs

```csharp
using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLReviewRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<ReviewDTO>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _dbContext.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            return reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate,
                ProductId = r.ProductId,
                UserName = r.User?.FullName ?? "Unknown"
            }).ToList();
        }

        public async Task<Review> AddReviewAsync(AddReviewDTO request)
        {
            var review = new Review
            {
                Rating = request.Rating,
                Comment = request.Comment,
                ProductId = request.ProductId,
                UserId = request.UserId,
                ReviewDate = DateTime.Now
            };
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> UpdateReviewAsync(int id, AddReviewDTO request)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review == null) return null;

            review.Rating = request.Rating;
            review.Comment = request.Comment;

            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> DeleteReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review == null) return null;
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }
    }
}
```

---

## 9. ReviewsController.cs

```csharp
using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewsController(IReviewRepository reviewRepository) { _reviewRepository = reviewRepository; }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _reviewRepository.GetReviewsByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddReviewDTO request)
        {
            var review = await _reviewRepository.AddReviewAsync(request);
            return Ok(review);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddReviewDTO request)
        {
            var review = await _reviewRepository.UpdateReviewAsync(id, request);
            return review == null ? NotFound() : Ok(review);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _reviewRepository.DeleteReviewAsync(id);
            return review == null ? NotFound() : Ok(review);
        }
    }
}
```

---

## 10. UpdateCartItemDTO.cs (FILE MỚI)

```csharp
namespace backend1.Models.DTO
{
    public class UpdateCartItemDTO
    {
        public int Quantity { get; set; }
    }
}
```

---

## 11. IShoppingCartRepository.cs

```csharp
using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<List<CartItemDTO>> GetCartByUserIdAsync(int userId);
        Task<ShoppingCartItem> AddToCartAsync(AddToCartRequestDTO request);
        Task<ShoppingCartItem?> UpdateCartItemAsync(int cartItemId, int quantity);
        Task<ShoppingCartItem?> RemoveFromCartAsync(int cartItemId);
    }
}
```

---

## 12. SQLShoppingCartRepository.cs

```csharp
using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLShoppingCartRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<CartItemDTO>> GetCartByUserIdAsync(int userId)
        {
            var items = await _dbContext.ShoppingCartItems
                .Include(c => c.Product)
                .ThenInclude(p => p.Images)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return items.Select(i => new CartItemDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown",
                Price = i.Product?.Price ?? 0,
                Quantity = i.Quantity,
                ImageUrl = i.Product?.Images?.FirstOrDefault()?.FilePath ?? ""
            }).ToList();
        }

        public async Task<ShoppingCartItem> AddToCartAsync(AddToCartRequestDTO request)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            var existingItem = await _dbContext.ShoppingCartItems
                .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                await _dbContext.SaveChangesAsync();
                return existingItem;
            }

            var newItem = new ShoppingCartItem
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                DateCreated = DateTime.Now
            };
            await _dbContext.ShoppingCartItems.AddAsync(newItem);
            await _dbContext.SaveChangesAsync();
            return newItem;
        }

        public async Task<ShoppingCartItem?> UpdateCartItemAsync(int cartItemId, int quantity)
        {
            var item = await _dbContext.ShoppingCartItems.FindAsync(cartItemId);
            if (item == null) return null;

            if (quantity <= 0)
            {
                _dbContext.ShoppingCartItems.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<ShoppingCartItem?> RemoveFromCartAsync(int cartItemId)
        {
            var item = await _dbContext.ShoppingCartItems.FindAsync(cartItemId);
            if (item == null) return null;
            _dbContext.ShoppingCartItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}
```

---

## 13. ShoppingCartController.cs

```csharp
using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _cartRepository;
        public ShoppingCartController(IShoppingCartRepository cartRepository) { _cartRepository = cartRepository; }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            return Ok(await _cartRepository.GetCartByUserIdAsync(userId));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDTO request)
        {
            var item = await _cartRepository.AddToCartAsync(request);
            return Ok(item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] UpdateCartItemDTO request)
        {
            var item = await _cartRepository.UpdateCartItemAsync(id, request.Quantity);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpDelete("remove/{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _cartRepository.RemoveFromCartAsync(id);
            return item == null ? NotFound() : Ok(item);
        }
    }
}
```

---

## 14. IImageRepository.cs

```csharp
using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IImageRepository
    {
        Task<List<Image>> GetImagesByProductIdAsync(int productId);
        Task<Image> UploadImageAsync(ImageDTO request);
        Task<Image?> DeleteImageAsync(int id);
    }
}
```

---

## 15. SQLImageRepository.cs

```csharp
using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public SQLImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<List<Image>> GetImagesByProductIdAsync(int productId)
        {
            return await _dbContext.Images.Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task<Image> UploadImageAsync(ImageDTO request)
        {
            var file = request.File;

            // SỬA 1: Sử dụng WebRootPath (wwwroot) thay vì ContentRootPath
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Nếu WebRootPath null (do chưa cấu hình đúng), fallback về ContentRootPath/wwwroot
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            }

            // Tạo đường dẫn tới thư mục Images bên trong wwwroot
            var imagesFolderPath = Path.Combine(webRootPath, "Images");

            // SỬA 2: Kiểm tra và tạo thư mục nếu chưa tồn tại (Tránh lỗi DirectoryNotFound)
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            var localFilePath = Path.Combine(imagesFolderPath, $"{request.FileName}{Path.GetExtension(file?.FileName)}");

            // Upload file to local folder
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // ... giữ nguyên phần tạo URL path và lưu DB
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{request.FileName}{Path.GetExtension(file.FileName)}";

            var image = new Image
            {
                FileName = request.FileName,
                FilePath = urlFilePath,
                IsThumbnail = request.IsThumbnail,
                ProductId = request.ProductId
            };

            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }

        public async Task<Image?> DeleteImageAsync(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null) return null;
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }
    }
}
```

---

## 16. ImagesController.cs

```csharp
using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository) { _imageRepository = imageRepository; }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            return Ok(await _imageRepository.GetImagesByProductIdAsync(productId));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageDTO request)
        {
            if (request.File == null || request.File.Length == 0) return BadRequest("File is empty");
            var image = await _imageRepository.UploadImageAsync(request);
            return Ok(image);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _imageRepository.DeleteImageAsync(id);
            return image == null ? NotFound() : Ok(image);
        }
    }
}
```

---

## Tóm tắt các endpoints mới thêm

| Controller | Method | Endpoint |
|------------|--------|----------|
| Discounts | GET | `/api/Discounts/{id}` |
| Discounts | PUT | `/api/Discounts/{id}` |
| ShippingAddresses | PUT | `/api/ShippingAddresses/{id}` |
| Reviews | PUT | `/api/Reviews/{id}` |
| ShoppingCart | PUT | `/api/ShoppingCart/{id}` |
| Images | GET | `/api/Images/product/{productId}` |
