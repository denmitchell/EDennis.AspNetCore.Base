using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Web;
using Hr.RepoApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Hr.RepoApi.Lib {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }
        public string AppName => HostEnvironment.ApplicationName;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //services.AddControllers();

            var _ = new ServiceConfig(services, Configuration)
                .AddControllersWithDefaultPolicies(AppName)  //to auto-generate [Authorize] and associated security policies
                .AddApi<IdentityServerApi>()//to setup DI dependency for SecureApiClient's
                .AddApplicationProperties() //to hold EntryPoint type for app (for user resolution)
                .AddSession()               //to setup Session.Id as fallback surrogate Id for user
                .AddScopeProperties()       //to resolve and hold User and other data
                .AddDbContext<HrContext>()  //to setup DI for DbContext subclass
                    .AddRepo<PersonRepo>()  //to setup DI for repo class
                    .AddRepo<AddressRepo>() //to setup DI for repo class
                .AddHeadersToClaims()       //to get user from razor app via X-User header
                .AddMockClient()            //to bypass oauth (not in production)
                //.AddPkRewriter()          //only needed when two apis access the same database
                .AddScopedTraceLogger();    //to setup user-specific trace logging that can be turned on and off via request query strings                


            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hr.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseMockClient();            //to mock OAuth authorization (not in production)
            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseHeadersToClaims();       //to get user from razor app via X-User header
            app.UseApplicationProperties(); //to hold EntryPoint type for app (for user resolution)
            app.UseSession();               //to setup Session.Id as fallback surrogate Id for user

            app.UseScopeProperties();       //to resolve and hold User and other data
            app.UseDbContextInterceptor<HrContext>();  //to replace injected DbContext with one that can be rolled back
            app.UseScopedTraceLogger();     //to turn on/off trace logging for specific users while the app is running


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers(); 
            });


            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR API V1");
            });


        }
    }
}
