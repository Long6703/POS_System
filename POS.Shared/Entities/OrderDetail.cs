using POS.Shared.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Shared.Entities
{
    public class OrderDetail
    {
        [Key]
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public int ProductId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
