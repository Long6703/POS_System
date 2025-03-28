using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.Imp
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSSystemDBContext _context;
        public IGenericRepository<User> Users { get; }
        public IGenericRepository<Product> Products { get; }
        public IGenericRepository<Order> Orders { get; }
        public IGenericRepository<OrderDetail> OrderDetails { get; }
        public IGenericRepository<Payment> Payments { get; }

        public UnitOfWork(POSSystemDBContext context)
        {
            _context = context;
            Users = new GenericRepository<User>(context);
            Products = new GenericRepository<Product>(context);
            Orders = new GenericRepository<Order>(context);
            OrderDetails = new GenericRepository<OrderDetail>(context);
            Payments = new GenericRepository<Payment>(context);
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
