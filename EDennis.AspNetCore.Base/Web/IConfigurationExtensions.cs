using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;

namespace EDennis.AspNetCore.Base.Web {
    public static class IConfigurationExtensions {

        public static List<string> PrintConfigFilePaths(this IConfiguration config) {
            var paths = new List<string>();
            var fileProviders = ((IConfigurationRoot)config).Providers
                .Where(p => p.GetType() 
                    == typeof(JsonConfigurationProvider))
                .Select(p => p as JsonConfigurationProvider);
            foreach(var provider in fileProviders) {
                var pfProvider = provider.Source.FileProvider as PhysicalFileProvider;
                paths.Add(pfProvider.Root + provider.Source.Path);
            }
            return paths;
        }

    }
}
