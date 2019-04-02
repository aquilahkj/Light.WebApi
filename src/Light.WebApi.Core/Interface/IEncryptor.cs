namespace Light.WebApi.Core
{
    public interface IEncryptor
    {
        string Encrypt(string content);

        string Decrypt(string content);
    }
}