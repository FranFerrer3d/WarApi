namespace WarApi.Services.Security
{
    public interface IEncryptionService
    {
        string Encrypt(string plain);
        string Decrypt(string cipher);
    }
}
