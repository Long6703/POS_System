using Microsoft.EntityFrameworkCore;
using POS.Shared.DTOs;
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

        public async Task<bool> UpdateRoleInShop(UpdateUserRolesDto updateRolesDto)
        {
            var user = await _dbSet
                .Include(u => u.UserShops)
                .FirstOrDefaultAsync(u => u.Id == updateRolesDto.UserId && !u.isDeleted);

            if (user == null)
            {
                return false;
            }

            var userShop = user.UserShops.FirstOrDefault(us => us.ShopId == updateRolesDto.ShopId);
            if (userShop == null)
            {
                UserShop userShop1 = new UserShop
                {
                    UserId = user.Id,
                    ShopId = updateRolesDto.ShopId,
                    Role = updateRolesDto.Role
                };
                user.UserShops.Add(userShop1);
                return true;
            }

            userShop.Role = updateRolesDto.Role;
            return true;
        }

        public async Task<UserDTO> GetUserByIdWithShopAsync(Guid id)
        {
            var user = await _dbSet
                .Include(u => u.UserShops)
                .FirstOrDefaultAsync(u => u.Id == id && !u.isDeleted);

            if (user == null)
            {
                return null;
            }

            var userDto = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                UserShops = user.UserShops.Select(us => new UserShopDTO
                {
                    UserId = us.UserId,
                    ShopId = us.ShopId,
                    Role = us.Role.ToString(),
                }).ToList()
            };

            return userDto;
        }
    }
}
