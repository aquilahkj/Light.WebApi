namespace Light.WebApi.Core
{
    internal interface IEncryptor
    {
        string Encrypt(string content);

        string Decrypt(string content);
    }
}