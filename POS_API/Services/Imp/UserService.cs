using AutoMapper;
using Microsoft.AspNetCore.Identity;
using POS.Shared;
using POS.Shared.DTOs;
using POS.Shared.Entities;
using POS_API.Repository.IRepository;
using POS_API.Services.IServices;

namespace POS_API.Services.Imp
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserDTO> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Check if email already exists
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Email == createUserDto.Email && !u.isDeleted)).FirstOrDefault();
            if (existingUser != null)
            {
                throw new InvalidOperationException("already exists");
            }

            var user = _mapper.Map<User>(createUserDto);
            user.PasswordHash = HashingService.Hash(createUserDto.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public Task<bool> DeleteUserAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Id == id && !u.isDeleted)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<PagedResultDto<UserDTO>> GetUsersAsync(UserSearchDto searchDto)
        {
            var query = (await _unitOfWork.Users.GetAllAsync())
                .Where(u => !u.isDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.Name))
            {
                query = query.Where(u => u.Name.Contains(searchDto.Name, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(searchDto.Email))
            {
                query = query.Where(u => u.Email.Contains(searchDto.Email, StringComparison.OrdinalIgnoreCase));
            }

            if (searchDto.IsAdmin.HasValue)
            {
                query = query.Where(u => u.IsAdmin == searchDto.IsAdmin.Value);
            }

            var totalCount = query.Count();

            if (!string.IsNullOrEmpty(searchDto.SortBy))
            {
                switch (searchDto.SortBy.ToLower())
                {
                    case "name":
                        query = searchDto.IsDescending
                            ? query.OrderByDescending(u => u.Name)
                            : query.OrderBy(u => u.Name);
                        break;
                    case "email":
                        query = searchDto.IsDescending
                            ? query.OrderByDescending(u => u.Email)
                            : query.OrderBy(u => u.Email);
                        break;
                    default:
                        query = searchDto.IsDescending
                            ? query.OrderByDescending(u => u.Id)
                            : query.OrderBy(u => u.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(u => u.Id);
            }

            var pagedUsers = query
                .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            var userDtos = _mapper.Map<List<UserDTO>>(pagedUsers);

            return new PagedResultDto<UserDTO>(userDtos, totalCount, searchDto.PageNumber, searchDto.PageSize);
        }


        public async Task<bool> UpdateUserRolesAsync(Guid id, UpdateUserRolesDto updateRolesDto)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Id == id && !u.isDeleted)).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            // Get user with UserShops
            if (user.UserShops == null)
            {
                // Find the user with the included UserShops
                user = (await _unitOfWork.Users.FindAsync(u => u.Id == id)).FirstOrDefault();
                if (user == null || user.UserShops == null)
                {
                    return false;
                }
            }

            // Get all shop IDs from the DTO
            var shopIds = updateRolesDto.ShopRoles.Select(r => r.ShopId).ToList();

            // Remove user from shops not in the update list
            var shopsToRemove = user.UserShops.Where(us => !shopIds.Contains(us.ShopId)).ToList();
            foreach (var shopToRemove in shopsToRemove)
            {
                user.UserShops.Remove(shopToRemove);
            }

            // Update roles for existing shops or add new ones
            foreach (var shopRole in updateRolesDto.ShopRoles)
            {
                var existingUserShop = user.UserShops.FirstOrDefault(us => us.ShopId == shopRole.ShopId);
                if (existingUserShop != null)
                {
                    // Update role
                    existingUserShop.Role = shopRole.Role;
                }
                else
                {
                    // Add new UserShop
                    user.UserShops.Add(new UserShop
                    {
                        UserId = user.Id,
                        ShopId = shopRole.ShopId,
                        Role = shopRole.Role
                    });
                }
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
