using IdentityModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public static class MiddlewareUtils {



        public static Stream StringToStream(string str) {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream);
            textWriter.Write(str);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        public static string StreamToString(Stream stream) {
            var reader = new StreamReader(stream);
            return reader.ReadToEndAsync().Result;
        }


        public static Task Reset(HttpClient[] httpClients, string instance) {
            return Task.WhenAll(httpClients.Select(a => Reset(a, instance)));
        }


        public static Task Reset(Apis apis, string instance) {
            return Task.WhenAll(apis.Values.Select(a => Reset(a, instance)));
        }

        public static async Task Reset(Api api, string instance) {
            if (api.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)) {
                using var tcp = new TcpClient("localhost", api.HttpPort.Value) {
                    SendTimeout = 500,
                    ReceiveTimeout = 1000
                };
                var builder = new StringBuilder()
                    .AppendLine($"{Constants.RESET_METHOD} /?{Constants.TESTING_INSTANCE_KEY}={instance} HTTP/1.1")
                    .AppendLine("Host: localhost")
                    .AppendLine("Connection: close")
                    .AppendLine();
                var header = Encoding.ASCII.GetBytes(builder.ToString());
                using var stream = tcp.GetStream();
                await stream.WriteAsync(header, 0, header.Length);
            }
            return;
        }


        public static async Task Reset(HttpClient httpClient, string instance) {
            if (httpClient.BaseAddress.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase)) {

                var msg = new HttpRequestMessage {
                    RequestUri = new Uri(
                        $"{httpClient.BaseAddress}?{Constants.TESTING_INSTANCE_KEY}={instance}&{Constants.TESTING_RESET_KEY}=true"),
                    Method = HttpMethod.Head
                };
                await httpClient.SendAsync(msg);
            }
            return;
        }



        public static string ResolveUser(HttpContext context, UserSources userSources, string purpose) {
            UserSource userSource = userSources.UnauthenticatedUserSource;
            if (context.User.Identity.IsAuthenticated)
                userSource = userSources.AuthenticatedUserSource;

            var user = ResolveUser(context, userSource);
            if (!string.IsNullOrEmpty(user))
                return user;
            throw new ApplicationException($"Cannot resolve user setting for {purpose} with source(s) = '{string.Join(',', userSource)}'.");
        }


        public static string ResolveUser(HttpContext context, UserSource userSource, string purpose) {
            var user = ResolveUser(context, userSource);
            if (!string.IsNullOrEmpty(user))
                return user;
            throw new ApplicationException($"Cannot resolve user setting for {purpose} with source(s) = '{string.Join(',',userSource)}'.");
        }


        public static string ResolveUser(HttpContext context, UserSource userSource) => userSource switch
        {
            UserSource.JwtNameClaim => GetClaimValue(context, JwtClaimTypes.Name),
            UserSource.OasisNameClaim => GetClaimValue(context, ClaimTypes.Name),
            UserSource.OasisEmailClaim => GetClaimValue(context, ClaimTypes.Email),
            UserSource.JwtPreferredUserNameClaim => GetClaimValue(context, JwtClaimTypes.PreferredUserName),
            UserSource.JwtSubjectClaim => GetClaimValue(context, JwtClaimTypes.Subject),
            UserSource.JwtEmailClaim => GetClaimValue(context, JwtClaimTypes.Email),
            UserSource.JwtPhoneClaim => GetClaimValue(context, JwtClaimTypes.PhoneNumber),
            UserSource.JwtClientIdClaim => GetClaimValue(context, JwtClaimTypes.ClientId),
            UserSource.SessionId => context.Session?.Id,
            UserSource.XUserHeader => GetHeaderValue(context, Constants.USER_KEY),
            UserSource.XUserQueryString => context.Request.Query[Constants.USER_KEY].ToString(),
            UserSource.WindowsUserName => Environment.GetEnvironmentVariable("USERNAME").ToString(),
            _ => null
        };


        private static string GetClaimValue(HttpContext context, string claimType)
            => context.User?.Claims?.FirstOrDefault(x
                => x.Type.Equals(claimType, StringComparison.OrdinalIgnoreCase))?.Value;


        private static string GetHeaderValue(HttpContext context, string headerKey)
            => context.Request?.Headers?.FirstOrDefault(x
                => x.Key.Equals(headerKey, StringComparison.OrdinalIgnoreCase)).Value.ToString();



    }


}
