using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {

    public class ScopePropertiesMiddleware {

        private readonly RequestDelegate _next;
        private readonly ScopePropertiesOptions _options;

        public ScopePropertiesMiddleware(RequestDelegate next, IOptions<ScopePropertiesOptions> options) {
            _next = next;
            _options = options?.Value ?? new ScopePropertiesOptions();
        }

        public async Task InvokeAsync(HttpContext context/*, IServiceProvider provider*/) {

            

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {
                var scopeProperties = context.RequestServices.GetRequiredService<IScopeProperties>();

                //var scopeProperties = provider.GetRequiredService(typeof(ScopeProperties22)) as ScopeProperties22;

                //set User property
                scopeProperties.User = _options.UserSource switch
                {
                    UserSource.CLAIMS_PRINCIPAL_IDENTITY_NAME => context.User?.Identity?.Name,
                    UserSource.NAME_CLAIM => GetClaimValue(context, JwtClaimTypes.Name),
                    UserSource.PREFERRED_USERNAME_CLAIM => GetClaimValue(context, JwtClaimTypes.PreferredUserName),
                    UserSource.SUBJECT_CLAIM => GetClaimValue(context, JwtClaimTypes.Subject),
                    UserSource.EMAIL_CLAIM => GetClaimValue(context, IdentityServerConstants.StandardScopes.Email),
                    UserSource.PHONE_CLAIM => GetClaimValue(context, IdentityServerConstants.StandardScopes.Phone),
                    UserSource.CLIENT_ID_CLAIM => GetClaimValue(context, JwtClaimTypes.ClientId),
                    UserSource.CUSTOM_CLAIM => GetClaimValue(context, _options.UserSourceClaimType),
                    UserSource.SESSION_ID => context.Session?.Id,
                    UserSource.X_USER_HEADER => GetHeaderValue(context,"X-User"),
                    _ => null
                };

                context.Request.Headers
                    .Where(h=>Regex.IsMatch(h.Key,_options.StoreHeadersWithPattern, RegexOptions.IgnoreCase))
                    .ToList()
                    .ForEach(h => scopeProperties.Headers
                    .Add(h.Key, h.Value.ToArray()));

                if (_options.AppendHostPath)
                        scopeProperties.Headers.Add("X-HostPath",
                            $"{context.Request.Headers["X-HostPath"].ToString()}>{context.Request.Headers["Host"]}");

                if (context?.User?.Claims != null) {
                    scopeProperties.Claims = context.User.Claims
                        .Where(c => Regex.IsMatch(c.Type, _options.StoreClaimTypesWithPattern, RegexOptions.IgnoreCase))
                        .ToArray();
                    var testConfigClaim = context.User.Claims.FirstOrDefault(c => c.Type == "X-TestConfig");
                    if (testConfigClaim != null) {
                        scopeProperties.TestConfig = new TestConfigParser().Parse(testConfigClaim.Value);
                        scopeProperties.ActiveProfile = scopeProperties.TestConfig.ProfileName;
                    }
                }


                if (context.Request.Headers.ContainsKey("X-TestConfig")) {
                    scopeProperties.TestConfig = new TestConfigParser().Parse(context.Request.Headers["X-TestConfig"]);
                    scopeProperties.ActiveProfile = scopeProperties.TestConfig.ProfileName;
                }

            }
            await _next(context);

        }


        private string GetClaimValue(HttpContext context, string claimType)
            => context.User?.Claims?.FirstOrDefault(x 
                => x.Type.Equals(claimType,StringComparison.OrdinalIgnoreCase))?.Value;


        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
                => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();


    }

    public static class IApplicationBuilderExtensionsForScopePropertiesMiddleware {
        public static IApplicationBuilder UseScopeProperties(this IApplicationBuilder app, IOptions<ScopePropertiesOptions> options) {
            app.UseMiddleware<ScopePropertiesMiddleware>(options);
            return app;
        }
    }


}