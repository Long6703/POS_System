using POS.Web.MVC.Services.IService;
using System.Text.Json;

namespace POS.Web.MVC.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext?.Session
                                    ?? throw new InvalidOperationException("Session is not available.");

        public Task SetStringAsync(string key, string value)
        {
            Session.SetString(key, value);
            return Task.CompletedTask;
        }

        public Task<string?> GetStringAsync(string key)
        {
            return Task.FromResult(Session.GetString(key));
        }

        public Task SetObjectAsync<T>(string key, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            Session.SetString(key, jsonData);
            return Task.CompletedTask;
        }

        public Task<T?> GetObjectAsync<T>(string key)
        {
            var jsonData = Session.GetString(key);
            return jsonData is not null
                ? Task.FromResult(JsonSerializer.Deserialize<T>(jsonData))
                : Task.FromResult<T?>(default);
        }

        public Task RemoveAsync(string key)
        {
            Session.Remove(key);
            return Task.CompletedTask;
        }
    }
}
