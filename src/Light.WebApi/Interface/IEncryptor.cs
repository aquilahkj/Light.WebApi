namespace Light.WebApi
{
    internal interface IEncryptor
    {
        string Encrypt(string content);

        string Decrypt(string content);
    }
}