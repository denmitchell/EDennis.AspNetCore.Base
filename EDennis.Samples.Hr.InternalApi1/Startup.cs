using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.JsonUtils;
using EDennis.Samples.Hr.InternalApi1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.Samples.Hr.InternalApi1 {
    public class Startup {

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(opt => {
                opt.SerializerSettings.Converters.Add(new SafeJsonConverter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //AspNetCore.Base config
            services.AddSqlContexts<HrContext>(Configuration, Environment);
            services.AddSqlRepos<EmployeeRepo,PositionRepo,EmployeePositionRepo>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                //AspNetCore.Base config 
                app.UseRepoInterceptor<EmployeeRepo, Employee, HrContext>();
                app.UseRepoInterceptor<PositionRepo, Position, HrContext>();
                app.UseRepoInterceptor<EmployeePositionRepo, EmployeePosition, HrContext>();
            }

            app.UseMvc();
        }
    }
}
