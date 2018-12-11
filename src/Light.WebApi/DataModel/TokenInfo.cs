namespace Light.WebApi
{
    public class TokenInfo
    {
        private readonly string userId;

        public string UserId {
            get {
                return userId;
            }
        }

        private readonly string client;

        public string Client {
            get {
                return client;
            }
        }

        private readonly string guid;

        public string Guid {
            get {
                return guid;
            }
        }

        public TokenInfo(string userId, string client, string guid)
        {
            this.userId = userId;
            this.client = client;
            this.guid = guid;
        }
    }
}