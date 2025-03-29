using POS.Shared.Entities;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<OrderDetail> OrderDetails { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Shop> Shops { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        Task<int> CompleteAsync();
    }
}
