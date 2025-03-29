using POS.Shared;
using POS.Shared.DTOs;

namespace POS_API.Services.IServices
{
    public interface IUserService
    {
        Task<PagedResultDto<UserDTO>> GetUsersAsync(UserSearchDto searchDto);
        Task<UserDTO> GetUserByIdAsync(Guid id);
        Task<UserDTO> CreateUserAsync(CreateUserDto createUserDto);
        Task<bool> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> UpdateUserRolesAsync(Guid id, UpdateUserRolesDto updateRolesDto);
    }
}
