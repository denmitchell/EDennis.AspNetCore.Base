using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Hr.InternalApi2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.Samples.Hr.InternalApi2 {
    public class Startup {
        public Startup(IConfiguration configuration, IHostingEnvironment environment) {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //AspNetCore.Base config
            services.AddDbContexts<
                AgencyInvestigatorCheckContext,
                AgencyOnlineCheckContext,
                FederalBackgroundCheckContext,
                StateBackgroundCheckContext>(Configuration, Environment);
            services.AddRepos<
                AgencyInvestigatorCheckRepo,
                AgencyOnlineCheckRepo,
                FederalBackgroundCheckRepo,
                StateBackgroundCheckRepo> ();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                //READONLY -- NOT NEEDED 
                //app.UseRepoInterceptor<AgencyInvestigatorCheckRepo, AgencyInvestigatorCheck, AgencyInvestigatorCheckContext>();
                //app.UseRepoInterceptor<AgencyOnlineCheckRepo, AgencyOnlineCheck, AgencyOnlineCheckContext>();
                //app.UseRepoInterceptor<FederalBackgroundCheckRepo, FederalBackgroundCheckView, FederalBackgroundCheckContext>();
                //app.UseRepoInterceptor<StateBackgroundCheckRepo, StateBackgroundCheckView, StateBackgroundCheckContext>();
            }

            app.UseMvc();
        }
    }
}
