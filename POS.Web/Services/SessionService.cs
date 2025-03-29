using POS.Web.Services.IService;
using System.Text.Json;

namespace POS.Web.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetString(string key, string value)
        {
            _httpContextAccessor.HttpContext?.Session.SetString(key, value);
        }

        public string? GetString(string key)
        {
            return _httpContextAccessor.HttpContext?.Session.GetString(key);
        }

        public void SetObject<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            _httpContextAccessor.HttpContext?.Session.SetString(key, json);
        }

        public T? GetObject<T>(string key)
        {
            var json = _httpContextAccessor.HttpContext?.Session.GetString(key);
            return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
        }

        public void Remove(string key)
        {
            _httpContextAccessor.HttpContext?.Session.Remove(key);
        }
    }
}
