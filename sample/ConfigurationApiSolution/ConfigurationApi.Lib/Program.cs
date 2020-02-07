using EDennis.AspNetCore.Base.Web;
using Microsoft.Extensions.Configuration;
using System;

namespace ConfigurationApi.Lib {
    public class Program : ProgramBase<Startup> {

        /// <summary>
        /// Override the default builder to use an in-memory collection
        /// </summary>
        public override Func<string[], IConfigurationBuilder> AppConfigurationBuilderFunc { get; set; }
                = (args) => {
                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

                    var configBuilder = new ConfigurationBuilder();
                    configBuilder.AddInMemoryCollection(ConfigurationApiConfiguration.GetConfiguration(env));

                    configBuilder
                        .AddEnvironmentVariables()
                        .AddCommandLine(args)
                        .AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });

                    return configBuilder;
                };


    }
}


