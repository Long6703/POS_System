using POS.Shared.RequestModel;

namespace POS.Web.MVC.Services.IService
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequest request);

    }
}