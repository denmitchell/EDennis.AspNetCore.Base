using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base {
    public class AppSettings {

        public ScopePropertiesSettings ScopeProperties { get; set; } = new ScopePropertiesSettings();

        public ApiSettingsDictionary Apis { get; set; }

        public DbContextSettingsDictionary DbContexts { get; set; }

        public string ActiveMockClientKey { get; set; }

        public MockClientSettings ActiveMockClient {
            get {
                if (MockClients == null || ActiveMockClientKey == null || !MockClients.ContainsKey(ActiveMockClientKey))
                    return null;
                else
                    return MockClients[ActiveMockClientKey];
            }
        }

        public MockClientSettingsDictionary MockClients { get; set; }
        public MockHeaderSettingsCollection MockHeaders { get; set; }

        public AppSettingsMappings Mappings { get; set; }

        public SecuritySettings Security { get; set; }

    }
}
