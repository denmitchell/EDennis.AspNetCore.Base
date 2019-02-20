using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EDennis.AspNetCore.Base.Testing {
    public class ApiClientInterceptor<TClient> : Interceptor
        where TClient : ApiClient {


        public ApiClientInterceptor(RequestDelegate next) : base(next) { }

        public async Task InvokeAsync(HttpContext context, IServiceProvider provider) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var testInfo = provider.GetRequiredService(typeof(TestInfo)) as TestInfo;

                var header = GetTestingHeader(context);

                if (header.Key == null) {
                    context.Request.Headers.Add(HDR_USE_CLONE, DEFAULT_NAMED_INSTANCE);
                    header = new KeyValuePair<string, string>(HDR_USE_CLONE, DEFAULT_NAMED_INSTANCE);
                }
                string operation = header.Key;
                string instanceName = header.Value;

                if (operation.Contains("InMemory"))
                    testInfo.TestDatabaseType = TestDatabaseType.InMemory;
                if (operation.Contains("Clone"))
                    testInfo.TestDatabaseType = TestDatabaseType.Clone;
                else
                    testInfo.TestDatabaseType = TestDatabaseType.Readonly;

                testInfo.Operation = operation;
                testInfo.InstanceName = instanceName;

                //does not seem to work
                var client = provider.GetRequiredService(typeof(TClient)) as TClient;

                if (operation == HDR_USE_INMEMORY || operation == HDR_USE_CLONE ) {
                    //does not seem to work
                    client.HttpClient.DefaultRequestHeaders.Add(operation, instanceName);
                } else if (operation == HDR_DROP_INMEMORY || operation == HDR_RETURN_CLONE ) {
                    client.HttpClient.SendResetAsync(operation,instanceName);
                    return;
                }
            }

            await _next(context);

        }

    }


    public static class IApplicationBuilderExtensions_ApiClientInterceptorMiddleware {
        public static IApplicationBuilder UseApiClientInterceptor<TClient>(this IApplicationBuilder app)
        where TClient : ApiClient {
            app.UseMiddleware<ApiClientInterceptor<TClient>>();
            return app;
        }
    }


}
