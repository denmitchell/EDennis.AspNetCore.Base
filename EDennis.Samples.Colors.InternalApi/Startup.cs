using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.MigrationsExtensions;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Colors.InternalApi {
    public class Startup {

        public Startup(IConfiguration configuration, IHostingEnvironment environment) {
            Environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //AspNetCore.Base config
            services.AddDbContexts<ColorDbContext,ColorHistoryDbContext>(Configuration, Environment);
            services.AddRepos<ColorRepo>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Color API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(options => {
                options.CustomSchemaIds(x => x.FullName);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                app.UseAutoLogin();

                app.UseTemporalRepoInterceptor<ColorRepo, Color, ColorDbContext, ColorHistoryDbContext>();
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseUser();

            app.UseStaticFiles();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                //c.RoutePrefix = string.Empty;
            });
        }

    }

}
