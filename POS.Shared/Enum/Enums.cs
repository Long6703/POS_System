namespace POS_API.Enum
{
    public class Enums
    {
        public enum UserRole
        {
            ShopOwner = 0,
            Manager = 1,
            Staff = 2
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
