using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using A = EDennis.Samples.Colors.InternalApi;

namespace EDennis.Samples.Colors.ExternalApi {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env, ILogger<Startup> logger) {
            Configuration = configuration;
            HostingEnvironment = env;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }
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

            if (HostingEnvironment.EnvironmentName == "Development") {
                services
                    .AddLauncher<A.Startup>(Configuration, Logger)
                    //.AddLauncher<B.Startup>()
                    //... etc.
                    .AwaitApis();
                //note: you must call AwaitApis() after adding all launchers
                //AwaitApis() blocks the main thread until the Apis are ready
            }

            services.AddControllers();

            //    .ExcludeReferencedProjectControllers<A.Startup>();  //IMPORTANT!



            services.AddApiClient<IInternalApi,InternalApi>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Color API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(options => {
                options.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();

                //AspNetCore.Base config
                app.UseApiClientInterceptor<InternalApi>();
            }


            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });


            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                //c.RoutePrefix = string.Empty;
            });


        }
    }
}
