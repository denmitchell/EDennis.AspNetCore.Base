using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base
{
    public class SecureTokenServiceSettings : SecurityBaseSettings
    {
        public int IdentityServerPingFrequency { get; set; } = 5 * 60; // default = every 5 minutes
    }
}
