using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
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

                context.Request.Headers.TryGetValue("User", out StringValues userHeader);
                var userName = context.User.Identity.Name;

                //try to get user name from header first
                if (scopeProperties.User == null && userHeader.Count == 1) {
                    scopeProperties.User = userHeader[0];

                //next, try to get user name from user principal
                } else if (userName != null && userName != "") {
                    scopeProperties.User = userName;

                //finally, try to get user name from user claims
                } else {
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