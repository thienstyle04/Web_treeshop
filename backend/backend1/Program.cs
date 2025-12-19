using backend1.Data;
using backend1.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// --- KHẮC PHỤC LỖI QUAN TRỌNG Ở ĐÂY ---
// Đăng ký HttpContextAccessor để SQLImageRepository có thể lấy được Scheme/Host khi tạo URL ảnh
builder.Services.AddHttpContextAccessor();
// -------------------------------------

// Enable Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShopTree",
        Version = "v1",
        Description = "Mô tả chi tiết về API Backend"
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Khai báo Repository Pattern
builder.Services.AddScoped<IProductRepository, SQLProductRepository>();
builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
builder.Services.AddScoped<IReviewRepository, SQLReviewRepository>();
builder.Services.AddScoped<IImageRepository, SQLImageRepository>();
builder.Services.AddScoped<IDiscountRepository, SQLDiscountRepository>();
builder.Services.AddScoped<IShoppingCartRepository, SQLShoppingCartRepository>();
builder.Services.AddScoped<IShippingAddressRepository, SQLShippingAddressRepository>();
builder.Services.AddScoped<ICategoryRepository, SQLCategoryRepository>();
builder.Services.AddScoped<IOrderRepository, SQLOrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, SQLOrderItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopTree");
    });
}

app.UseHttpsRedirection();

// Cho phép truy cập ảnh trong thư mục wwwroot
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();