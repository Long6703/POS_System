using POS.Web.Exceptions;
using POS.Web.Services.IService;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;

namespace POS.Web.Services
{
    public class BaseApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionService _sessionService;

        public BaseApiService(HttpClient httpClient, ISessionService sessionService)
        {
            _httpClient = httpClient;
            _sessionService = sessionService;
        }

        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }

        public async Task<T?> PostAsync<TRequest, T>(string url, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data, _jsonOptions);
            return await HandleResponse<T>(response);
        }

        public async Task<T?> PutAsync<TRequest, T>(string url, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data, _jsonOptions);
            return await HandleResponse<T>(response);
        }

        public async Task<bool> DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        private async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            string? token = _sessionService.GetString("AuthToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new ApiException(response.StatusCode, errorContent);
        }
    }
}
