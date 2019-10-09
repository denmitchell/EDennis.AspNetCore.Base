using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesApi.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using H = Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using A = IdentityServer;
using EDennis.AspNetCore.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EDennis.Samples.DefaultPoliciesApi {
    public class Startup {

        public Startup(ILogger<Startup> logger,
            IConfiguration configuration,
            IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public ILogger Logger { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            //****************************************************
            //Important: ApiLaunchers must be added to the service
            //collection before any dependent services are added
            //(e.g., if you are launching IdentityServer with 
            //ApiLauncher, it must be launched before calls 
            //to AddAuthentication("Bearer") -- where the 
            //address of IdentityServer must be known)
            //****************************************************

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development
                && (string.IsNullOrEmpty(Configuration["Apis:IdentityServer:BaseAddress"]))
                ) {
                services
                    .AddLauncher<A.Startup>(Configuration, Logger)
                    //.AddLauncher<B.Startup>()
                    //... etc.
                    .AwaitApis();
                //note: you must call AwaitApis() after adding all launchers
                //AwaitApis() blocks the main thread until the Apis are ready
            }

            var securityOptions = new SecurityOptions();
            Configuration.GetSection("Security").Bind(securityOptions);


            services.AddClientAuthenticationAndAuthorizationWithDefaultPolicies(securityOptions);

            services.AddMvc(options => {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .ExcludeReferencedProjectControllers<A.Startup>();


            //add an AuthorizationPolicyProvider using a factory pattern, 
            //so that the construction of the class is delayed until after
            //AddDefaultAuthorizationPolicyConvention is called
            services.AddSingleton<IAuthorizationPolicyProvider>(factory => {
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    Configuration, securityOptions.ScopePatternOptions);
            });



            Task.Run(() => {
                CurrentDirectoryHelpers.SetCurrentDirectory();
            });

            services.AddScoped<ScopeProperties>();

            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlite($"Data Source={HostingEnvironment.ContentRootPath}/hr.db")
                            );



            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            if (env.EnvironmentName == EnvironmentName.Development) {
                app.UseMockClientAuthorization();
            }

            app.UseAuthentication();
            app.UseUser();


            if (env.EnvironmentName == EnvironmentName.Development) {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseMvcWithDefaultRoute();
        }



    }
}
