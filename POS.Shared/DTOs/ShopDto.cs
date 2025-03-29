namespace POS.Shared.DTOs
{

    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string EmailShopOwner { get; set; }
        public bool isDeleted { get; set; }
    }
}
