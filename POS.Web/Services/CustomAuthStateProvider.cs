using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace POS.Web.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("AuthToken");

            if (string.IsNullOrEmpty(token))
            {
                var anonymous = new ClaimsIdentity();
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
            }

            var claims = ParseClaimsFromToken(token);
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public void SetAuthToken(string token)
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            _httpContextAccessor.HttpContext.Session.SetString("AuthToken", token);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void ClearAuthToken()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("AuthToken");
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken.Claims;
        }
    }
}
