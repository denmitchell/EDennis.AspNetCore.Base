using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Linq;
using System.Threading.Tasks;
using EDennis.Samples.Utils;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace EDennis.AspNetCore.Base.Web {

    public abstract class ProgramBase<TStartup> : ProgramBase, IProgram
        where TStartup : class
        {
        public override Type Startup {
            get {
                return typeof(TStartup);
            }
        }
    }

    public abstract class ProgramBase : IProgram {

        public virtual IConfiguration Configuration {
            get {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
                var config = new ConfigurationBuilder();

                if (UsesEmbeddedConfigurationFiles) {
                    var assembly = Startup.Assembly;
                    var provider = new ManifestEmbeddedFileProvider(assembly);
                    config.AddJsonFile(provider, $"appsettings.json", true, true);
                    config.AddJsonFile(provider, $"appsettings.{env}.json", true, true);

                } else {
                    config.AddJsonFile($"appsettings.json", true, true);
                    config.AddJsonFile($"appsettings.{env}.json", true, true);
                }
                if (UsesLauncherConfigurationFile)
                    config.AddJsonFile($"appsettings.Launcher.json", true, true);

                config.AddEnvironmentVariables()
                    .AddCommandLine(new string[] { $"ASPNETCORE_ENVIRONMENT={env}" });

                return config.Build();
            }
        }
        public virtual bool UsesEmbeddedConfigurationFiles { get; } = true;
        public virtual bool UsesLauncherConfigurationFile { get; } = true;
        public virtual string ApisConfigurationSection { get; } = "Apis";
        public abstract Type Startup { get; }

        public Api Api { get; }

        public ProgramBase() {

            var apis = new Apis();
            var config = Configuration;
            var projectName = GetType().Assembly.GetName().Name.Replace(".Lib","");
            try {
                config.GetSection(ApisConfigurationSection).Bind(apis);
            } catch (Exception) {
                throw new ApplicationException($"Cannot bind to {ApisConfigurationSection} in Configuration.");
            }
            try {
                Api = apis.FirstOrDefault(a => a.Value.ProjectName == projectName).Value;
            } catch (Exception) {
                throw new ApplicationException($"Cannot bind to {ApisConfigurationSection}:{projectName} in Configuration.");
            }

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
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var urls = Api.Urls;
                    webBuilder
                    .UseConfiguration(Configuration)
                    .UseUrls(urls)
                    .UseStartup(Startup);
                });
            return builder;
        }

        #region CanPingAsync
        public static bool CanPingAsync<TProgram1>(TProgram1 program1)
            where TProgram1 : IProgram{
            var tasks = new Task[] {
                Task.Run(async ()=> { return await CanPingAsync(program1.Api.Host, program1.Api.MainPort.Value); })
            };
            Task.WhenAll(tasks);
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
            where TProgram5 : IProgram
            {
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
