using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLOrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLOrderRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<OrderDTO>> GetAllOrdersAsync(
            string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true,
            int pageNumber = 1, int pageSize = 1000)
        {
            var orders = _dbContext.Orders.AsQueryable();

            // 1. Filter: Lọc theo Trạng thái (Status) hoặc UserID
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    // Giả sử Status là string hoặc enum chuyển sang string
                    orders = orders.Where(x => x.OrderStatus.Contains(filterQuery));
                }
            }

            // 2. Sort: Sắp xếp theo Ngày đặt hoặc Tổng tiền
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("OrderDate", StringComparison.OrdinalIgnoreCase))
                {
                    orders = isAscending ? orders.OrderBy(x => x.OrderDate) : orders.OrderByDescending(x => x.OrderDate);
                }
                else if (sortBy.Equals("TotalPrice", StringComparison.OrdinalIgnoreCase))
                {
                    orders = isAscending ? orders.OrderBy(x => x.TotalAmount) : orders.OrderByDescending(x => x.TotalAmount);
                }
            }

            // 3. Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            // Map sang DTO
            return await orders
                .Skip(skipResults)
                .Take(pageSize)
                .Select(order => new OrderDTO
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    OrderStatus = order.OrderStatus,
                    TotalAmount = order.TotalAmount,
                    UserId = order.UserId,
                    RecipientName = order.ShippingAddress != null ? order.ShippingAddress.RecipientName : null,
                    Phone = order.ShippingAddress != null ? order.ShippingAddress.Phone : null,
                    StreetAddress = order.ShippingAddress != null ? order.ShippingAddress.StreetAddress : null,
                    City = order.ShippingAddress != null ? order.ShippingAddress.City : null,
                    OrderItems = order.OrderItems.Select(oi => new OrderDetailItemDTO
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.Product != null ? oi.Product.Name : "Unknown",
                        Quantity = oi.Quantity,
                        Price = oi.UnitPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _dbContext.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return MapToDTO(orders);
        }

        public async Task<OrderDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return null;
            return new OrderDTO
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                RecipientName = order.ShippingAddress?.RecipientName,
                OrderItems = order.OrderItems.Select(oi => new OrderDetailItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "Unknown",
                    Quantity = oi.Quantity,
                    Price = oi.UnitPrice
                }).ToList()
            };
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequestDTO request)
        {
            // Lấy giá sản phẩm hiện tại để tính tiền
            var productIds = request.Items?.Select(i => i.ProductId).ToList() ?? new List<int>();
            var products = await _dbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

            // Handle shipping address - create new if inline address is provided
            int shippingAddressId;
            if (request.ShippingAddressId.HasValue && request.ShippingAddressId.Value > 0)
            {
                shippingAddressId = request.ShippingAddressId.Value;
            }
            else
            {
                // Create new shipping address from inline data
                var newAddress = new ShippingAddress
                {
                    UserId = request.UserId,
                    RecipientName = request.RecipientName ?? "",
                    Phone = request.Phone ?? "",
                    StreetAddress = request.StreetAddress ?? "",
                    City = request.City ?? "",
                    IsDefault = false
                };
                await _dbContext.ShippingAddresses.AddAsync(newAddress);
                await _dbContext.SaveChangesAsync();
                shippingAddressId = newAddress.Id;
            }

            decimal total = 0;
            var newOrder = new Order
            {
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                UserId = request.UserId,
                ShippingAddressId = shippingAddressId,
                DiscountCodeUsed = request.DiscountCodeUsed,
                DiscountAmount = request.DiscountAmount
            };

            foreach (var item in request.Items ?? new List<CreateOrderItemDTO>())
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    // Kiểm tra và giảm số lượng tồn kho
                    if (product.StockQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"Sản phẩm '{product.Name}' không đủ số lượng trong kho. Còn lại: {product.StockQuantity}");
                    }
                    product.StockQuantity -= item.Quantity;

                    var price = product.Price;
                    total += price * item.Quantity;
                    newOrder.OrderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = price 
                    });
                }
            }
            newOrder.TotalAmount = total - request.DiscountAmount;
            if (newOrder.TotalAmount < 0) newOrder.TotalAmount = 0;

            await _dbContext.Orders.AddAsync(newOrder);
            await _dbContext.SaveChangesAsync();
            return newOrder;
        }

        public async Task<Order?> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) return null;
            order.OrderStatus = status;
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> DeleteOrderAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order == null) return null;
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        private List<OrderDTO> MapToDTO(List<Order> orders)
        {
            return orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                OrderStatus = o.OrderStatus,
                UserId = o.UserId,
                RecipientName = o.ShippingAddress?.RecipientName,
                Phone = o.ShippingAddress?.Phone,
                StreetAddress = o.ShippingAddress?.StreetAddress,
                City = o.ShippingAddress?.City,
                OrderItems = o.OrderItems.Select(oi => new OrderDetailItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.Name ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.UnitPrice
                }).ToList()
            }).ToList();
        }
    }
}