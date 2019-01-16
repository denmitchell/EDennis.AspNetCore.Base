using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Creates a mock client with specified client id,
    /// secret, and scope.  A mock client is only created
    /// when there is no Authorization header already.
    /// 
    /// The class uses a client 
    /// configuration defined in appsettings.
    /// 
    /// in appsettings.Development.json:
    /// {
    ///   "IdentityServer": {
    ///     "Authority": "http://localhost:5000",
    ///     "ApiName": "api1",
    ///     "Client": {
    ///       "Authority": "http://localhost:5000",
    ///       "ClientId": "client",
    ///       "Secret": "secret",
    ///       "Scope": "api1"
    ///     }
    ///   }
    /// }
    /// </summary>
    public class MockClientAuthorizationMiddleware {

        private readonly RequestDelegate _next;

        public MockClientAuthorizationMiddleware(RequestDelegate next) {
            _next = next;
        }

        /// <summary>
        /// Invoke the middleware
        /// </summary>
        /// <param name="context">The HttpContext</param>
        /// <param name="config">The Configuration</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IConfiguration config) {

            //get a reference to the request headers
            var headers = context.Request.Headers;

            //only create mock client, if no authorization header
            //already exists
            if (!headers.ContainsKey("Authorization")) {

                //instantiate a new HttpClient for communicating
                //with Identity Server
                var client = new HttpClient();

                //get parameters for building the token request
                var tokenRequestData = GetClientCredentialsTokenRequestData(config);

                //get the discovery document from Identity Server
                var disco = await client.GetDiscoveryDocumentAsync(tokenRequestData.Authority as string);
                if (disco.IsError) {
                    throw new SecurityException(disco.Error);
                }

                //send a token request and receive back the response
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest {
                        Address = disco.TokenEndpoint,
                        ClientId = tokenRequestData.ClientId,
                        ClientSecret = tokenRequestData.ClientSecret,
                        Scope = string.Join(' ', tokenRequestData.Scopes)
                    });

                //handle errors
                if (tokenResponse.IsError) {
                    throw new SecurityException(tokenResponse.Error);
                }

                //build the authorization header with bearer token
                //and add it to the current request headers
                headers.Add("Authorization", "Bearer " + tokenResponse.AccessToken);

            }


            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }


        private dynamic GetClientCredentialsTokenRequestData(IConfiguration config) {

            //get command-line arguments
            var args = config.GetCommandLineArguments();

            //get AutoLogin entry, if it exists
            var mockClientArg = config.GetCommandLineArguments()
                .FirstOrDefault(a => a.Key.ToLower() == "mockclient")
                .Value;

            IConfigurationSection mockClientConfig;
            dynamic tokenRequestData;
            string authority;

            if (mockClientArg != null) {
                mockClientConfig = config.GetSection($"MockClient:{mockClientArg}");
                if (mockClientConfig == null)
                    throw new ArgumentException($"MockClientAuthorizationMiddleware requires 'MockClient:{mockClientArg}' configuration key, which is missing.");
                else {
                    tokenRequestData = GetClientCredentialsTokenRequestData(mockClientConfig);
                }
            } else {
                mockClientConfig = config.GetChildren().Where(s => s.Key.StartsWith("MockClient")).FirstOrDefault().GetChildren().FirstOrDefault();
                if (mockClientConfig == null)
                    throw new ArgumentException("MockClientAuthorizationMiddleware requires 'MockClient...' configuration key, which is missing.");
                else {
                    tokenRequestData = GetClientCredentialsTokenRequestData(mockClientConfig);
                }
            }
            if (tokenRequestData.Authority == null) {
                if(config["IdentityServer:Authority"] != null) {
                    authority = config["IdentityServer:Authority"];
                } else if (config["IdentityServer"] != null && config["IdentityServer"].StartsWith("http")) {
                    authority = config["IdentityServer"];
                } else {
                    throw new ArgumentException("MockClientAuthorizationMiddleware requires 'IdentityServer:Authority' configuration key, which is missing.");
                }
            } else {
                authority = tokenRequestData.Authority;
            }

            return new {
                Authority = authority,
                tokenRequestData.ClientId,
                tokenRequestData.ClientSecret,
                tokenRequestData.Scopes
            };

        }



        private dynamic GetClientCredentialsTokenRequestData(IConfigurationSection configSection) {

            string authority = null;
            string clientId;
            string clientSecret;
            string[] scopes;

            clientId = Regex.Replace(configSection.Key, "^MockClient:", "");

            if (configSection["Authority"] != null)
                authority = configSection["Authority"];


            if (configSection["ClientSecret"] != null)
                clientSecret = configSection["ClientSecret"];
            else
                throw new ArgumentException($"MockClientAuthorizationMiddleware requires '{configSection.Key}:ClientSecret' configuration key, which is missing.");

            if (configSection.GetSection("Scopes").GetChildren().Count() > 0)
                scopes = configSection.GetSection("Scopes").GetChildren().Select(s=>s.Value).ToArray();
            else
                throw new ArgumentException($"MockClientAuthorizationMiddleware requires '{configSection.Key}:Scopes' configuration key, which is missing.");


            return new {
                Authority = authority,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scopes = scopes
            };

        }

    }

    /// <summary>
    /// Extension methods for MockClientAuthorizationMiddleware
    /// </summary>
    public static class IApplicationBuilder_MockClientAuthorizationExtensionMethods {

        /// <summary>
        /// Creates a Use... method for the middleware
        /// </summary>
        /// <param name="app">application builder used by Startup.cs</param>
        /// <returns>application builder (for method chaining)</returns>
        public static IApplicationBuilder UseMockClientAuthorization(
                this IApplicationBuilder app) {
            return app.UseMiddleware<MockClientAuthorizationMiddleware>();
        }
    }
}
