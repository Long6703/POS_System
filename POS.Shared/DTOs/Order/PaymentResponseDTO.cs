using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs.Order
{
    public class PaymentResponseDTO
    {
        public int Id { get; set; }
        public int PaymentMethod { get; set; }
        public string PaymentMethodName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
