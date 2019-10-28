using EDennis.AspNetCore.Base.Web.Security;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    ///- *** ENVIRONMENT GUARD - NEVER IN PRODUCTION
    ///
    ///- Checks for X-PreAuthentication header or AppSettings.PreAuthentication
	///	-- if present and true, then based upon PreAuthenticationMiddleware 
    ///	   Options either ...
	///	   ---  Adds a MockClient with requested scope for the current application,
	///         as well as a special 'Instruction:{Instruction}' scope
    ///             OR
	///	   --- Adds an AutoLogin ClaimsPrincipal with designated claims
    /// 
    /// NOTE: Place in pipeline just before Authentication middleware
    /// NOTE: If using MockClient, ensure that the MockClient has been configured
    ///       in IdentityServer, also.  The IdentityServer configuration for the 
    ///       MockClient must include all scopes configured in appsettings.
    ///       It must also include the Instruction:{Instruction} scope, if
    ///       the app has been configured to process Instructions (either
    ///       with InstructionMiddleware or DbContextOptionsInterceptor).
    /// </summary>
    public class PreAuthenticationMiddleware {

        private readonly RequestDelegate _next;
        private readonly PreAuthenticationOptions _options;
        private readonly AppSettings _appSettings;
        private readonly Profiles _profiles;
        private readonly SecureTokenService _tokenService;

        public PreAuthenticationMiddleware(RequestDelegate next, 
            IOptionsMonitor<PreAuthenticationOptions> options,
            IOptionsMonitor<AppSettings> appSettings,
            IOptionsMonitor<Profiles> profiles,
            SecureTokenService tokenService) {
            _next = next;
            _options = options?.CurrentValue ?? new PreAuthenticationOptions();
            _appSettings = appSettings.CurrentValue;
            _profiles = profiles.CurrentValue;
            _tokenService = tokenService;

            if (!_profiles.NamesUpdated)
                _profiles.UpdateNames();
        }


        public async Task InvokeAsync(HttpContext context,
            IScopeProperties scopeProperties) {

            var activeProfile = scopeProperties.ActiveProfile ?? _profiles["Default"];
            var instruction = scopeProperties.Instruction;

            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                bool preAuth = false;
                var preAuthHeader = GetHeaderValue(context, PreAuthentication.HEADER);
                if (preAuthHeader != null) {
                    if (preAuthHeader.Equals("true", StringComparison.OrdinalIgnoreCase))
                        preAuth = true;
                    else if (preAuthHeader.Trim().Equals(""))
                        preAuth = true;
                } else {
                    var appSettingsPreAuth = _appSettings?.PreAuthentication;
                    if (appSettingsPreAuth != null)
                        preAuth = appSettingsPreAuth.Value;
                }

                if (preAuth) {
                    if (_options.PreAuthenticationType == PreAuthenticationType.AutoLogin) {
                        await ConfigureAutoLogin(context, activeProfile, instruction);
                    } else if (_options.PreAuthenticationType == PreAuthenticationType.MockClient) {
                        await ConfigureMockClient(context, activeProfile, instruction);
                    }
                }

            }

            await _next(context);

        }

        private async Task ConfigureMockClient(HttpContext httpContext, Profile activeProfile, Instruction instruction) {

            var mockClient = activeProfile.MockClient;

            var tokenResponse = await _tokenService.GetTokenResponse(
                mockClient.ClientId, mockClient.ClientSecret, 
                mockClient.Scopes, instruction?.ToString());
            
            httpContext.Request.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);

            httpContext.User.Claims

            return;
        }

        private async Task ConfigureAutoLogin(HttpContext httpContext, Profile activeProfile, Instruction instruction) {

            var autoLogin = activeProfile.AutoLogin;

            List<Claim> claims = new List<Claim>();

            //add all claims specified in configuration
            foreach (var claim in autoLogin.Claims) {
                claims.Add(new Claim(claim.Type, claim.Value));
            }

            //ensure microsoft URI name claim is added, when JWT name claim is provided
            if (claims.Any(c => c.Type == JwtClaimTypes.Name) && !claims.Any(c => c.Type == ClaimTypes.Name))
                claims.Add(new Claim(ClaimTypes.Name, claims.FirstOrDefault(c=>c.Type=="name").Value)); //microsoft uri

            //add Instruction scope, if relevant
            if (instruction != null)
                claims.Add(new Claim(JwtClaimTypes.Scope, $"{Instruction.SCOPE_PREFIX}{instruction.ToString()}"));

            //create the new user object
            var identity = new ClaimsIdentity(claims,
                  CookieAuthenticationDefaults.AuthenticationScheme);
            httpContext.User = new ClaimsPrincipal(identity);


            //sign the user on and serialize the user principal to a cookie
            await httpContext.SignInAsync(httpContext.User);

            return;
        }



        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
            => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();

    }
}
