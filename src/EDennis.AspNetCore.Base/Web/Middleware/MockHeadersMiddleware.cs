using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class MockHeadersMiddleware {

        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<MockHeaderSettingsCollection> _settings;
        public bool Bypass { get; } = false;

        public MockHeadersMiddleware(RequestDelegate next, 
            IOptionsMonitor<MockHeaderSettingsCollection> settings,
            IWebHostEnvironment env ) {
            _next = next;
            _settings = settings;
            if (env.EnvironmentName == "Production")
                Bypass = true;
        }

        public async Task InvokeAsync(HttpContext context) {
            var req = context.Request;
            var enabled = (_settings.CurrentValue?.Enabled ?? new bool?(false)).Value;

            if (Bypass || !enabled || !req.Path.StartsWithSegments(new PathString("/swagger"))) {

                var mockHeaders = _settings.CurrentValue;
                if (mockHeaders != null) {

                    var headers = req.Headers;


                    //loop over the grouped mock headers, adding them to the RequestHeaders
                    foreach (var key in mockHeaders.Keys) {
                        //if the request already contains the header key, then either:
                        //(a) skip the mock
                        //(b) overwrite the existing with the mock
                        //(c) replace with a union over all values -- existing and mock
                        if (headers.ContainsKey(key)) {
                            var mock = mockHeaders[key];
                            if (mock.ConflictResolution == MockHeaderConflictResolution.Skip)
                                continue;
                            else {
                                StringValues newValues;
                                if (mock.ConflictResolution == MockHeaderConflictResolution.Overwrite)
                                    newValues = mock.Values;
                                else
                                    newValues = new StringValues(mock.Values.Union(headers[key]).ToArray());

                                headers.Remove(key);
                                headers.Add(key, newValues);
                            }
                        } else {
                            headers.Add(key, mockHeaders[key].Values);
                        }
                    }
                }
            }
            await _next(context);
        }

    }




    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseMockHeaders(this IApplicationBuilder app){
            app.UseMiddleware<MockHeadersMiddleware>();
            return app;
        }
    }




}
