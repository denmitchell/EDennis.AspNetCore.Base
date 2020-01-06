using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Web {
    public class PkRewriterMiddleware {

        protected readonly RequestDelegate _next;
        protected readonly IOptionsMonitor<PkRewriterSettings> _settings;
        protected readonly ILogger<PkRewriterMiddleware> _logger;
        protected readonly IConfiguration _config;

        public PkRewriterMiddleware(RequestDelegate next,
            IOptionsMonitor<PkRewriterSettings> settings,
            ILogger<PkRewriterMiddleware> logger,
            IConfiguration config) {
            _next = next;
            _settings = settings;
            _logger = logger;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context) {


            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            //handle bypass query/header
            if (req.ContainsPathHeaderOrQueryKey(Constants.PK_REWRITER_BYPASS_KEY, out string bypassStr))
                if (bypassStr == "true")
                    enabled = false;

            if (!enabled || req.Path.StartsWithSegments(new PathString("/swagger"))) {
                await _next(context);
            } else {


                var devName = _config[_settings.CurrentValue.DeveloperNameEnvironmentVariable];
                var devPrefix = _settings.CurrentValue.DeveloperPrefixes[devName];
                var basePrefix = _settings.CurrentValue.BasePrefix;

                var rewriter = new PkRewriter(devPrefix, basePrefix); 

                _logger.LogInformation($"Replacing {rewriter.BasePrefix} with {rewriter.DeveloperPrefix} in HTTP Request");

                //replace path & query string
                req.QueryString = new QueryString(rewriter.Encode(context.Request.QueryString.Value));
                req.Path = new PathString(rewriter.Encode(context.Request.Path.Value));

                //replace route values
                var rv = new RouteValueDictionary();
                foreach (var key in req.RouteValues.Keys)
                    rv.Add(key, rewriter.Encode(req.RouteValues[key].ToString()));

                req.RouteValues = rv;

                //replace request body
                if (req.Method.Equals("Post",StringComparison.OrdinalIgnoreCase) 
                    || req.Method.Equals("Put", StringComparison.OrdinalIgnoreCase)
                    || req.Method.Equals("Patch", StringComparison.OrdinalIgnoreCase)) {
                    req.EnableBuffering();
                    var body = req.Body;
                    try {
                        req.Body = Transform(req.Body, rewriter.Encode);
                    } catch {
                        req.Body = body;
                    }
                }

                //see https://stackoverflow.com/a/43404745
                //store reference to original stream
                Stream originalBody = context.Response.Body;

                //repoint response body to readable stream
                using var memStream = new MemoryStream();
                context.Response.Body = memStream;

                //await next middleware and HTTP Response
                await _next(context);

                _logger.LogInformation($"Replacing {devPrefix} with {basePrefix} in HTTP Response");

                //modify the response body by replacing developer prefix with base prefix
                memStream.Position = 0;
                string responseBody = new StreamReader(memStream).ReadToEnd();
                responseBody = rewriter.Decode(responseBody);

                //generate a stream from the modified responseBody string
                var modifiedBody = MiddlewareUtils.StringToStream(responseBody);

                //copy the content of the modified response body stream 
                //to the original stream object (a non-readable stream)
                await modifiedBody.CopyToAsync(originalBody);

                //repoint response body to the original stream object, 
                //now holding the modified content
                context.Response.Body = originalBody;
            }
        }


        private static Stream Transform(Stream stream, Func<string,string> transform) {

            var body = MiddlewareUtils.StreamToString(stream);
            body = transform(body);

            return MiddlewareUtils.StringToStream(body);
        }


        private static Stream Replace(Stream stream, string replaceWhat, string replaceWith) {

            var body = MiddlewareUtils.StreamToString(stream);
            body = body.Replace(replaceWhat, replaceWith);

            return MiddlewareUtils.StringToStream(body);
        }

    }


    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UsePkRewriter(this IApplicationBuilder app) {
            app.UseMiddleware<PkRewriterMiddleware>();
            return app;
        }
    }

}