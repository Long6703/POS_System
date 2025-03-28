using POS.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using static POS_API.Enum.Enums;

namespace POS.Shared.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public Guid OrderId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }

        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }

        public virtual Order Order { get; set; }
    }
}
