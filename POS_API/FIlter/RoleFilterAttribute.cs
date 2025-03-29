using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Collections.Generic;
using static POS_API.Enum.Enums;
using POS.Shared.Extensions;

namespace POS_API.FIlter
{
    public class RoleFilterAttribute : ActionFilterAttribute
    {
        private readonly List<UserRole> _roles;

        public RoleFilterAttribute(params UserRole[] roles)
        {
            _roles = new List<UserRole>(roles);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var roles = jwtToken.Claims.FirstOrDefault(c => c.Type == "ShopId-Role")?.Value;

            if (roles == null || context.RouteData.Values["shopid"] == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var data = roles.ParseRoles();
            var shopId = int.Parse(context.RouteData.Values["shopid"].ToString());
            var role = data?.FirstOrDefault(sr => sr.ShopId == shopId)?.Role;

            if (role == null || !System.Enum.TryParse(typeof(UserRole), role, out var parsedRole) || !_roles.Contains((UserRole)parsedRole))
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
