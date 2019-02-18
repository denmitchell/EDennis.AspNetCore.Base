using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using A = EDennis.Samples.Colors.InternalApi;

namespace EDennis.Samples.Colors.ExternalApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //AspNetCore.Base config
            services.AddApiClients<InternalApi>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Color API", Version = "v1" });
            });

            services.ConfigureSwaggerGen(options => {
                options.CustomSchemaIds(x => x.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider provider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();

                //AspNetCore.Base config
                app.UseApiClientInterceptor<InternalApi>();
                app.UseLaunchers<A.Startup>(provider, Configuration);
            }


            app.UseMvc();


            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                //c.RoutePrefix = string.Empty;
            });


        }
    }
}
