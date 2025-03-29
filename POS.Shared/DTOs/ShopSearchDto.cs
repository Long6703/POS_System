namespace POS.Shared.DTOs
{
    public class ShopSearchDto : BaseSearchDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
    }
}
