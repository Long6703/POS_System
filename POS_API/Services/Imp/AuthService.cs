using AutoMapper;
using eStoreAPI.Helper;
using POS.Shared.DTOs;
using POS.Shared.RequestModel;
using POS_API.Repository.IRepository;
using POS_API.Services.IServices;

namespace POS_API.Services.Imp
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtHelper _jwt;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IAuthRepository authRepository,JwtHelper jwt, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _authRepository = authRepository;
            _jwt = jwt;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Login(LoginRequest request)
        {
            var user = await _authRepository.Login(request);
            if (user == null || !HashingService.VerifyHash(request.Password, user.PasswordHash))
            {
                return string.Empty;
            }
            var userDto = _mapper.Map<UserDTO>(user);

            userDto.UserShops = user.UserShops.Select(us => new UserShopDTO
            {
                UserId = us.UserId,
                ShopId = us.ShopId,
                Role = us.Role.ToString()
            }).ToList();

            var token = _jwt.GenerateJwtToken(userDto);
            return token;
        }

        public Task<bool> Register(RegisterRequest register)
        {
            throw new NotImplementedException();
        }
    }
}
