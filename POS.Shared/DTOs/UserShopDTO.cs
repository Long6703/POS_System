using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_API.Enum.Enums;

namespace POS.Shared.DTOs
{
    public class UserShopDTO
    {
        public Guid UserId { get; set; }
        public int ShopId { get; set; }
        public UserRole Role { get; set; }
    }
}
