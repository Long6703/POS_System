using Microsoft.IdentityModel.Tokens;
using POS.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eStoreAPI.Helper
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJwtToken(UserDTO userDTO)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString()),
                new Claim(ClaimTypes.Name, userDTO.Name),
                new Claim(ClaimTypes.Email, userDTO.Email),
                new Claim(ClaimTypes.Role, userDTO.IsAdmin ? "Admin" : "User")
            };

            if (userDTO.UserShops.Any())
            {
                foreach (var userShop in userDTO.UserShops)
                {
                    claims.Add(new Claim("ShopId", userShop.ShopId.ToString()));
                    claims.Add(new Claim("UserRole", userShop.Role.ToString()));
                }
            }

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

