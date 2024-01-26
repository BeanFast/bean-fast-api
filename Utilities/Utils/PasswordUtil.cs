namespace Utilities.Utils
{
    public static class PasswordUtil
    {
        public static string HashPassword(string rawPassword)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(13);
            return BCrypt.Net.BCrypt.HashPassword(rawPassword, salt);
        }
        public static bool VerifyPassword(string rawPassword, string hashPassword)
        {
            Console.WriteLine(rawPassword);
            Console.WriteLine(hashPassword);
            return BCrypt.Net.BCrypt.Verify(text: rawPassword, hash: hashPassword);
        }
    }
}
