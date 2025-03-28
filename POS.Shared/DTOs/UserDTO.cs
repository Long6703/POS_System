using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public bool isDeleted { get; set; } = false;
        public List<UserShopDTO> UserShops { get; set; } = new();

    }
}
