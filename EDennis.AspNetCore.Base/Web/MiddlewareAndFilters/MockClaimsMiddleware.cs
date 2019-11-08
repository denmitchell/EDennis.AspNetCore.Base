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
            var mockClaims = _appSettings.CurrentValue.MockClaims;
            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                if (context.User != null) {
                    var claims = new List<Claim>();
                    if (mockClaims != null && mockClaims != null)
                        foreach (var claim in mockClaims)
                            claims.Add(new Claim(claim.Type, claim.Value));

                    var appIdentity = new ClaimsIdentity(claims);
                    context.User.AddIdentity(appIdentity);
                }
            }
            await _next(context);
        }
    }
}
