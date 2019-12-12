using EDennis.AspNetCore.Base.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class OAuth {

        public string ClientSecret { get; set; }

        public string Authority { get; set; }
        public string Audience { get; set; }

        public bool RequireHttpsMetadata { get; set; } = false;
        public bool SaveTokens { get; set; } = true;
        public string ExclusionPrefix { get; set; } = "-";
        public bool ClearDefaultInboundClaimTypeMap { get; set; } = true;
    }
}
