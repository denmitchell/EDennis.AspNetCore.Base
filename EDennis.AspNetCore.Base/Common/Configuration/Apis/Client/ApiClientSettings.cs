using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiClientSettings : ApiBaseSettings {

        public ApiMappings Mappings { get; set; }
        public string[] Scopes { get; set; }

    }
}
