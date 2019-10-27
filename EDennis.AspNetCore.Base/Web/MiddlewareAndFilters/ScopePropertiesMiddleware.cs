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

    /// <summary>
    /// This middleware is designed to retrieve and store important data for later processing by
    /// ApiClients, Repos, and perhaps other classes.  In particular, the middleware retrieves
    /// and stores:
    /// <list type="bullet">
    /// <item>User name from any of a variety of sources</item>
    /// <item>An "spi:" prefixed scope that determines the Active Profile from Configuration</item>
    /// <item>Other Claims that match a prespecified pattern</item>
    /// <item>Other Headers that match a prespecified pattern</item>
    /// </list>
    /// </summary>
    public class ScopePropertiesMiddleware {

        private readonly RequestDelegate _next;
        private readonly ScopePropertiesOptions _options;


        public ScopePropertiesMiddleware(RequestDelegate next, IOptions<ScopePropertiesOptions> options) {
            _next = next;
            _options = options?.Value ?? new ScopePropertiesOptions();
        }

        public async Task InvokeAsync(HttpContext context,
            IOptionsMonitor<AppSettings> appSettings,
            IOptionsMonitor<Profiles> profiles,
            IOptionsMonitor<Apis> apis,
            IOptionsMonitor<ConnectionStrings> connectionStrings,
            IOptionsMonitor<MockClients> mockClients,
            IOptionsMonitor<AutoLogins> autoLogins) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var activeProfileName = "Default";

                //get active profile name from launch profile setting, when provided
                if (string.IsNullOrWhiteSpace(appSettings.CurrentValue.LaunchProfile))
                    activeProfileName = appSettings.CurrentValue.LaunchProfile;

                //get a reference to scope properties
                var scopeProperties = context.RequestServices.GetRequiredService<IScopeProperties>();

                //update the Scope Properties User with identity, claim or header data
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

                //copy headers to ScopeProperties headers, based upon the provided match expession
                context.Request.Headers
                    .Where(h=>Regex.IsMatch(h.Key,_options.StoreHeadersWithPattern, RegexOptions.IgnoreCase))
                    .ToList()
                    .ForEach(h => scopeProperties.Headers
                    .Add(h.Key, h.Value.ToArray()));

                //append the host path to a ScopeProperties header, if configured 
                if (_options.AppendHostPath)
                        scopeProperties.Headers.Add(ScopePropertiesOptions.HOSTPATH_HEADER,
                            $"{context.Request.Headers[ScopePropertiesOptions.HOSTPATH_HEADER].ToString()}>" +
                            $"{context.Request.Headers["Host"]}");


                //add user claims, if configured
                if (context?.User?.Claims != null) {
                    scopeProperties.Claims = context.User.Claims
                        .Where(c => Regex.IsMatch(c.Type, _options.StoreClaimTypesWithPattern, RegexOptions.IgnoreCase))
                        .ToArray();
                    //update the test config claim and ActiveProfile, if needed
                    var testConfigClaim = context.User.Claims.FirstOrDefault(c => c.Type == Instruction.HEADER);
                    if (testConfigClaim != null) {
                        scopeProperties.Instruction = new InstructionParser().Parse(testConfigClaim.Value);
                        activeProfileName = scopeProperties.Instruction.ProfileName;
                    }
                }


                //update the test config claim and ActiveProfile, if needed
                if (context.Request.Headers.ContainsKey(Instruction.HEADER)) {
                    scopeProperties.Instruction = new InstructionParser().Parse(context.Request.Headers[Instruction.HEADER]);
                    activeProfileName = scopeProperties.Instruction.ProfileName;
                }

                scopeProperties.ActiveProfile = new ResolvedProfile();
                scopeProperties.ActiveProfile.Load(activeProfileName, 
                    profiles?.CurrentValue[activeProfileName], 
                    apis?.CurrentValue, connectionStrings?.CurrentValue, 
                    mockClients?.CurrentValue, autoLogins?.CurrentValue);

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