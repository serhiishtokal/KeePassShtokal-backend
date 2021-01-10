using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace KeePassShtokal.AppCore.Helpers
{
    public static class SymmetricEncryptor
    {
        public static string EncryptString(string toEncrypt, string password)
        {
            var key = GetKey(password);

            using var aes = Aes.Create();
            using var encryptor = aes.CreateEncryptor(key, key);
            var plainText = Encoding.UTF8.GetBytes(toEncrypt);
            return Convert.ToBase64String(encryptor.TransformFinalBlock(plainText, 0, plainText.Length).ToArray());
        }

        public static string DecryptToString(string encryptedData, string password)
        {
            var key = GetKey(password);

            using var aes = Aes.Create();
            using var encryptor = aes.CreateDecryptor(key, key);
            var plainText = Convert.FromBase64String(encryptedData);
            var decryptedBytes = encryptor
                .TransformFinalBlock(plainText, 0, plainText.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        private static byte[] GetKey(string password)
        {
            var keyBytes = Encoding.UTF8.GetBytes(password);
            using var md5 = MD5.Create();
            return md5.ComputeHash(keyBytes);
        }
    }
}
