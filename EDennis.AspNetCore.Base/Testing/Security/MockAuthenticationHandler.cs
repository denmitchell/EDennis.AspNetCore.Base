using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing {

    public class MockAuthenticationOptions : AuthenticationSchemeOptions {
        public Claim[] Claims { get; set; }
    }


    public class MockAuthenticationHandler : AuthenticationHandler<MockAuthenticationOptions> {

        public MockAuthenticationHandler(
            IOptionsMonitor<MockAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            return await Task.Run(() => {
                var cp = new ClaimsPrincipal(new ClaimsIdentity(Options.Claims, Scheme.Name));
                var ticket = new AuthenticationTicket(cp, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            });
        }

    }

    public static class MockAuthenticationDefaults {
        public const string AuthenticationScheme = "Mock";
    }

    public static class MockAuthenticationExtensions {

        public static AuthenticationBuilder AddMock(this AuthenticationBuilder builder, 
            Action<MockAuthenticationOptions> options){
            return builder.AddScheme<MockAuthenticationOptions, MockAuthenticationHandler>(
                MockAuthenticationDefaults.AuthenticationScheme, options);
        }

    }

}
