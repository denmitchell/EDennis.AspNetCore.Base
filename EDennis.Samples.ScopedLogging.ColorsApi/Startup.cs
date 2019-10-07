using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.Samples.ScopedLogging.ColorsApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using S = Serilog;
using M = Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Serilog;
using Serilog.Events;
using EDennis.AspNetCore.Base;
using EDennis.Samples.ScopedLogging.ColorsApi.Middleware;
using EDennis.Samples.ScopedLogging.ColorsApi.Logging;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EDennis.Samples.ScopedLogging.ColorsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public IServiceCollection Services { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            //services.RemoveAll(typeof(M.ILoggerFactory));
            //services.RemoveAll(typeof(M.ILogger<>));

            services.AddScoped<ScopeProperties>();
            services.AddSingleton<ILoggerChooser>(new DefaultLoggerChooser());
            services.AddSingleton(typeof(ILogger<>), typeof(TraceLogger<>));


            services.AddDbContext<ColorDbContext>(options =>
                            options.UseSqlite($"Data Source={Environment.ContentRootPath}/color.db")
                            );

            if (Environment.EnvironmentName == "Development") {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Color API", Version = "v1" });
                });
            }

            Services = services;

            //ServiceProvider = services.BuildServiceProvider();

            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAutoLogin();

            app.UseMiddleware<SimpleTraceLoggerTestMiddleware>();

            //app.Use(async (context, next) =>
            //{
            //    var provider = Services.BuildServiceProvider();
            //    var scopeProperties = provider.GetRequiredService<ScopeProperties>();
            //    if (context.Request.Query.ContainsKey("logger")) {
            //        scopeProperties.LoggerIndex = int.Parse(context.Request.Query["logger"]);
            //    }
            //    await next();
            //});


            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            if (env.EnvironmentName == "Development") {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                });
            }

        }
    }
}
