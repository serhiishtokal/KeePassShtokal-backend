using System.Security.Cryptography;
using System.Text;

namespace KeePassShtokal.AppCore.Helpers
{
    public static class HashHelper
    {
        public static string Sha512(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashString = Encoding.UTF8.GetString(hashedInputBytes);
            return hashString;
        }

        public static string HmacSha512(string input, string secretKey)
        {
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var inputBytes = Encoding.UTF8.GetBytes(input);

            using var hmac = new HMACSHA512(secretKeyBytes);
            var hashValue = hmac.ComputeHash(inputBytes);
            var hashString = Encoding.UTF8.GetString(hashValue);
            return hashString;
        }
    }
}
