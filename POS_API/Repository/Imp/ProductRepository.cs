using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

namespace POS_API.Repository.Imp
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(POSSystemDBContext context) : base(context)
        {
        }
    }
}
