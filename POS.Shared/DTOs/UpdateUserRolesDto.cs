using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs
{
    public class UpdateUserRolesDto
    {
        [Required]
        public List<UserShopRoleDto> ShopRoles { get; set; } = new List<UserShopRoleDto>();
    }
}
