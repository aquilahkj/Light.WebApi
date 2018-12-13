using System;
namespace Light.WebApi.Client.Demo.Model
{
    public class LoginModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Client { get; set; }
    }

    public class LoginResultModel
    {
        public string Token { get; set; }
    }
}
