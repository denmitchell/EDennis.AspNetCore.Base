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
    ///- Checks for X-Instruction header
	///	-- if present 
	///		--- creates ActiveProfile based upon Instruction header
	///	-- else
	///		--- creates ActiveProfile based upon ...
	///			---- LaunchProfile, if present
	///			---- Default, otherwise
	///- Based upon PreAuthenticationMiddleware Options either ...
	///	-- Adds a MockClient with requested scope for the current application,
	///       as well as a special 'spi:{SPI}' scope
    ///             OR
	///	-- Adds an AutoLogin ClaimsPrincipal with designated claims
    /// 
    /// NOTE: Place in pipeline just before Authentication middleware
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
            IOptionsMonitor<Profiles> profiles,
            IOptionsMonitor<Apis> apis,
            IOptionsMonitor<ConnectionStrings> connectionStrings,
            IOptionsMonitor<MockClients> mockClients,
            IOptionsMonitor<AutoLogins> autoLogins) {


            //ignore, if swagger meta-data processing
            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                scopeProperties.ActiveProfile = new ResolvedProfile();
                var profileName = "Default";

                var instructionHeader = GetHeaderValue(context, Instruction.HEADER);
                if(instructionHeader != null) {
                    var parser = new InstructionParser();
                    scopeProperties.Instruction = parser.Parse(instructionHeader);
                    profileName = scopeProperties.Instruction.ProfileName;
                } else {
                    var launchProfile = appSettings?.CurrentValue?.LaunchProfile;
                    if (launchProfile != null)
                        profileName = launchProfile;
                }

                scopeProperties.ActiveProfile.Load(profileName,
                    profiles?.CurrentValue, apis?.CurrentValue,
                    connectionStrings?.CurrentValue, mockClients?.CurrentValue,
                    autoLogins?.CurrentValue);

                if (_options.PreAuthenticationType == PreAuthenticationType.AutoLogin) {
                    var autoLoginKey = profiles.CurrentValue[profileName].AutoLoginKey;
                    await ConfigureAutoLogin(context, scopeProperties, autoLogins.CurrentValue[autoLoginKey], autoLoginKey);
                } else if (_options.PreAuthenticationType == PreAuthenticationType.MockClient) {
                    var mockClientKey = profiles.CurrentValue[profileName].MockClientKey;
                    await ConfigureMockClient(context, scopeProperties, mockClients, mockClientKey);
                }

            }

            await _next(context);

        }

        private Task ConfigureMockClient(HttpContext context, IScopeProperties scopeProperties, IOptionsMonitor<MockClients> mockClients, object mockClientKey) {
            throw new NotImplementedException();
        }

        private async Task ConfigureAutoLogin(HttpContext httpContext, IScopeProperties scopeProperties, 
            AutoLogin autoLogin, string autoLoginKey) {

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

        }



        private string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
            => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();

    }
}
