using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

namespace POS_API.Repository.Imp
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(POSSystemDBContext context) : base(context)
        {
        }
    }
}
