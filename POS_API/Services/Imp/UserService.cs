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
        private readonly IEmailService _emailService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<UserDTO> CreateUserAsync(CreateUserDto createUserDto)
        {
            var existingUser = (await _unitOfWork.Users.FindAsync(u => u.Email == createUserDto.Email && !u.isDeleted)).FirstOrDefault();
            if (existingUser != null)
            {
                return null;
            }

            var user = _mapper.Map<User>(createUserDto);
            string password = GenerateRandomString(8);
            user.PasswordHash = HashingService.Hash(password);

            string emailContent = $@"
                <p>Hello,</p>
                <p>Your account has been successfully created.</p>
                <p><strong>Email:</strong> {user.Email}</p>
                <p><strong>Password:</strong> {password}</p>
                <p>Please log in and change your password.</p>";

            await _emailService.QueueEmailAsync(
                user.Email,
                "Account Information",
                emailContent,
                true
            );

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = (await _unitOfWork.Users.FindAsync(u => u.Id == id && !u.isDeleted)).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<UserDTO> GetUserByIdAsync(Guid id)
        {
            var user = (await _unitOfWork.Users.GetUserByIdWithShopAsync(id));
            if (user == null)
            {
                return null;
            }

            return user;
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


        public async Task<bool> UpdateUserRolesAsync(UpdateUserRolesDto updateRolesDto)
        {
            var result = await _unitOfWork.Users.UpdateRoleInShop(updateRolesDto);
            if (!result)
            {
                return false;
            }
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}
