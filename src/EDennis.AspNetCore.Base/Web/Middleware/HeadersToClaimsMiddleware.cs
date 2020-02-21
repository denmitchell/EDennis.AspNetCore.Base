using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;

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
    public class HeadersToClaimsMiddleware {
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<HeadersToClaims> _settings;

        public HeadersToClaimsMiddleware(RequestDelegate next,
            IOptionsMonitor<HeadersToClaims> settings) {
            _next = next;
            _settings = settings;
        }

        public async Task InvokeAsync(HttpContext context) {

            var req = context.Request;
            var enabled = _settings.CurrentValue.Count > 0 ;

            if (!enabled || req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {


                if (context.User != null) {

                    var htc = _settings.CurrentValue;
                    var claims = new List<Claim>();
                    bool claimsAdded = false;

                    if (context.User != null) {
                        if (htc != null) {
                            foreach (var key in htc.Keys) {
                                foreach (var hdr in req.Headers.Where(h => h.Key == key)) {
                                    foreach (var val in hdr.Value.ToArray()) {
                                        Debug.WriteLine($"{this.GetHashCode()}: {htc[key]},{val}");

                                        claims.Add(new Claim(htc[key], val));
                                        claimsAdded = true;
                                    }
                                }
                            }
                        }
                    }

                    //if any claims were added, add a new claims identity with those claims
                    if (claimsAdded) {
                        var appIdentity = new ClaimsIdentity(claims);
                        context.User.AddIdentity(appIdentity);
                    }
                }
                await _next(context);
            }
        }
    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseHeadersToClaims(this IApplicationBuilder app) {
            app.UseMiddleware<HeadersToClaimsMiddleware>();
            return app;
        }
    }

}
