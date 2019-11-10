using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiSettings {
        public string ProjectName { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int HttpsPort { get; set; }
        public int HttpPort { get; set; }
        public decimal Version { get; set; }
        public string[] Scopes { get; set; }
        public bool NeedsLaunched { get; set; }

        public ApiMappings Mappings { get; set; }
        public ApiSettingsFacade Facade { get; set; }
    }
}
