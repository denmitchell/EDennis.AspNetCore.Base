using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using EDennis.JsonUtils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.Samples.Hr.InternalApi1 {
    public class Startup {

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => {
                if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                    options.Filters.Add<TestingActionFilter>();
                }
            })
            .AddJsonOptions(opt => {
                opt.SerializerSettings.Converters.Add(new SafeJsonConverter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContextPools(Configuration);
            services.AddRepos();

            //services.AddDbContextPool<HrContext>(options => {
            //    options.UseSqlServer(Configuration.GetConnectionString("HrContext"));
            //});
            //services.AddTransient<EmployeeRepo, EmployeeRepo>();
            //services.AddTransient<PositionRepo, PositionRepo>();
            //services.AddTransient<EmployeePositionRepo, EmployeePositionRepo>();
            //services.AddTransient<ManagerPositionRepo, ManagerPositionRepo>();

            if (HostingEnvironment.EnvironmentName == EnvironmentName.Development) {
                services.AddSingleton<IDbContextBaseTestCache, DbContextBaseTestCache>();
            }


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
