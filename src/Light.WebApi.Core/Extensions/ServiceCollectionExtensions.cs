using System;
using Microsoft.Extensions.DependencyInjection;

namespace Light.WebApi.Core
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
            var management = new AuthorizeManagement(options);
            services.AddSingleton<IAuthorizeManagement>(management);
            services.AddSingleton<IPermissionManagement, PermissionManagement>();
            Type permissionType = builder.GetPermissionType();
            if (permissionType != null) {
                services.AddSingleton(typeof(IPermissionModule), permissionType);
            }
            services.AddMvc(x => {
                x.Filters.Add<AuthorizeFilter>();
            });
            return services;
        }
    }
}
