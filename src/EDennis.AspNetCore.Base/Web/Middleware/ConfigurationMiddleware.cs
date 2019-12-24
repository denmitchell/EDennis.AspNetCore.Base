using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ConfigurationMiddleware {

        protected readonly RequestDelegate _next;
        public bool Bypass { get; } = false;

        public ConfigurationMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, 
            IConfiguration configuration,
            IServiceProvider serviceProvider
            ) {


            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {


                var method = context.Request.Method;

                if (method == Constants.CONFIG_METHOD 
                    || context.Request.ContainsHeaderOrQueryKey(Constants.CONFIG_QUERY_KEY, out string _)) {

                    context.Request.EnableBuffering();
                    var body = StreamToString(context.Request.Body);

                    var config = new ConfigurationBuilder()
                        .AddJsonStream(StringToStream(body))
                        .Build();


                    var configDict = config.Flatten();
                    foreach (var key in configDict.Keys)
                        configuration[key] = configDict[key];

                    return;

                }

            }
            await _next(context);
        }


        public static Stream StringToStream(string str) {
            var memStream = new MemoryStream();
            var textWriter = new StreamWriter(memStream);
            textWriter.Write(str);
            textWriter.Flush();
            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }

        public static string StreamToString(Stream stream) {
            //stream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(stream);

            try {
                return reader.ReadToEndAsync().Result;
            } catch(Exception ex) {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

    }



    public static partial class IApplicationBuilderExtensions_Middleware {
        public static IApplicationBuilder UseConfiguration(this IApplicationBuilder app)
        {
            app.UseMiddleware<ConfigurationMiddleware>();
            return app;
        }
    }

}