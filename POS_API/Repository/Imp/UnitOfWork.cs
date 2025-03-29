using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.Imp
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSSystemDBContext _context;
        public IUserRepository Users { get; }
        public IProductRepository Products { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IPaymentRepository Payments { get; }
        public IAuthRepository Auth { get; }
        public IShopRepository Shops { get; }
        public IUserShopRepository UserShops { get; }

        public UnitOfWork(
            POSSystemDBContext context,
            IUserRepository users,
            IProductRepository products,
            IOrderRepository orders,
            IOrderDetailRepository orderDetails,
            IPaymentRepository payments,
            IAuthRepository auth,
            IShopRepository shops,
            IUserShopRepository userShops)
        {
            _context = context;
            Users = users;
            Products = products;
            Orders = orders;
            OrderDetails = orderDetails;
            Payments = payments;
            Auth = auth;
            Shops = shops;
            UserShops = userShops;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
