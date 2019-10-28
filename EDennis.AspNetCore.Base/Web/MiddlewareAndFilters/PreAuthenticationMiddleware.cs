using EDennis.AspNetCore.Base.Web.Security;
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


        public PreAuthenticationMiddleware(RequestDelegate next, 
            IOptionsMonitor<PreAuthenticationOptions> options) {
            _next = next;
            _options = options?.CurrentValue ?? new PreAuthenticationOptions();
        }


        public async Task InvokeAsync(HttpContext context,
            IScopeProperties scopeProperties,
            IOptionsMonitor<AppSettings> appSettings,
            SecureTokenService tokenService) {


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
                    var appSettingsPreAuth = appSettings?.CurrentValue?.PreAuthentication;
                    if (appSettingsPreAuth != null)
                        preAuth = appSettingsPreAuth.Value;
                }

                if (preAuth) {
                    if (_options.PreAuthenticationType == PreAuthenticationType.AutoLogin) {
                        await ConfigureAutoLogin(context, scopeProperties);
                    } else if (_options.PreAuthenticationType == PreAuthenticationType.MockClient) {
                        await ConfigureMockClient(context, scopeProperties, tokenService);
                    }
                }

            }

            await _next(context);

        }

        private async Task ConfigureMockClient(HttpContext httpContext, IScopeProperties scopeProperties,
            SecureTokenService tokenService) {
            var mockClient = scopeProperties.ActiveProfile.MockClient;
            var mockClientKey = scopeProperties.ActiveProfile.MockClientKey;
            var instruction = scopeProperties.Instruction;

            var tokenResponse = await tokenService.GetTokenResponse(
                mockClient.ClientId ?? mockClientKey,
                mockClient.ClientSecret, mockClient.Scopes, instruction.ToString());
            
            httpContext.Request.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);

            return;
        }

        private async Task ConfigureAutoLogin(HttpContext httpContext, IScopeProperties scopeProperties) {

            var autoLogin = scopeProperties.ActiveProfile.AutoLogin;
            var autoLoginKey = scopeProperties.ActiveProfile.AutoLoginKey;

            List<Claim> claims = new List<Claim>();

            if (autoLogin.Claims == null || !autoLogin.Claims.Any(c => c.Type == "name"))
                claims.Add(new Claim("name", autoLoginKey)); //jwt/simple type

            if (autoLogin.Claims == null || !autoLogin.Claims.Any(c => c.Type == ClaimTypes.Name))
                claims.Add(new Claim(ClaimTypes.Name, autoLoginKey)); //microsoft uri

            foreach (var claim in autoLogin.Claims) {
                claims.Add(new Claim(claim.Type, claim.Value));
            }

            //ensure microsoft URI name claim is added, when JWT name claim is provided
            if (claims.Any(c => c.Type == "name") && !claims.Any(c => c.Type == ClaimTypes.Name))
                claims.Add(new Claim(ClaimTypes.Name, claims.FirstOrDefault(c=>c.Type=="name").Value)); //microsoft uri


            scopeProperties.User = autoLoginKey;

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
