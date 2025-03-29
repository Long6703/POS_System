using Microsoft.EntityFrameworkCore;
using POS.Shared.Entities;
using POS_API.DatabaseContext;
using POS_API.Repository.IRepository;

namespace POS_API.Repository.Imp
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(POSSystemDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetUsersWithDetailsAsync()
        {
            return await _dbSet
                .Include(u => u.UserShops)
                    .ThenInclude(us => us.Shop)
                .Where(u => !u.isDeleted)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(u => u.UserShops)
                    .ThenInclude(us => us.Shop)
                .FirstOrDefaultAsync(u => u.Id == id && !u.isDeleted);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email && !u.isDeleted);
        }
    }
}
