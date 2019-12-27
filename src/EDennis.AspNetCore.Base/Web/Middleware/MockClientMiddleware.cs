using EDennis.AspNetCore.Base.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// NOTE: Place in pipeline just before Authentication middleware
    /// NOTE: If using MockClient, ensure that the MockClient has been configured
    ///       in IdentityServer, also.  The IdentityServer configuration for the 
    ///       MockClient must include all scopes configured in appsettings.
    /// </summary>
    public class MockClientMiddleware {

        private readonly RequestDelegate _next;
        private readonly ISecureTokenService _tokenService;
        private readonly IOptionsMonitor<ActiveMockClientSettings> _settings;
        private readonly IConfiguration _config;

        public bool Bypass { get; } = false;

        public MockClientMiddleware(RequestDelegate next, 
            ISecureTokenService tokenService,
            IWebHostEnvironment env,
            IOptionsMonitor<ActiveMockClientSettings> settings,
            IConfiguration config) {
            _next = next;
            _tokenService = tokenService;
            _settings = settings;
            _config = config;
            if (env.EnvironmentName == "Production")
                Bypass = true;
        }


        public async Task InvokeAsync(HttpContext context) {


            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            if (Bypass || !enabled || !req.Path.StartsWithSegments(new PathString("/swagger"))) {

                var activeMockClientKey = _config[Constants.ACTIVE_MOCK_CLIENT_KEY];
                var mockClient = _settings.CurrentValue.MockClients[activeMockClientKey];
                if (mockClient != null) {

                    var tokenResponse = await _tokenService.GetTokenResponse(
                        mockClient.ClientId, mockClient.ClientSecret,
                        mockClient.Scopes);

                    req.Headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);
                }
            }

            await _next(context);

        }

    }

    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseMockClient(this IApplicationBuilder app) {
            app.UseMiddleware<MockClientMiddleware>();
            return app;
        }
    }


}
