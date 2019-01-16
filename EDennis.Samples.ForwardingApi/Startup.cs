using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.ForwardingApi {
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
                var launcher = new ApiLauncher(Configuration);
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
