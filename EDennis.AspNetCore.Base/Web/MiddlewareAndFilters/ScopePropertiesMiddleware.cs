using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
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
        private readonly Profiles _profiles;

        public ScopePropertiesMiddleware(RequestDelegate next, 
            IOptionsMonitor<ScopePropertiesOptions> options,
            IOptionsMonitor<Profiles> profiles
            ) {
            _next = next;
            _options = options?.CurrentValue ?? new ScopePropertiesOptions();
            _profiles = profiles.CurrentValue;
            if (!_profiles.NamesUpdated)
                _profiles.UpdateNames();
        }

        public async Task InvokeAsync(HttpContext context, IScopeProperties scopeProperties) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var activeProfile = scopeProperties.ActiveProfile;

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
                    UserSource.X_USER_HEADER => GetHeaderValue(context,ScopePropertiesOptions.USER_HEADER),
                    _ => null
                };



                //get host/protocol information
                scopeProperties.Host = context.Request.Host.Host;
                scopeProperties.Port = context.Request.Host.Port.Value;
                scopeProperties.Scheme = context.Request.Scheme;


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
                if (context.User?.Claims != null) {
                    scopeProperties.Claims = context.User.Claims
                        .Where(c => Regex.IsMatch(c.Type, _options.StoreClaimTypesWithPattern, RegexOptions.IgnoreCase))
                        .ToArray();
                }

                //if the active profile is null, look to see if the user has an `Instruction:` scope claim
                if (activeProfile == null) {
                    string activeProfileName = null;
                    if (context.User?.Claims != null) {
                        //look for `Instruction:`-prefixed scope claim
                        var instructionClaim = context.User.Claims.FirstOrDefault(
                                c => c.Type == JwtClaimTypes.Scope 
                                && c.Value.StartsWith(Instruction.SCOPE_PREFIX));
                        if (instructionClaim != null) {
                            scopeProperties.Instruction = new InstructionParser().Parse(instructionClaim.Value);
                            activeProfileName = scopeProperties.Instruction.ProfileName;
                        }
                    }
                    if (activeProfileName == null)
                        activeProfileName = Profile.DEFAULT_NAME;

                    scopeProperties.ActiveProfile = _profiles[activeProfileName];
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