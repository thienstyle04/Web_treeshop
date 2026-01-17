using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace backend1.Data
{
    public class LoginAuthDbContext : IdentityDbContext<IdentityUser>
    {
        public LoginAuthDbContext(DbContextOptions<LoginAuthDbContext> options) : base(options)
        {
        }

        // phan quyen reader  va writer cho nguoi dung
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var readerRoleId = "b1a1f1e2-3c4d-5e6f-7081-91a2b3c4d5e6";
            var writerRoleId = "c2b2f2e3-4d5e-6f70-8192-a2b3c4d5e6f7";
            base.OnModelCreating(builder);
            // Thêm các vai trò mặc định
           var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "READER"
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "WRITER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }

}
