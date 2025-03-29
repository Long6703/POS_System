using Microsoft.EntityFrameworkCore;
using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

namespace POS_API.Repository.Imp
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(POSSystemDBContext context) : base(context)
        {
        }

        public async Task<Order> GetOrderWithDetailsAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Payments)
                .Include(o => o.Shop)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId, int shopId)
        {
            var query = _context.Orders.AsQueryable();

            if (shopId > 0)
                query = query.Where(o => o.ShopId == shopId);

            return await query
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.Payments)
                .Include(o => o.Shop)
                .ToListAsync();
        }
    }
}
