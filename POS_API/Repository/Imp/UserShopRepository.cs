using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

namespace POS_API.Repository.Imp
{
    public class UserShopRepository : GenericRepository<UserShop>, IUserShopRepository
    {
        public UserShopRepository(POSSystemDBContext context) : base(context)
        {
        }
    }
}
