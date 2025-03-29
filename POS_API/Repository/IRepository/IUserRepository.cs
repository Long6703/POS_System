using POS.Shared.DTOs;
using POS.Shared.Entities;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<User>> GetUsersWithDetailsAsync();
        Task<User> GetUserByIdWithDetailsAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UpdateRoleInShop(UpdateUserRolesDto updateRolesDto);
        Task<UserDTO> GetUserByIdWithShopAsync(Guid id);
    }
}
