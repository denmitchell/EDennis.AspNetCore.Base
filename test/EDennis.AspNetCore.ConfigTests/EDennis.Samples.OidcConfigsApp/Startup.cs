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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EDennis.Samples.OidcConfigsApp {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Environment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //services.AddRazorPages();
            var _ = new ServiceConfig(services, Configuration)
                .AddApi<IdentityServerApi>("Apis:IdentityServer")
                .AddRazorPagesWithDefaultPolicies(Environment.ApplicationName,"Apis:IdentityServer");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (context, next) => {

                var claims = new List<Claim>();

                claims.Add(new Claim("scope", "EDennis.Samples.OidcConfigsApp.*"));
                claims.Add(new Claim("scope", "EDennis.Samples.OidcConfigsApp.Index"));
                claims.Add(new Claim("scope", "EDennis.Samples.OidcConfigsApp.Error"));
                claims.Add(new Claim("scope", "EDennis.Samples.OidcConfigsApp.Privacy"));
                var appIdentity = new ClaimsIdentity(claims);
                context.User.AddIdentity(appIdentity);

                await next();

            });

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
            });
        }
    }
}
