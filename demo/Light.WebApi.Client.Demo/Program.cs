using System;
using Light.WebApi.Client.Demo.Model;

namespace Light.WebApi.Client.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            ApiClient client = new ApiClient("http://localhost:5001/");
            Console.WriteLine("entry password:");
            string password = Console.ReadLine();
            LoginModel loginModel = new LoginModel() {
                Account = "admin",
                Password = password,
                Client = "cmt"
            };

            string token = null;
            try {
                var result = client.PostAndGetSingleAsync<LoginResultModel, LoginModel>("api/account/login", loginModel).Result;
                token = result.Token;
                Console.WriteLine(result.Token);
            }
            catch (AggregateException ex) {
                if (ex.InnerException is ApiException apiException) {
                    Console.WriteLine(apiException.ToApiMessage());
                }
                else {
                    Console.WriteLine(ex);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.ReadLine();

            client.SetToken(token);
            try {
                var result = client.PostAndGetListAsync<CategoryModel>("api/category/query").Result;
                foreach (var item in result) {
                    Console.WriteLine($"code:{item.CategoryCode},name:{item.CategoryName}");
                }
            }
            catch (AggregateException ex) {
                if (ex.InnerException is ApiException apiException) {
                    Console.WriteLine(apiException.ToApiMessage());
                }
                else {
                    Console.WriteLine(ex);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
