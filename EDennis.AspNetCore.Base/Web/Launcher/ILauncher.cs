using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Web {
    public interface ILauncher<TStartup>
        where TStartup : class {
        Action<WebHostBuilderContext, IConfigurationBuilder> ConfigureDelegate { get; set; }

        void Run(string[] args);
        Task<bool> RunAsync(ApiSettingsFacade api, ILogger logger);
        Task RunAsync(string[] args);
    }
}