using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.MigrationsExtensions;
using EDennis.Samples.Colors.InternalApi.Models;
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
            services.AddSqlContexts<ColorDbContext>(Configuration, Environment);
            //AspNetCore.Base config
            services.AddSqlRepos<ColorRepo>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Color API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(options => {
                options.CustomSchemaIds(x => x.FullName);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                //AspNetCore.Base config 
                app.UseRepoInterceptor<ColorRepo, Color, ColorDbContext>();
            }

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
