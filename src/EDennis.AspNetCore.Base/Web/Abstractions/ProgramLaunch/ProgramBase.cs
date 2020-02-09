using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ConfigCore.ApiSource;
using ConfigCore.Extensions;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class ProgramBase<TStartup> : ProgramBase, IProgram
        where TStartup : class {
        public override Type Startup {
            get {
                return typeof(TStartup);
            }
        }
    }


    public abstract class ProgramBase : IProgram {
        public abstract string ProjectName { get; }
        public virtual Func<string[], IConfigurationBuilder> AppConfigurationBuilderFunc { get; set; }
        public virtual Func<string[], IConfigurationBuilder> HostConfigurationBuilderFunc { get; set; }
        public virtual bool UsesConfigurationApi { get; } = true;
        public virtual bool UsesEmbeddedConfigurationFiles { get; } = true;
        public virtual bool UsesLauncherConfigurationFile { get; } = true;

        /// <summary>
        /// Whether the project places all wwwroot artifacts in 
        /// wwwroot\{ProjectName}.
        /// </summary>
        public virtual bool UsesProjectRoot { get; } = true;
        public virtual string ApisConfigurationSection { get; } = "Apis";
        public abstract Type Startup { get; }

        private static ILogger logger;

        public Apis Apis { get; }
        public Api Api { get; }

        public ProgramBase() {

            if (AppConfigurationBuilderFunc == null)
                AppConfigurationBuilderFunc = (args) =>
                    CreateDefaultConfigurationBuilder(args);

            Apis = new Apis();
            var config = AppConfigurationBuilderFunc(new string[] { }).Build();
            var projectName = GetType().Assembly.GetName().Name.Replace(".Lib", "");
            try {
                config.GetSection(ApisConfigurationSection).Bind(Apis);
            } catch (Exception) {
                throw new ApplicationException($"Cannot bind to {ApisConfigurationSection} in Configuration.");
            }
            try {
                Api = Apis.FirstOrDefault(a => a.Value.ProjectName == projectName).Value;
                LogLaunch("Attempting to start {0} at {1}", projectName, $"{Api.Urls[0]};{Api.Urls[1]}");
            } catch (Exception) {
                throw new ApplicationException($"Cannot bind to {ApisConfigurationSection}:{projectName} in Configuration.");
            }

        }


        static ProgramBase() {
            var loggerFactory = LoggerFactory.Create(builder => {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddConsole()
                    .AddEventLog();
            });
            logger = loggerFactory.CreateLogger<ProgramBase>();
        }

        public IProgram Run(string[] args) {
            Task.Run(() => {
                var host = CreateHostBuilder(args).Build();
                host.RunAsync();
                LauncherUtils.Block(args);
            });
            //var pingable = CanPingAsync(Api.Host, Api.MainPort.Value).Result;
            return this;
        }


        public IHostBuilder CreateHostBuilder(string[] args) {

            var builder = Host.CreateDefaultBuilder(args);

            if (HostConfigurationBuilderFunc != null) {
                var hostConfigBuilder = HostConfigurationBuilderFunc(args);
                builder.ConfigureHostConfiguration(config => {
                    foreach (var source in hostConfigBuilder.Sources)
                        config.Add(source);
                });
            }

            var appConfigBuilder = AppConfigurationBuilderFunc(args);

            builder.ConfigureWebHostDefaults(webBuilder => {
                var urls = Api.Urls;
                var urlEnvVars = new Dictionary<string, string>
                    {
                        {"URLS", $"{urls[0]};{urls[1]}"},
                        {"HTTPS_PORT", Api.HttpsPort.ToString()}
                    };


                webBuilder
                    .ConfigureAppConfiguration((config) => {
                        config.Sources.Clear();
                        foreach (var source in appConfigBuilder.Sources)
                            config.Add(source);
                        config.AddInMemoryCollection(urlEnvVars);
                    })
                    .UseUrls(urls)
                    .UseStartup(Startup);

                if (UsesProjectRoot)
                    webBuilder.UseWebRoot($"ProjectRoot\\{ProjectName}\\wwwroot");


            });
            return builder;
        }

        public static void OpenBrowser(string url) {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Process.Start("xdg-open", url);  // Works ok on linux
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                Process.Start("open", url); // Not tested
            } else {
                throw new Exception("Cannot open browser");
            }
        }

        private static void LogLaunch(string message, params object[] messageArgs) {
            logger.LogInformation(string.Format(message, messageArgs));
        }


        private IConfigurationBuilder CreateDefaultConfigurationBuilder(string[] args) {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var apiUrl = Environment.GetEnvironmentVariable("ConfigurationApiUrl");
            var apiKey = Environment.GetEnvironmentVariable("ConfigurationApiKey");

            var configBuilder = new ConfigurationBuilder();


            if (UsesEmbeddedConfigurationFiles) {
                var assembly = Startup.Assembly;
                var provider = new ManifestEmbeddedFileProvider(assembly);
                if (UsesProjectRoot) {
                    configBuilder.AddJsonFile(provider, $"ProjectRoot\\{ProjectName}\\appsettings.json", true, true);
                    configBuilder.AddJsonFile(provider, $"ProjectRoot\\{ProjectName}\\appsettings.{env}.json", true, true);
                } else {
                    configBuilder.AddJsonFile(provider, $"appsettings.json", true, true);
                    configBuilder.AddJsonFile(provider, $"appsettings.{env}.json", true, true);
                }
            } else {
                if (UsesProjectRoot) {
                    configBuilder.AddJsonFile($"ProjectRoot\\{ProjectName}\\appsettings.json", true, true);
                    configBuilder.AddJsonFile($"ProjectRoot\\{ProjectName}\\appsettings.{env}.json", true, true);
                } else {
                    configBuilder.AddJsonFile($"appsettings.json", true, true);
                    configBuilder.AddJsonFile($"appsettings.{env}.json", true, true);
                }
            }

            if (UsesConfigurationApi)
                configBuilder.AddApiSource("ConfigurationApiUrl", "ApiKey", "ConfigurationApiKey", ProjectName, true);

            //if needed, to add/overwrite settings for a Launcher (during testing)
            if (UsesLauncherConfigurationFile)
                configBuilder.AddJsonFile($"appsettings.Launcher.json", true, true);


            configBuilder
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });

            return configBuilder;


        }


        #region CanPingAsync
        public static bool CanPingAsync<TProgram1>(TProgram1 program1)
            where TProgram1 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            LogLaunch("Successfully pinged {0}", $"{program1.Api.MainAddress}");

            return true;

        }

        public static bool CanPingAsync<TProgram1, TProgram2>(TProgram1 program1, TProgram2 program2)
            where TProgram1 : IProgram
            where TProgram2 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<TProgram1, TProgram2, TProgram3>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<TProgram1, TProgram2, TProgram3, TProgram4>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }



        public static bool CanPingAsync<TProgram1, TProgram2, TProgram3, TProgram4, TProgram5>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4, TProgram5 program5)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram
            where TProgram5 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program5.Api.Host, program5.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<
            TProgram1, TProgram2, TProgram3, TProgram4, TProgram5,
            TProgram6>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4, TProgram5 program5,
            TProgram6 program6)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram
            where TProgram5 : IProgram
            where TProgram6 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program5.Api.Host, program5.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program6.Api.Host, program6.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<
            TProgram1, TProgram2, TProgram3, TProgram4, TProgram5,
            TProgram6, TProgram7>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4, TProgram5 program5,
            TProgram6 program6, TProgram7 program7)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram
            where TProgram5 : IProgram
            where TProgram6 : IProgram
            where TProgram7 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program5.Api.Host, program5.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program6.Api.Host, program6.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program7.Api.Host, program7.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<
            TProgram1, TProgram2, TProgram3, TProgram4, TProgram5,
            TProgram6, TProgram7, TProgram8>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4, TProgram5 program5,
            TProgram6 program6, TProgram7 program7, TProgram8 program8)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram
            where TProgram5 : IProgram
            where TProgram6 : IProgram
            where TProgram7 : IProgram
            where TProgram8 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program5.Api.Host, program5.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program6.Api.Host, program6.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program7.Api.Host, program7.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program8.Api.Host, program8.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<
            TProgram1, TProgram2, TProgram3, TProgram4, TProgram5,
            TProgram6, TProgram7, TProgram8, TProgram9>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4, TProgram5 program5,
            TProgram6 program6, TProgram7 program7, TProgram8 program8, TProgram9 program9)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram
            where TProgram5 : IProgram
            where TProgram6 : IProgram
            where TProgram7 : IProgram
            where TProgram8 : IProgram
            where TProgram9 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program5.Api.Host, program5.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program6.Api.Host, program6.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program7.Api.Host, program7.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program8.Api.Host, program8.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program9.Api.Host, program9.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        public static bool CanPingAsync<
            TProgram1, TProgram2, TProgram3, TProgram4, TProgram5,
            TProgram6, TProgram7, TProgram8, TProgram9, TProgram10>(
            TProgram1 program1, TProgram2 program2, TProgram3 program3, TProgram4 program4, TProgram5 program5,
            TProgram6 program6, TProgram7 program7, TProgram8 program8, TProgram9 program9, TProgram10 program10)
            where TProgram1 : IProgram
            where TProgram2 : IProgram
            where TProgram3 : IProgram
            where TProgram4 : IProgram
            where TProgram5 : IProgram
            where TProgram6 : IProgram
            where TProgram7 : IProgram
            where TProgram8 : IProgram
            where TProgram9 : IProgram
            where TProgram10 : IProgram {
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program2.Api.Host, program2.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program3.Api.Host, program3.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program4.Api.Host, program4.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program5.Api.Host, program5.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program6.Api.Host, program6.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program7.Api.Host, program7.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program8.Api.Host, program8.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program9.Api.Host, program9.Api.MainPort.Value); }),
                Task.Run(async ()=> { return await CanPingAsync(program10.Api.Host, program10.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
            return true;
        }


        private static async Task<bool> CanPingAsync(string host, int port, int timeoutSeconds = 15) {

            var pingable = false;

            await Task.Run(() => {

                var sw = new Stopwatch();

                sw.Start();
                while (sw.ElapsedMilliseconds < (timeoutSeconds * 1000)) {
                    try {
                        using var tcp = new TcpClient(host, port);
                        var connected = tcp.Connected;
                        pingable = true;
                        break;
                    } catch (Exception ex) {
                        if (!ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                            throw ex;
                        else
                            Thread.Sleep(1000);
                    }

                }

            });
            return pingable;
        }

        #endregion

    }
}
