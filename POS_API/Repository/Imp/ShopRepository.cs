using Microsoft.EntityFrameworkCore;
using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

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
    }
}
