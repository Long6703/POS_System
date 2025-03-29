using POS.Shared.Entities;

namespace POS_API.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersWithDetailsAsync();
        Task<User> GetUserByIdWithDetailsAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
    }
}
