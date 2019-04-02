namespace Light.WebApi.Core
{
    public interface IAuthorizeManagement
    {
        string CreateUserToken(AccountAuthorizeInfo info);
        AccountAuthorizeInfo GetAuthorize(TokenInfo info);
        string GetSystemClientId(string token);
        TokenInfo ParseUserToken(string token);
        void RemoveAuthorize(string client, string userId);
        void SetAuthorize(AccountAuthorizeInfo info);
        bool TestMode { get; }
    }
}