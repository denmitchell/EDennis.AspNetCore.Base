using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This middleware keeps track of the entry point for an application
    /// and whether the application is a child API of another application
    /// </summary>
    public class ApplicationPropertiesMiddleware {
        private readonly RequestDelegate _next;
        private readonly ApplicationProperties _applicationProperties;
        private bool _propertiesSet = false;
         
        public ApplicationPropertiesMiddleware(RequestDelegate next,
            ApplicationProperties applicationProperties) {
            _next = next;
            _applicationProperties = applicationProperties;
        }

        public async Task InvokeAsync(HttpContext context) {

            var req = context.Request;

            Debug.WriteLine($"_propertiesSet: {_propertiesSet}");
            Debug.WriteLine($"Request.Headers['X-EntryPoint']: {req.Headers["X-EntryPoint"]}");

            if (_propertiesSet)
                await _next(context);
            else if (req.Path.StartsWithSegments(new PathString("/swagger"))) {
                _applicationProperties.EntryPoint = EntryPoint.Swagger;       
                await _next(context);
            } else {
                if (req.Headers.TryGetValue(Constants.ENTRYPOINT_KEY, out StringValues strEntryPoint))
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
