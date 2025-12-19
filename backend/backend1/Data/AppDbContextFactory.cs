using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System; // Nhớ thêm dòng này để dùng Console

namespace backend1.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // 1. In ra đường dẫn thư mục hiện tại mà lệnh đang chạy
            var basePath = Directory.GetCurrentDirectory();
            Console.WriteLine($"\n[DEBUG] Current Directory: {basePath}");

            // 2. Thiết lập cấu hình
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true) // optional: true để không crash ngay nếu thiếu file
                .Build();

            // 3. Lấy connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // 4. === IN LOG RA MÀN HÌNH ===
            Console.WriteLine("------------------------------------------------------------");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[DEBUG] LỖI: Connection String bị NULL hoặc Rỗng!");
                Console.WriteLine("[DEBUG] Kiểm tra xem file appsettings.json có ở thư mục trên không?");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[DEBUG] Connection String tìm thấy: {connectionString}");
                Console.ResetColor();
            }
            Console.WriteLine("------------------------------------------------------------\n");

            // 5. Nếu null thì gán tạm một giá trị để tránh lỗi "Object reference" ngay tại đây, 
            // giúp ta đọc được log bên trên trước khi chương trình dừng lại.
            if (string.IsNullOrEmpty(connectionString))
            {
                // Gán bừa để không crash dòng UseSqlServer, nhưng vẫn sẽ lỗi kết nối sau đó
                connectionString = "Server=ERROR;Database=ERROR;";
            }

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            try
            {
                var context = new AppDbContext(optionsBuilder.Options);
                Console.WriteLine("[DEBUG] Constructor chạy xong.");

                // === THÊM DÒNG NÀY ĐỂ BẮT LỖI OnModelCreating ===
                Console.WriteLine("[DEBUG] Đang cố gắng build Model (Chạy OnModelCreating)...");
                var model = context.Model; // Dòng này ép OnModelCreating chạy ngay lập tức
                                           // ===============================================

                Console.WriteLine("[DEBUG] Build Model thành công! Factory hoàn tất.");
                return context;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[LỖI CHI TIẾT] Lỗi xảy ra bên trong OnModelCreating:");
                Console.WriteLine(ex.ToString()); // Nó sẽ chỉ ra chính xác dòng nào trong AppDbContext bị null
                Console.ResetColor();
                throw;
            }

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}