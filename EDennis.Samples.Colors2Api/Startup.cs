using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors2Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace EDennis.Samples.Colors2Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddMvc(options => {
                    options.Conventions.Add(new WriteableControllerRouteConvention());
                    options.Conventions.Add(new ReadonlyControllerRouteConvention());
                    //and then, if desired ...
                    //options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
                })
                .ConfigureApplicationPartManager(m => {
                    m.FeatureProviders.Add(new WriteableControllerFeatureProvider());
                    m.FeatureProviders.Add(new ReadonlyControllerFeatureProvider());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //services.AddClientAuthenticationAndAuthorizationWithDefaultPolicies();

            services.AddDbContexts<ColorsDbContext>(Configuration, HostingEnvironment);
            services.AddRepos<RgbRepo,HslRepo>();

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "Color Api", Version = "v1" });
                });
            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.EnvironmentName == EnvironmentName.Development) {
                //app.UseMockClientAuthorization();
            }

            //app.UseAuthentication();
            app.UseUser();

            if (env.EnvironmentName == EnvironmentName.Development) {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                });
            }


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
