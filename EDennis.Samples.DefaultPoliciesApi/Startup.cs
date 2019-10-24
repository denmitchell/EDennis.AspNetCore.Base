using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Threading.Tasks;
using A = IdentityServer;

namespace EDennis.Samples.DefaultPoliciesApi {
    public class Startup {

        public static ILogger<Startup> Logger;
        static Startup() {

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.{env}.json")
                            .Build();

            //This is the default logger.
            //   Name = "Logger", Index = 0, LogLevel = Information
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration, "Logging:Loggers:Default")
                        .CreateLogger();

            Log.Logger.Information($"Starting Application: EDennis.Samples.DefaultPoliciesApi, {env}");

            Logger = new SerilogLoggerFactory(Log.Logger).CreateLogger<Startup>();
        }

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

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

            if (HostingEnvironment.EnvironmentName == "Development"
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

            services.AddAuthentication(securityOptions);

            services.AddControllers(options => {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
            }).ExcludeReferencedProjectControllers<A.Startup>();


            services.AddScoped<ScopeProperties22>();

            //add secondary loggers for on-demand, per-user verbose and debug logging
            services.AddSecondaryLoggers(typeof(SerilogVerboseLogger<>), typeof(SerilogDebugLogger<>));


            //add an AuthorizationPolicyProvider which generates default
            //policies upon first access to any controller action
            services.AddSingleton<IAuthorizationPolicyProvider>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultPoliciesAuthorizationPolicyProvider>>();
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    Configuration, securityOptions.ScopePatternOptions, logger);
                }
            );


            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlite($"Data Source={HostingEnvironment.ContentRootPath}/hr.db")
                            );



            if (HostingEnvironment.EnvironmentName == "Development") {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                });
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();
            }

            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseMockClientAuthorization();
            }

            app.UseAuthentication();
            app.UseUser();

            app.UseRouting();
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
