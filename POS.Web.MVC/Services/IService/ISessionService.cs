namespace POS.Web.MVC.Services.IService
{
    public interface ISessionService
    {
        Task SetStringAsync(string key, string value);
        Task<string?> GetStringAsync(string key);
        Task SetObjectAsync<T>(string key, T value);
        Task<T?> GetObjectAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}
