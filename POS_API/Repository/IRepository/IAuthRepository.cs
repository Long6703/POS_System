using POS.Shared.Entities;
using POS.Shared.RequestModel;
using static POS_API.Repository.IRepository.IGenericRepository;

namespace POS_API.Repository.IRepository
{
    public interface IAuthRepository : IGenericRepository<User>
    {
        Task<User> Login(LoginRequest request);
    }
}
