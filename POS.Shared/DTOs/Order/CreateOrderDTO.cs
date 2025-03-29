using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Order
{
    public class CreateOrderDTO
    {
        public string? CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public int ShopId { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; } = new List<OrderDetailDTO>();
    }
}
