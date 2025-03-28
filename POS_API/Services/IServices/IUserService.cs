using POS.Shared.DTOs;

namespace POS_API.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUserDTOs();
    }
}
