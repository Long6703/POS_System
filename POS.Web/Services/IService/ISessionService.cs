namespace POS.Web.Services.IService
{
    public interface ISessionService
    {
        void SetString(string key, string value);
        string? GetString(string key);
        void SetObject<T>(string key, T value);
        T? GetObject<T>(string key);
        void Remove(string key);
    }
}
