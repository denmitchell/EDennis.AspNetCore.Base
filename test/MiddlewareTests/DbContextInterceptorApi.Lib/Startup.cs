using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DbContextInterceptorMiddlewareApi.Lib {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            var _ = new ServiceConfig(services, Configuration)
                .AddScopeProperties()
                .AddScopedConfiguration()
                .AddPkRewriter()
                .AddDbContext<AppDbContext>()
                .AddRepo<PersonRepo>()
                .AddRepo<PositionRepo>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }


            //get developer name from query/header
            app.Use(async (context, next) => {
                var method = context.Request.Method;
                await next();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseScopedConfiguration();

            //get developer name from query/header
            app.Use(async (context, next) => {
                if (context.Request.ContainsPathHeaderOrQueryKey("X-DeveloperName", out string developerName)) {
                    Configuration["DeveloperName"] = developerName;
                }
                await next();
            });

            app.UseScopeProperties();
            app.UsePkRewriter();
            app.UseDbContextInterceptor<AppDbContext>();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
