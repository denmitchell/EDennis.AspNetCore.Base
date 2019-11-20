using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public interface IProgramBase {
        string ApisConfigurationSection { get; }
        IConfiguration Configuration { get; }
        Type Startup { get; }
        bool UsesEmbeddedConfigurationFiles { get; }
        bool UsesSharedConfigurationFile { get; }

        IHostBuilder CreateHostBuilder(string[] args);
        string[] GetUrls();
        void RunAsync(string[] args);
    }
}