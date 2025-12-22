using backend1.Data;
using backend1.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

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
builder.Services.AddScoped<IAuthRepository, SQLAuthRepository>();
// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
option.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    ClockSkew = TimeSpan.Zero,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
});
// Đăng ký DbContext cho Authentication và Identity
builder.Services.AddDbContext<LoginAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("LoginAuthConnection")));

// Cấu hình CORS cho phép frontend gọi API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Sử dụng CORS
app.UseCors("AllowAll");

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();