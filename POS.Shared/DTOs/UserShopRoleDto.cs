using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_API.Enum.Enums;

namespace POS.Shared.DTOs
{
    public class UserShopRoleDto
    {
        [Required]
        public int ShopId { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }
    }
}
