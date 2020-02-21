using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.OpenApi.Models;

namespace MockClientApi.Lib {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //services.AddControllers();

            var _ = new ServiceConfig(services, Configuration)
                .AddControllersWithDefaultPolicies("MockClientApi", "IdentityServer")
                .AddApi<IdentityServerApi>("Apis:IdentityServer")
                .AddScopedConfiguration()
                .AddMockClient();

            if (HostingEnvironment.EnvironmentName == "Development") {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            //if (env.IsDevelopment()) {
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseScopedConfiguration();
            app.UseMockClient();

            app.Use(async (context, next) => {
                var user = context.User;
                await next();
            });

            app.UseAuthentication();

            app.Use(async (context, next) => {
                var user = context.User;
                await next();
            });

            //app.UseAuthorization();

            app.Use(async (context, next) => {
                var user = context.User;
                await next();
            });


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

        }
    }
}
