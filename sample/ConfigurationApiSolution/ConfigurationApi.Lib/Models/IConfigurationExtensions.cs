using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib.Models {
    public static class IConfigurationExtensions {

        public static Dictionary<string, string> Flatten(this IConfiguration config) {
            var dict = new Dictionary<string, string>();
            config.GetChildren().AsParallel().ToList()
                .ForEach(x => x.Flatten(dict));
            return dict;
        }
        private static void Flatten(this IConfigurationSection section,
            Dictionary<string, string> dict, string parentKey = "") {
            if (section.Value == null)
                section.GetChildren().AsParallel().ToList()
                    .ForEach(x => x.Flatten(dict, $"{parentKey}{section.Key}:"));
            else
                dict.Add($"{parentKey}{section.Key}", section.Value);
        }

    }
}
