using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class MockClaimsMiddleware {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<AppSettings> _appSettings;
        public MockClaimsMiddleware(RequestDelegate next, IOptionsMonitor<AppSettings> appSettings) {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context) {
            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                if (context.User != null) {
                    var mockClaims = _appSettings.CurrentValue.MockClaims;
                    if (mockClaims != null) {
                        var claims = new List<Claim>();
                        foreach (var claim in mockClaims)
                            claims.Add(new Claim(claim.Type, claim.Value));

                        var appIdentity = new ClaimsIdentity(claims);
                        context.User.AddIdentity(appIdentity);
                    }
                }
            }
            await _next(context);
        }
    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseMockClaims(this IApplicationBuilder app){
            app.UseMiddleware<MockClaimsMiddleware>();
            return app;
        }
    }

}
