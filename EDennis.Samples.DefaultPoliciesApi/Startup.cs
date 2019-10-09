using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.DefaultPoliciesApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using A = IdentityServer;

namespace EDennis.Samples.DefaultPoliciesApi {
    public class Startup {

        public Startup(ILogger<Startup> logger,
            IConfiguration configuration,
            IWebHostEnvironment env) {
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


            Task.Run(() => {
                CurrentDirectoryHelpers.SetCurrentDirectory();
            });

            services.AddScoped<ScopeProperties>();

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
