using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.Samples.Hr.InternalApi2 {
    public class Startup {
        public Startup(IConfiguration configuration, IHostingEnvironment environment) {
            Configuration = configuration;
            HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => {
                if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                    options.Filters.Add<TestingActionFilter>();
                }
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContextPools(Configuration);
            services.AddRepos();

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services.AddSingleton<IDbContextBaseTestCache, DbContextBaseTestCache>();
            }

            var provider = services.BuildServiceProvider();
            var mvc = provider.GetRequiredService<IMvcBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
