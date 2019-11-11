using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class SecureTokenServiceBaseSettings : ApiBaseSettings {
        public string ClientSecret { get; set; }
        public int PingFrequency { get; set; } = 5 * 60; // default = every 5 minutes

    }
}
