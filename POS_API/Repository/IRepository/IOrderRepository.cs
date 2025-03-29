using POS.Shared.Entities;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetOrderWithDetailsAsync(Guid id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId, int shopId);
    }
}
