using System;
using Light.WebApi.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddException(this IServiceCollection services, Action<ExceptionOptionsBuilder> action = null)
        {
            var builder = new ExceptionOptionsBuilder();
            action?.Invoke(builder);
            var options = builder.Build();
            var management = new ExceptionManagement(options);
            services.AddSingleton<IExceptionManagement>(management);
            services.AddMvc(x => {
                x.Filters.Add<ExceptionFilter>();
            });
            return services;
        }

        public static IServiceCollection AddAuthorize(this IServiceCollection services, Action<AuthorizeOptionsBuilder> action = null)
        {
            var builder = new AuthorizeOptionsBuilder();
            action?.Invoke(builder);
            var options = builder.Build();
            if (options.AuthorizeData == null) {
                var admin = new AdminUser(0, "admin", "Spuer Admin", "admin");
                options.AuthorizeData = new BasicAuthorizeData(new AdminUser[] { admin });
            }
            services.AddSingleton(options);
            services.AddSingleton<IAuthorizeManagement, AuthorizeManagement>();
            services.AddMvc(x => {
                x.Filters.Add<AuthorizeFilter>();
            });
            return services;
        }

        public static AuthorizeOptionsBuilder UseBasicAuthorizeData(this AuthorizeOptionsBuilder builder, string account, string password, string userName = null)
        {
            if (string.IsNullOrEmpty(account)) {
                throw new ArgumentException("value is null", nameof(account));
            }

            if (string.IsNullOrEmpty(password)) {
                throw new ArgumentException("value is null", nameof(password));
            }

            var admin = new AdminUser(0, account, userName ?? account, password);
            IAuthorizeData authorizeData = new BasicAuthorizeData(new AdminUser[] { admin });
            builder.SetAuthorizeData(authorizeData);
            return builder;
        }

       

        //public static IServiceCollection AddAuthorize<T>(this IServiceCollection services, Action<AuthorizeOptionsBuilder> action = null) where T : class, IAuthorizeManagement, new()
        //{
        //    var builder = new AuthorizeOptionsBuilder();
        //    action?.Invoke(builder);
        //    var options = builder.Build();
        //    services.AddSingleton(options);
        //    services.AddSingleton<IAuthorizeManagement, T>();
        //    services.AddMvc(x => {
        //        x.Filters.Add<AuthorizeFilter>();
        //    });
        //    return services;
        //}
    }
}
