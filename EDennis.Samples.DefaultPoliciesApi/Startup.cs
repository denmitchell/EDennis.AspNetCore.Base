using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace EDennis.Samples.DefaultPoliciesApi {
    public class Startup {

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddClientAuthenticationAndAuthorizationWithDefaultPolicies();

            services.AddMvc(options => {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment,Configuration));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddTransient<PersonRepo, PersonRepo>();
            services.AddTransient<PositionRepo, PositionRepo>();


            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                });
            }

            //if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
            //    var launcher = new ApiLauncher(Configuration);
            //}

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }


            if (env.EnvironmentName == EnvironmentName.Development)
                app.UseMockClientAuthorization();

            app.UseAuthentication();

            if (env.EnvironmentName == EnvironmentName.Development) {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseMvc();
        }
    }
}
