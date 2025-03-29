using System.ComponentModel.DataAnnotations;

namespace POS.Shared
{
    public class UpdateShopDto
    {
        [Required(ErrorMessage = "Input required")]
        [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Input required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }
    }
}
