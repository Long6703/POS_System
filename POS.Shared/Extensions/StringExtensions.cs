using POS.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.Extensions
{
    public static class StringExtensions
    {
        public static List<UserShopDTO> ParseRoles(this string roles)
        {
            var shopRoles = new List<UserShopDTO>();
            var rolePairs = roles.Split(',');

            foreach (var rolePair in rolePairs)
            {
                var parts = rolePair.Split('-');
                if (parts.Length == 2)
                {
                    shopRoles.Add(new UserShopDTO
                    {
                        ShopId = Int32.Parse(parts[0]),
                        Role = parts[1]
                    });
                }
            }

            return shopRoles;
        }
    }
}
