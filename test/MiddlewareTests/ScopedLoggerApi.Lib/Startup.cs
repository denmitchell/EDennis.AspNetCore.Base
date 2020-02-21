using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ScopedLoggerApi.Lib {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            var _ = new ServiceConfig(services, Configuration)
                .AddApplicationProperties()
                .AddSession()
                .AddScopeProperties()
                .AddScopedConfiguration()
                .AddHeadersToClaims()
                .AddScopedTraceLogger();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseScopedConfiguration(); //for test configurations




            //note: don't use MockHeadersMiddleware here because
            //      the headers must be different between requests

            //to send a header via a query string (for specific requests)
            //  (for the Xunit tests, there are three requests sent during
            //   each test.  The second request sends a query string that
            //   represents a new user, for which scoped logging should
            //   not be enabled.)
            app.Use(async (context, next) => {

                Debug.WriteLine(context.Request.QueryString.ToString());

                //add or replace headers from query string
                foreach (var query in context.Request.Query.Where(q => q.Key.StartsWith("header*"))) {
                    var headerKey = query.Key.Substring("header*".Length);
                    if (context.Request.Headers.ContainsKey(headerKey))
                        context.Request.Headers.Remove(headerKey);
                    context.Request.Headers.Add(query.Key.Substring("header*".Length), query.Value);
                }

                context.Request.Headers.Add("queryString", WebUtility.UrlDecode(context.Request.QueryString.ToString()).Replace('\u0026', '&'));

                await next();

            });



            app.UseSession();
            app.UseApplicationProperties();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();



            app.UseHeadersToClaims();

            app.UseScopeProperties();
            app.UseScopedTraceLogger();


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
