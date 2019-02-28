using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using A = IdentityServer;

namespace EDennis.Samples.Hr.InternalApi2 {
    public class Startup {
        public Startup(ILogger<Startup> logger, 
            IConfiguration configuration, IHostingEnvironment environment) {
            Configuration = configuration;
            HostingEnvironment = environment;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; set; }
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

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services
                    .AddLauncher<A.Startup>(Configuration, Logger)
                    //.AddLauncher<B.Startup>()
                    //... etc.
                    .AwaitApis();
                //note: you must call AwaitApis() after adding all launchers
                //AwaitApis() blocks the main thread until the Apis are ready
            }

            services.AddClientAuthenticationAndAuthorizationWithDefaultPolicies();


            services.AddMvc(options => {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddDbContexts<         //you can add 5 at a time.
                AgencyInvestigatorCheckContext,
                AgencyOnlineCheckContext,
                FederalBackgroundCheckContext,
                StateBackgroundCheckContext>(Configuration, HostingEnvironment);
            services.AddDbContexts<
                AgencyInvestigatorCheckHistoryContext,
                FederalBackgroundCheckHistoryContext>(Configuration, HostingEnvironment);
            services.AddRepos<
                AgencyInvestigatorCheckRepo,
                AgencyOnlineCheckRepo,
                FederalBackgroundCheckRepo,
                StateBackgroundCheckRepo> ();


            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {

                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "HR Internal API 2", Version = "v1" });
                });

                services.ConfigureSwaggerGen(options => {
                    // UseFullTypeNameInSchemaIds replacement for .NET Core
                    options.CustomSchemaIds(x => x.FullName);
                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                app.UseMockClientAuthorization();

                app.UseTemporalRepoInterceptor<
                    AgencyInvestigatorCheckRepo, 
                    AgencyInvestigatorCheck, 
                    AgencyInvestigatorCheckContext,
                    AgencyInvestigatorCheckHistoryContext>();
                app.UseRepoInterceptor<
                    AgencyOnlineCheckRepo, 
                    AgencyOnlineCheck, 
                    AgencyOnlineCheckContext>();

                //NOTE: Interceptors not needed for 
                //         FederalBackgroundCheckRepo and
                //         StateBackgroundCheckRepo.
                //      These repos are readonly. 
            }

            app.UseAuthentication();
            app.UseUser();

            app.UseMvc();


            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR Internal API 2 V1");
                });
            }

        }
    }
}
