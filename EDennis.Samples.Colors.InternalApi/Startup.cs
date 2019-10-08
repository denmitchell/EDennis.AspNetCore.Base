using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors.InternalApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace EDennis.Samples.Colors.InternalApi {
    public class Startup {

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            HostingEnvironment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddControllers();

            //AspNetCore.Base config
            services.AddDbContexts<ColorDbContext,ColorHistoryDbContext>(Configuration, HostingEnvironment);
            services.AddRepos<ColorRepo>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Color API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(options => {
                options.CustomSchemaIds(x => x.FullName);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();

                app.UseAutoLogin();

                app.UseTemporalRepoInterceptor<ColorRepo, Color, ColorDbContext, ColorHistoryDbContext>();
            }

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseUser();

            app.UseStaticFiles();

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
