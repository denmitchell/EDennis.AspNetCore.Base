using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base
{
    public class SecureTokenServiceSettings : SecureTokenServiceBaseSettings
    {
        public OidcSettings OidcSettings { get; set; }
        public OAuthSettings OAuthSettings { get; set; }
        
    }
}
