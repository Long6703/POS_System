using Microsoft.EntityFrameworkCore;
using POS.Shared.DTOs;
using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;
using static POS_API.Enum.Enums;

namespace POS_API.Repository.Imp
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        public ShopRepository(POSSystemDBContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Shop>> GetShopsWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Products)
                .Include(s => s.UserShops)
                    .ThenInclude(us => us.User)
                .ToListAsync();
        }

        public async Task<Shop> GetShopByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(s => s.Products)
                .Include(s => s.UserShops)
                    .ThenInclude(us => us.User)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<ShopDto>> GetShopsWithShopOwnerAsync()
        {
            return await _context.Shops
                .Select(s => new ShopDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    EmailShopOwner = s.UserShops
                        .Where(us => us.Role == UserRole.ShopOwner)
                        .Select(us => us.User.Email)
                        .FirstOrDefault(),
                    isDeleted = s.isDeleted 
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateDelete(int shopId)
        {
            var shop = await _dbSet.FirstOrDefaultAsync(s => s.Id == shopId);
            if (shop == null)
            {
                return false;
            }
            shop.isDeleted = !shop.isDeleted;
            _dbSet.Update(shop);
            return true;
        }
    }
}
