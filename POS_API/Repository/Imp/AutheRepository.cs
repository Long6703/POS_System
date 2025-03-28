using Microsoft.EntityFrameworkCore;
using POS.Shared.Entities;
using POS.Shared.RequestModel;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

namespace POS_API.Repository.Imp
{
    public class AutheRepository : GenericRepository<User>, IAuthRepository
    {
        public AutheRepository(POSSystemDBContext context) : base(context)
        {
        }

        public async Task<User> Login(LoginRequest request)
        {
            var user = await _context.Users.Include(u => u.UserShops).Where(u => u.isDeleted == false).FirstOrDefaultAsync(user => user.Email.Equals(request.Email));
            return user;
        }
    }
}
