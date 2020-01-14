using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Colors2.Models;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Serialization;
using EDennis.AspNetCore.Base.Web;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;

namespace Colors2Api.Lib {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            var _ = new ServiceConfig(services, Configuration)
                .AddScopeProperties()
                .AddDbContext<Color2DbContext>()
                .AddRepo<RgbRepo>()
                .AddRepo<HslRepo>()
                .AddNullScopedLogger();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Colors2Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseScopeProperties();
            app.UseDbContextInterceptor<Color2DbContext>();


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Colors2 API V1");
            });

        }

        IEdmModel GetEdmModel() {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Rgb>("Rgb");
            odataBuilder.EntitySet<Hsl>("Hsl");

            return odataBuilder.GetEdmModel();
        }
    }
}
