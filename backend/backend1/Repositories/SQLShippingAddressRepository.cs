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