// Slightly adapted from ...
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Web.Security;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace IdentityServer {
    public class Startup {

        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment environment, IConfiguration config) {
            Environment = environment;
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services) {


            var builder = services.AddIdentityServer()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetUsers())
                .AddProfileService<StaticClaimsProfileService>();

            if (Environment.IsDevelopment()) {
                //var dir = Environment.ContentRootPath;
                //var cert = new X509Certificate2($"{dir}/is4.pfx","is4",keyStorageFlags: X509KeyStorageFlags.EphemeralKeySet);
                //builder.AddSigningCredential(cert);
                builder.AddDeveloperSigningCredential(true,"temp.rsa");
            } else {
                throw new Exception("need to configure key material");
            }

            services.AddMvc();


        }

        public X509Certificate2 GetCert() {
            X509Certificate2 cert = null;
            using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine)) {
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    // Replace below with your cert's thumbprint
                    "975FAB88433D95CB677892F7B92ED53A204ED929",
                    false);

                // Get the first cert with the thumbprint
                if (certCollection.Count > 0) {
                    cert = certCollection[0];
                }
            }
            return cert;
        }


        public void Configure(IApplicationBuilder app) {
            if (Environment.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseIdentityServer();


            app.UseMvcWithDefaultRoute();
        }



    }
}