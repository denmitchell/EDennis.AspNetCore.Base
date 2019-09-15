using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {

    public class RequestLoggingMiddleware {
        readonly RequestDelegate _next;
        readonly RequestLoggingOptions _options;

        private const object NA = null;
        private ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next,
            IOptions<RequestLoggingOptions> options) {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
            _options = options?.Value;
        }


        public async Task Invoke(HttpContext httpContext,
            ILogger<RequestLoggingMiddleware> logger) {

            _logger = logger;

            if (!httpContext.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                if (!LoggerEnabled(_options.LogLevelRange)) {
                    await _next(httpContext);
                    return;
                }

                var req = httpContext.Request;
                var resp = httpContext.Response;

                var url = $"{req.Scheme}://{req.Host}{req.Path}";

                string requestBody = "";
                string responseBody = "";

                req.EnableBuffering(bufferThreshold: 1024 * 1000);

                if (LoggerEnabled(_options.RequestBodyLogLevel) && req.ContentLength > 0)
                    requestBody = await ToText(req.Body, req.ContentLength, true);

                var originalResponseBody = resp.Body;



                try {
                    if (LoggerEnabled(_options.ResponseBodyLogLevel)) {
                        using (var responseBodyMem = new MemoryStream()) {
                            resp.Body = responseBodyMem;

                            await _next(httpContext);

                            responseBody = await ToText(resp.Body, 1024 * 1000, true);

                            await responseBodyMem.CopyToAsync(originalResponseBody);
                        }
                    }
                    else {
                        await _next(httpContext);
                    }

                } catch (Exception ex) {
                    var RequestLog = new RequestLog(logger, _options, httpContext, requestBody, responseBody, ex);

                    logger.Log(LogLevel.Error, "Url: {Url}, QueryString: {QueryString}, Method: {Method}, StatusCode: {StatusCode}, Headers: {Headers}, Claims: {Claims}, RequestBody: {RequestBody:lj}, ResponseBody: {ResponseBody:lj}, ExceptionMessage: {ExceptionMessage}, StackTrace: {StackTrace}",
                        RequestLog.Url, RequestLog.QueryString, RequestLog.Method, RequestLog.StatusCode, RequestLog.Headers, RequestLog.Claims, RequestLog.RequestBody, RequestLog.ResponseBody, RequestLog.ExceptionMessage, RequestLog.StackTrace);
                    throw ex;
                }

                var statusCode = httpContext.Response.StatusCode;

                var logLevel = (statusCode >= 500)
                    ? LogLevel.Error
                    : (statusCode >= 400)
                    ? LogLevel.Warning
                    : LogLevel.Information;

                if (true) {
                    var RequestLog = new RequestLog(logger, _options, httpContext, requestBody, responseBody);

                    logger.Log(logLevel, "Url: {Url}, QueryString: {QueryString}, Method: {Method}, StatusCode: {StatusCode}, Headers: {Headers}, Claims: {Claims}, RequestBody: {RequestBody:lj}, ResponseBody: {ResponseBody:ly}",
                        RequestLog.Url, RequestLog.QueryString, RequestLog.Method, RequestLog.StatusCode, RequestLog.Headers, RequestLog.Claims, RequestLog.RequestBody, RequestLog.ResponseBody);
                }

            }
        }

        private async Task<dynamic> ToText(Stream stream, long? size, bool rewind) {
            string text = "";
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(
                       stream,
                       encoding: Encoding.UTF8,
                       detectEncodingFromByteOrderMarks: false,
                       bufferSize: Convert.ToInt32(size),
                       leaveOpen: rewind)) {
                text = await reader.ReadToEndAsync();
                if (rewind)
                    stream.Seek(0, SeekOrigin.Begin);
            }
            return text;
        }


        private bool LoggerEnabled(LogLevelRange range) {
            return _options.LogLevelRange != LogLevelRange.None
                && _logger.IsEnabled((LogLevel)range);
        }

    }


    public static class RequestLoggingMiddlewareExtensions {
        public static IServiceCollection AddRequestLogging(this IServiceCollection service,
            Action<RequestLoggingOptions> options = null) {

            options = options ?? (opts => { });

            service.Configure(options);
            return service;
        }

        public static IApplicationBuilder UseRequestLogging(
            this IApplicationBuilder app) {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }



    }


    public class RequestLog {
        private ILogger<RequestLoggingMiddleware> _logger;
        private RequestLoggingOptions _options;
        private HttpContext _httpContext;
        private string _requestBody;
        private string _responseBody;
        private Exception _exception;

        public string Url { get; set; }
        public string QueryString { get; set; }
        public string Method { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }


        public RequestLog(ILogger<RequestLoggingMiddleware> logger, RequestLoggingOptions requestLoggingOptions,
            HttpContext httpContext, string requestBody = null,
            string responseBody = null,
            Exception exception = null

            ) {
            _logger = logger;
            _options = requestLoggingOptions;
            _httpContext = httpContext;
            _requestBody = requestBody;
            _responseBody = responseBody;
            _exception = exception;

            Build();
        }

        public void Build() {

            var req = _httpContext.Request;
            var resp = _httpContext.Response;

            QueryString = req.QueryString.ToString();
            Method = req.Method;
            Url = $"{req.Scheme}://{req.Host}{req.Path}";

            StatusCode = (HttpStatusCode)resp.StatusCode;

            if (LoggerEnabled(_options.RequestBodyLogLevel))
                RequestBody = _requestBody;

            if (LoggerEnabled(_options.ResponseBodyLogLevel))
                ResponseBody = _responseBody;

            if (LoggerEnabled(_options.ExceptionMessage.LogLevelRange)
                && StatusCode.In(_options.ExceptionMessage.StatusCodeRange))
                ExceptionMessage = _exception?.Message;

            if (LoggerEnabled(_options.StackTrace.LogLevelRange)
                && StatusCode.In(_options.StackTrace.StatusCodeRange))
                StackTrace = _exception?.StackTrace;


            if (LoggerEnabled(_options.Headers.LogLevelRange)
                && StatusCode.In(_options.Headers.StatusCodeRange)) {
                Headers = req.Headers
                    .Select(h => KeyValuePair.Create(
                        h.Key, h.Value.ToString()))
                    .ToDictionary(h => h.Key, h => h.Value);
            }


            if (LoggerEnabled(_options.UserClaims.LogLevelRange)
                && StatusCode.In(_options.UserClaims.StatusCodeRange)
                && _httpContext.User.Claims.Count() > 0) {
                foreach (var claim in _httpContext.User.Claims) {
                    if (Claims.ContainsKey(claim.Type))
                        Claims[claim.Type] += $",{claim.Value}";
                    else
                        Claims.Add(claim.Type, claim.Value);
                }
            }

        }

        private bool LoggerEnabled(LogLevelRange range) {
            return _options.LogLevelRange != LogLevelRange.None
                && _logger.IsEnabled((LogLevel)range);
        }

    }

}
