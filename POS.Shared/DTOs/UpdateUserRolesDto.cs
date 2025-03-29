using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static POS_API.Enum.Enums;

namespace POS.Shared.DTOs
{
    public class UpdateUserRolesDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ShopId { get; set; }
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserRole Role { get; set; }
    }
}
