using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using I = IdentityServer;
using A = EDennis.Samples.Hr.InternalApi1;
using B = EDennis.Samples.Hr.InternalApi2;

namespace EDennis.Samples.Hr.ExternalApi {
    public class Startup {

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger) {
            Configuration = configuration;
            HostingEnvironment = env;
            Logger = logger;
        }

        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }
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
                    .AddLauncher<I.Startup>(Configuration, Logger)
                    .AddLauncher<A.Startup>()
                    .AddLauncher<B.Startup>()
                    //... etc.
                    .AwaitApis();
                //note: you must call AwaitApis() after adding all launchers
                //AwaitApis() blocks the main thread until the Apis are ready
            }


            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .ExcludeReferencedProjectControllers<I.Startup>()
                .ExcludeReferencedProjectControllers<A.Startup>()
                .ExcludeReferencedProjectControllers<B.Startup>();

            //AspNetCore.Base config
            services.AddApiClients<IdentityServerApi,InternalApi1,InternalApi2>();

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {


                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "HR External API", Version = "v1" });
                });

                services.ConfigureSwaggerGen(options => {
                    // UseFullTypeNameInSchemaIds replacement for .NET Core
                    options.CustomSchemaIds(x => x.FullName);
                });
            }



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                //AspNetCore.Base config
                app.UseApiClientInterceptor<InternalApi1>();
                app.UseApiClientInterceptor<InternalApi2>();

            }

            app.UseMvc();


            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR External API V1");
                    //c.RoutePrefix = string.Empty;
                });
            }
        }
    }
}
