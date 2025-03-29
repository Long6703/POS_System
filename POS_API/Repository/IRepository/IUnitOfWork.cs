using POS.Shared.Entities;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IPaymentRepository Payments { get; }
        IShopRepository Shops { get; }
        IOrderDetailRepository OrderDetails { get; }
        IAuthRepository Auth { get; }
        IUserShopRepository UserShops { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        Task<int> CompleteAsync();
    }
}
