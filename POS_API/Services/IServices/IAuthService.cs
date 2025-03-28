using POS.Shared.RequestModel;

namespace POS_API.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Login(LoginRequest request);
        Task<bool> Register(RegisterRequest register);
    }
}
