using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationApi.Lib {
    public static class ConfigurationApiConfiguration {
        public static IEnumerable<KeyValuePair<string,string>> GetConfiguration(string environment) {

            List<KeyValuePair<string, string>> config = new List<KeyValuePair<string, string>>();

            if(environment == "Development" || environment == "Local")
                config.AddRange(new KeyValuePair<string, string>[]
                    {
                        KeyValuePair.Create("ApiKey", "YPne5WW2J9q2W_kP^3jk$_2p3 @BYZPgJ"),
                        KeyValuePair.Create("Apis:ConfigurationApi:ProjectName", "ConfigurationApi"),
                        KeyValuePair.Create("Apis:ConfigurationApi:HttpsPort", "44312"),
                        KeyValuePair.Create("Apis:ConfigurationApi:HttpPort", "60712"),
                        KeyValuePair.Create("Apis:ConfigurationApi:Scopes:0", "ConfigurationApi.*"),
                        KeyValuePair.Create("DbContexts:ConfigurationDbContext:ConnectionString", "Server=(localdb)\\MSSQLLocalDB;Database=ConfigurationApi;Trusted_Connection=True;MultipleActiveResultSets=True;"),
                        KeyValuePair.Create("Logging:LogLevel:Default", "Information"),
                        KeyValuePair.Create("Logging:LogLevel:Microsoft", "Warning"),
                        KeyValuePair.Create("Logging:LogLevel:Microsoft.Hosting.Lifetime", "Information"),
                    });

            return config;

        }
    }
}
