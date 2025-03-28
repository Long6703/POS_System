using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_API.Enum.Enums;

namespace POS.Shared.RequestModel
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public int ShopId { get; set; }
        public UserRole role { get; set; }
    }
}
