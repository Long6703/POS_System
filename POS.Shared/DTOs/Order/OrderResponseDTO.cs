using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Order
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailResponseDTO> OrderDetails { get; set; } = new List<OrderDetailResponseDTO>();
        public List<PaymentResponseDTO> Payments { get; set; } = new List<PaymentResponseDTO>();
    }
}
