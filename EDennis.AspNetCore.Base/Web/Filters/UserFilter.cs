using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {

    public class UserFilter {

        RequestDelegate _next;

        public UserFilter(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties)) as ScopeProperties;
                var userName = context.User.Identity.Name;
                if (userName != null && userName != "")
                    scopeProperties.User = userName;
                else {
                    scopeProperties.User = context.User.Claims.Where(x =>
                        x.Type == "name" ||
                        x.Type == "client_name" ||
                        x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                        .FirstOrDefault()?.Value;
                }
            }
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