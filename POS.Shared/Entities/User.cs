using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Shared.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public bool IsAdmin { get; set; }

        public bool isDeleted { get; set; } = false;

        public virtual ICollection<UserShop> UserShops { get; set; } = new List<UserShop>();
    }
}
