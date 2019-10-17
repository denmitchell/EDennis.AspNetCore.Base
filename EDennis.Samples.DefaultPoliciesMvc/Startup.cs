using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Extensions;
using EDennis.Samples.DefaultPoliciesMvc.ApiClients;
using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using A = IdentityServer;
using B = EDennis.Samples.DefaultPoliciesApi;

namespace EDennis.Samples.DefaultPoliciesMvc {
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



        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
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
                    .AddLauncher<B.Startup>()
                    //... etc.
                    .AwaitApis();
                //note: you must call AwaitApis() after adding all launchers
                //AwaitApis() blocks the main thread until the Apis are ready
            }


            services.AddControllersWithViews(options=> {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
            })
                .ExcludeReferencedProjectControllers<A.Startup>()
                .ExcludeReferencedProjectControllers<B.Startup>();


            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var securityOptions = new EDennis.AspNetCore.Base.Security.SecurityOptions();
            Configuration.GetSection("Security").Bind(securityOptions);

            services.AddAuthentication(securityOptions);

            services.AddControllersWithViews(options => {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
            });


            services.AddScoped<ScopeProperties>();
            services.AddApiClients<IdentityServerApi>();
            services.AddApiClient<IDefaultPoliciesApiClient, DefaultPoliciesApiClient>();

            //add an AuthorizationPolicyProvider which generates default
            //policies upon first access to any controller action
            services.AddSingleton<IAuthorizationPolicyProvider>((container) => {
                var logger = container.GetRequiredService<ILogger<DefaultPoliciesAuthorizationPolicyProvider>>();
                return new DefaultPoliciesAuthorizationPolicyProvider(
                    Configuration, securityOptions.ScopePatternOptions, logger);
            }
            );


            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlite("Data Source=hr.db")
                            );



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();


            //app.UseAuthorization();
            app.UseAuthentication();
            app.UseUser();


            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
