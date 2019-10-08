using EDennis.AspNetCore.Base.Testing;
using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Colors2Api.Models;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace EDennis.Samples.Colors2Api {
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddOData();

            services.AddControllers();
            //services
            //    .AddMvc(options => {

            //        //and then, if desired ...
            //        //options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));

            //        // Workaround: https://github.com/OData/WebApi/issues/1177
            //        foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0)) {
            //            outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            //        }
            //        foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0)) {
            //            inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            //        }


            //    })
            //    .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //services.AddClientAuthenticationAndAuthorizationWithDefaultPolicies();

            services.AddDbContexts<ColorsDbContext>(Configuration, HostingEnvironment);
            services.AddRepos<RgbRepo,HslRepo>();

            if (HostingEnvironment.EnvironmentName == "Development") {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Color Api", Version = "v1" });
                });
            }


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app) {
            if (HostingEnvironment.EnvironmentName == "Environment") {
                app.UseDeveloperExceptionPage();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (HostingEnvironment.EnvironmentName == "Development") {
                //app.UseMockClientAuthorization();
                app.UseRepoInterceptor<RgbRepo, Rgb, ColorsDbContext>();
            }

            //app.UseAuthentication();
            app.UseUser();

            if (HostingEnvironment.EnvironmentName == "Development") {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                });
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });


            //app.UseMvc(routeBuilder => {

            //    //routeBuilder.MapODataServiceRoute("ODataRoute", "odata", routeBuilder.GetEdmModel());

            //    // Workaround: https://github.com/OData/WebApi/issues/1175
            //    routeBuilder.EnableDependencyInjection();

            //    routeBuilder.Select().OrderBy().Filter().MaxTop(null);

            //});
        }
    }
}
