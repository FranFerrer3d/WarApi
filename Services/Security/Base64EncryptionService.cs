using System.Text;

namespace WarApi.Services.Security
{
    public class Base64EncryptionService : IEncryptionService
    {
        public string Encrypt(string plain)
        {
            if (string.IsNullOrEmpty(plain)) return plain;
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
        }

        public string Decrypt(string cipher)
        {
            if (string.IsNullOrEmpty(cipher)) return cipher;
            return Encoding.UTF8.GetString(Convert.FromBase64String(cipher));
        }
    }
}
