using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public interface IProgram {
        Apis Apis { get; }
        Api Api { get; }
        Func<string[], IConfigurationBuilder> AppConfigurationBuilderFunc { get; set; }
        Func<string[], IConfigurationBuilder> HostConfigurationBuilderFunc { get; set; }
        IHostBuilder CreateHostBuilder(string[] args);
        IProgram Run(string[] args);
        string ApisConfigurationSection { get; }
    }
}