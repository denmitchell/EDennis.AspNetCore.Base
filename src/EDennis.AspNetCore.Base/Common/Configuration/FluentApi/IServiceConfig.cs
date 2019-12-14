using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base {
    public interface IServiceConfig {
        IConfiguration Configuration { get; }
        IServiceCollection Services { get; }

        T Bind<T>(string path) where T : class, new();
        void Configure<T>(string path) where T : class, new();
        T BindAndConfigure<T>(string path) where T : class, new();


        //TODO: Not Needed?
        T GetObject<T>(string path) where T : class, new();

        //TODO: Not Needed?
        T GetParentObject<T>(string path) where T : class, new();


    }
}