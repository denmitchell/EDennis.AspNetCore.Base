using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This middleware will transform headers into user claims per configuration settings.
    /// 
    /// This middleware is designed to be used immediately BEFORE or AFTER UseAuthentication() or
    /// UseAuthorization().  All header/claims configured for PostAuthentication will be ignored if
    /// the User is not authenticated.
    /// 
    /// When used after UseAuthorization, the claims are merely extra claims that can be used for
    /// purposes other than all-or-nothing access to a protected resource (e.g., using a claim's
    /// value to filter data requests.)
    /// </summary>
    public class ApplicationPropertiesMiddleware {
        private readonly RequestDelegate _next;
        private readonly ApplicationProperties _applicationProperties;
        private static bool _propertiesSet = false;
         
        public ApplicationPropertiesMiddleware(RequestDelegate next,
            ApplicationProperties applicationProperties) {
            _next = next;
            _applicationProperties = applicationProperties;
        }

        public async Task InvokeAsync(HttpContext context) {

            var req = context.Request;

            if(_propertiesSet)
                await _next(context);
            else if (req.Path.StartsWithSegments(new PathString("/swagger"))) {
                _applicationProperties.EntryPoint = EntryPoint.Swagger;       
                await _next(context);
            } else {
                if (req.Headers.TryGetValue("X-EntryPoint", out StringValues strEntryPoint))
                    _applicationProperties.EntryPoint = Enum.Parse<EntryPoint>(strEntryPoint[0]);

                _applicationProperties.IsChild = req.Headers.ContainsKey("X-RequestFrom");
                _propertiesSet = true;
                
                await _next(context);
            }
        }
    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseApplicationProperties(this IApplicationBuilder app) {
            app.UseMiddleware<ApplicationPropertiesMiddleware>();
            return app;
        }
    }

}
