using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using POS.Web.Services.IService;
using System.Text.Json;

namespace POS.Web.Services
{
    public class SessionService : ISessionService
    {
        private readonly ProtectedSessionStorage _sessionStorage;

        public SessionService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task SetStringAsync(string key, string value)
        {
            await _sessionStorage.SetAsync(key, value);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            var result = await _sessionStorage.GetAsync<string>(key);
            return result.Success ? result.Value : null;
        }

        public async Task SetObjectAsync<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            await _sessionStorage.SetAsync(key, json);
        }

        public async Task<T?> GetObjectAsync<T>(string key)
        {
            var result = await _sessionStorage.GetAsync<string>(key);
            return result.Success ? JsonSerializer.Deserialize<T>(result.Value!) : default;
        }

        public async Task RemoveAsync(string key)
        {
            await _sessionStorage.DeleteAsync(key);
        }
    }

}
