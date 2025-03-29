using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Shared.DTOs
{
    public class UpdateUserDto
    {
        [Required(ErrorMessage = "Tên người dùng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên người dùng không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}
