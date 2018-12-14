using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace EDennis.Samples.ExternalApi {
    public class Startup {

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => {
                if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                    options.Filters.Add<TestingActionFilter>();
                }
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpClients(Configuration);

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new Info { Title = "HR API", Version = "v1" });
                });

                services.ConfigureSwaggerGen(options => {
                    // UseFullTypeNameInSchemaIds replacement for .NET Core
                    options.CustomSchemaIds(x => x.FullName);
                });
            }

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                var logger = services.BuildServiceProvider().GetService<ILogger<ApiLauncher>>() as ILogger<ApiLauncher>;
                var launcher = new ApiLauncher(Configuration,logger);
            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();


            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HR API V1");
                    //c.RoutePrefix = string.Empty;
                });
            }
        }
    }
}
