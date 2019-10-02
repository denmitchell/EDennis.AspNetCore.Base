﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Security;
using EDennis.AspNetCore.Base.Web;
using EDennis.AspNetCore.Base.Web.Extensions;
using EDennis.Samples.DefaultPoliciesMvc.ApiClients;
using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.DefaultPoliciesMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {



            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            var spo = new SecurityOptions();
            Configuration.GetSection("Security").Bind(spo);

            //only if injected ... services.Configure<SecurityPolicyOptions>(opt => opt = spo);

            services.AddClientAuthenticationAndAuthorizationWithDefaultPolicies(Options.Create(spo));

            services.AddMvc(options => {
                options.Conventions.Add(new AddDefaultAuthorizationPolicyConvention(HostingEnvironment, Configuration));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


            services.AddScoped<ScopeProperties>();
            services.AddApiClients<IdentityServerClient>();
            services.AddApiClient<IDefaultPoliciesApiClient, DefaultPoliciesApiClient>();


            services.AddDbContext<AppDbContext>(options =>
                            options.UseSqlite("Data Source=hr.db")
                            );


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options => {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options => {
                    options.SignInScheme = "Cookies";

                    options.Authority = "http://localhost:51159";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "EDennis.Samples.DefaultPoliciesMvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("EDennis.Samples.DefaultPoliciesMvc");
                    options.Scope.Add("user_data");
                    options.Scope.Add("offline_access");

                });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseUser();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}