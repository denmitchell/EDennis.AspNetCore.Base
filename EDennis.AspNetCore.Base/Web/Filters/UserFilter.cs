using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;


namespace ExploringEFHistory.Web {

    public class UserFilter {

        RequestDelegate _next;

        public UserFilter(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider) {

            var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties)) as ScopeProperties;
            scopeProperties.User = context.User.Identity.Name;

            await _next(context);

        }
    }

    public static class IApplicationBuilderExtensionsForUserFilter {
        public static IApplicationBuilder UseUser(this IApplicationBuilder app) {
            app.UseMiddleware<UserFilter>();
            return app;
        }
    }


}