using EDennis.AspNetCore.Base.Testing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public static class IApplicationBuilderExtensions_Launcher {

        public static IApplicationBuilder UseLaunchers<TStartup1>(this IApplicationBuilder app, 
            IServiceProvider provider, IConfiguration config )
            where TStartup1 : class {
            if (!(provider.GetService<ILogger<ApiLauncherOld>>() is ILogger<ApiLauncherOld> logger)) {
                logger = new Logger<ApiLauncherOld>(new LoggerFactory());
            }
            var launcher = new ApiLauncherOld(config, logger);
            launcher.StartAsync<TStartup1>().Wait();
            return app;
        }

        public static IApplicationBuilder UseLaunchers<TStartup1, TStartup2>(this IApplicationBuilder app,
            IServiceProvider provider, IConfiguration config)
            where TStartup1: class
            where TStartup2: class {
            app.UseLaunchers<TStartup1>(provider,config);
            app.UseLaunchers<TStartup2>(provider, config);
            return app;
        }

        public static IApplicationBuilder UseLaunchers<TStartup1, TStartup2, TStartup3>(this IApplicationBuilder app,
            IServiceProvider provider, IConfiguration config)
            where TStartup1 : class
            where TStartup2 : class
            where TStartup3 : class {
            app.UseLaunchers<TStartup1>(provider, config);
            app.UseLaunchers<TStartup2>(provider, config);
            app.UseLaunchers<TStartup3>(provider, config);
            return app;
        }


        public static IApplicationBuilder UseLaunchers<TStartup1, TStartup2, TStartup3, TStartup4>(this IApplicationBuilder app,
            IServiceProvider provider, IConfiguration config)
            where TStartup1 : class
            where TStartup2 : class
            where TStartup3 : class
            where TStartup4 : class {
            app.UseLaunchers<TStartup1>(provider, config);
            app.UseLaunchers<TStartup2>(provider, config);
            app.UseLaunchers<TStartup3>(provider, config);
            app.UseLaunchers<TStartup4>(provider, config);
            return app;
        }

        public static IApplicationBuilder UseLaunchers<TStartup1, TStartup2, TStartup3, TStartup4, TStartup5>(this IApplicationBuilder app,
            IServiceProvider provider, IConfiguration config)
            where TStartup1 : class
            where TStartup2 : class
            where TStartup3 : class
            where TStartup4 : class
            where TStartup5 : class {
            app.UseLaunchers<TStartup1>(provider, config);
            app.UseLaunchers<TStartup2>(provider, config);
            app.UseLaunchers<TStartup3>(provider, config);
            app.UseLaunchers<TStartup4>(provider, config);
            app.UseLaunchers<TStartup5>(provider, config);
            return app;
        }

    }
}

