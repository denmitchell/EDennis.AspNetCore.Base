using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EDennis.Samples.HeadersToClaimsMiddlewareApi.Lib {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            //configure HeadersToClaimsMiddleware
            var _ = new ServiceConfig(services, Configuration)
                .AddHeadersToClaims();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseAuthorization();


            app.UseScopedConfiguration(); //for test configurations

            //for testing purposes:
            //intercept request to add claims and headers 
            //(for ScopePropertiesMiddleware to process)
            app.Use(async (context, next) => {

                //add claims from query string
                var claims = new List<Claim>();
                foreach (var query in context.Request.Query.Where(q => q.Key.StartsWith("claim*")))
                    claims.Add(new Claim(query.Key.Substring("claim*".Length), query.Value));

                if (claims.Count > 0) {
                    var appIdentity = new ClaimsIdentity(claims);
                    context.User.AddIdentity(appIdentity);
                }

                //add headers from query string
                foreach (var query in context.Request.Query.Where(q => q.Key.StartsWith("header*")))
                    context.Request.Headers.Add(query.Key.Substring("header*".Length), query.Value);

                await next();

            });


            //Use HeadersToClaimsMiddleware
            app.UseHeadersToClaims();


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
