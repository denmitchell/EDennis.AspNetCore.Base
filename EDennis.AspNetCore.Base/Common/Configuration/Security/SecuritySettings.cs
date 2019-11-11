using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base
{
    public class SecuritySettings : SecurityBaseSettings
    {
        public AuthenticationSettings Authentication { get; set; }
        public SecureTokenServiceSettings TokenService { get; set; }
    }
}
