using System.ComponentModel.DataAnnotations.Schema;
using static POS_API.Enum.Enums;

namespace POS.Shared.Entities
{
    public class UserShop
    {

        public Guid UserId { get; set; }
        public int ShopId { get; set; }

        public UserRole Role { get; set; }

        public virtual User User { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
