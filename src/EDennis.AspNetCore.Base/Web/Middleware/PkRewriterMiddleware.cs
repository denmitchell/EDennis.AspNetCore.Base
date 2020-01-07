using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class PkRewriterMiddleware {

        protected readonly RequestDelegate _next;
        protected readonly IOptionsMonitor<PkRewriterSettings> _settings;
        protected readonly ILogger<PkRewriterMiddleware> _logger;
        protected readonly IConfiguration _config;
        protected readonly PerDeveloperIdCache _perDeveloperIdCache;
        protected PkRewriter _rewriter;

        public PkRewriterMiddleware(RequestDelegate next,
            IOptionsMonitor<PkRewriterSettings> settings,
            PerDeveloperIdCache perDeveloperIdCache,
            ILogger<PkRewriterMiddleware> logger,
            IConfiguration config) {
            _next = next;
            _settings = settings;
            _logger = logger;
            _config = config;
            _perDeveloperIdCache = perDeveloperIdCache;
        }

        public async Task InvokeAsync(HttpContext context) {

            var req = context.Request;
            var settings = _settings.CurrentValue;

            var middlewareEnabled = settings.Enabled;

            //handle bypass query/header
            if (req.ContainsPathHeaderOrQueryKey(Constants.PK_REWRITER_BYPASS_KEY, out string bypassStr))
                if (bypassStr == "true")
                    middlewareEnabled = false;

            if (!middlewareEnabled || req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {

                var basePrefix = settings.BasePrefix;
                var devName = _config[settings.DeveloperNameEnvironmentVariable] ?? "Anonymous";
                settings.DeveloperPrefixes.TryGetValue(devName, out int devPrefix);


                var rewriteEnabled = settings.DeveloperPrefixes.ContainsKey(devName);

                var addIdsEnabled = req.Method.Equals("Post", StringComparison.OrdinalIgnoreCase)
                    && settings.ExplicitIds != null
                    && settings.ExplicitIds.ContainsKey(req.Path.ToString());


                if (rewriteEnabled)
                    UpdatePathAndQueryString(req, devPrefix, basePrefix);

                if (HasRequestBody(req))
                    UpdateRequestBody(req, devName, addIdsEnabled, rewriteEnabled);

                if (!rewriteEnabled)
                    await _next(context);
                else
                    await UpdateResponseBody(context);

            }
        }

        private void UpdatePathAndQueryString(HttpRequest req, int devPrefix, int basePrefix) {

            _rewriter = new PkRewriter(devPrefix, basePrefix);

            //replace path & query string
            req.QueryString = new QueryString(_rewriter.Encode(req.QueryString.Value));
            req.Path = new PathString(_rewriter.Encode(req.Path.Value));

            //replace route values
            var rv = new RouteValueDictionary();
            foreach (var key in req.RouteValues.Keys)
                rv.Add(key, _rewriter.Encode(req.RouteValues[key].ToString()));

            req.RouteValues = rv;

        }


        private void UpdateRequestBody(HttpRequest req, string devName, bool addIdsEnabled, bool rewriteEnabled) {

            if (rewriteEnabled || addIdsEnabled) {
                req.EnableBuffering();
                var body = req.Body;
                try {
                    if (addIdsEnabled) {
                        GetIdExpression(req, devName, out string idExpression);
                        _logger.LogInformation($"Adding `{idExpression}` to HTTP Request Body");
                        string addId(string s) => AddId(s, idExpression);
                        req.Body = Transform(req.Body, addId);
                    }
                    if (rewriteEnabled) {
                        _logger.LogInformation($"Replacing {_rewriter.BasePrefix} with {_rewriter.DeveloperPrefix} in HTTP Request");
                        req.Body = Transform(req.Body, _rewriter.Encode);
                    }
                } catch {
                    req.Body = body;
                }
            }

        }


        private async Task UpdateResponseBody(HttpContext context) {

            //see https://stackoverflow.com/a/43404745
            //store reference to original stream
            Stream originalBody = context.Response.Body;

            //repoint response body to readable stream
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            //await next middleware and HTTP Response
            await _next(context);

            _logger.LogInformation($"Replacing {_rewriter.DeveloperPrefix} with {_rewriter.BasePrefix} in HTTP Response");

            //modify the response body by replacing developer prefix with base prefix
            memStream.Position = 0;
            string responseBody = new StreamReader(memStream).ReadToEnd();
            responseBody = _rewriter.Decode(responseBody);

            //generate a stream from the modified responseBody string
            var modifiedBody = MiddlewareUtils.StringToStream(responseBody);

            //copy the content of the modified response body stream 
            //to the original stream object (a non-readable stream)
            await modifiedBody.CopyToAsync(originalBody);

            //repoint response body to the original stream object, 
            //now holding the modified content
            context.Response.Body = originalBody;

        }



        private void GetIdExpression(HttpRequest req, string devName, out string idExpression) {
            var idSettings = _settings.CurrentValue.ExplicitIds[req.Path.ToString()];
            var idFieldName = idSettings.IdFieldName;
            var idValue = GetAddUpdateIdValue(devName, req.Path, idSettings.BaseId);
            idExpression = $"\"{idFieldName}\":{idValue}";
        }

        private int GetAddUpdateIdValue(string devName, string path, int baseValue) {
            var dict = _perDeveloperIdCache.GetOrAdd(devName, key => {
                var cd = new ConcurrentDictionary<string, int>();
                cd.TryAdd(path, baseValue + 1);
                return cd;
            });
            dict.TryGetValue(path, out int value);
            var newValue = value - 1;
            dict.TryUpdate(path, newValue, value);
            return newValue;
        }

        private static Stream Transform(Stream stream, Func<string, string> transform) {

            var body = MiddlewareUtils.StreamToString(stream);
            body = transform(body);

            return MiddlewareUtils.StringToStream(body);
        }


        private static string AddId(string body, string idExpression) {

            var idParts = idExpression.Split(':');
            var idFieldName = idParts[0].Replace("\"","");
            var idValue = idParts[1];

            var pattern = new Regex($"(\\\"{idFieldName}\\\"\\s*:\\s*)0");
            if (pattern.IsMatch(body))
                body = pattern.Replace(body, idExpression);
            else {
                var firstCurlyBrace = body.IndexOf('{');
                body = body.Insert(firstCurlyBrace + 1, idExpression + ",");
            }
            return body;
        }


        private static Stream Replace(Stream stream, string replaceWhat, string replaceWith) {

            var body = MiddlewareUtils.StreamToString(stream);
            body = body.Replace(replaceWhat, replaceWith);

            return MiddlewareUtils.StringToStream(body);
        }


        private bool HasRequestBody(HttpRequest req) {
            return req.Method.Equals("Post", StringComparison.OrdinalIgnoreCase)
                    || req.Method.Equals("Put", StringComparison.OrdinalIgnoreCase)
                    || req.Method.Equals("Patch", StringComparison.OrdinalIgnoreCase);
        }


    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UsePkRewriter(this IApplicationBuilder app) {
            app.UseMiddleware<PkRewriterMiddleware>();
            return app;
        }
    }

}