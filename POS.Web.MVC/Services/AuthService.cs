using POS.Web.MVC.Services.IService;
using System.Threading.Tasks;
using POS.Shared.ResponseModel;
using POS.Shared.RequestModel;

namespace POS.Web.MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly BaseApiService _baseApiService;

        public AuthService(BaseApiService baseApiService)
        {
            _baseApiService = baseApiService;
        }

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            var response = await _baseApiService.PostAsync<LoginRequest, LoginResponse>("auth/login", request);
            return response?.Token;
        }
    }
}
