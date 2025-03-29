namespace POS_API.Enum
{
    public class Enums
    {
        public enum UserRole
        {
            ShopOwner,
            Manager,
            Staff
        }

        public enum OrderStatus
        {
            Pending,
            Paid,
            Cancelled
        }

        public enum PaymentMethod
        {
            Cash,
            QRCode,
            Card
        }
    }
}
