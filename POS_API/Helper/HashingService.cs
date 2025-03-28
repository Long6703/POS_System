namespace POS_API.Services
{
    public static class HashingService
    {
        public static string Hash(string input, int workFactor = 12)
        {
            return BCrypt.Net.BCrypt.HashPassword(input, workFactor);
        }

        public static bool VerifyHash(string input, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashed);
        }
    }
}
