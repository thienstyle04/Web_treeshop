using backend1.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace backend1.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Khi xóa User -> KHÔNG tự động xóa Order (phải xóa Order trước hoặc set null)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Quan trọng: NoAction thay vì Cascade

            // 2. Khi xóa ShippingAddress -> KHÔNG tự động xóa Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany() // Nếu bên ShippingAddress không có List<Order> thì để trống
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.NoAction); // Quan trọng: NoAction

            // 3. (Tùy chọn) Cấu hình cho OrderItem để chắc chắn
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Order thì xóa OrderItem là đúng (giữ nguyên)
        }

        // 3. Khai báo các bảng (DbSet) - Đầy đủ theo danh sách bạn cung cấp
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
