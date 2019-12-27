
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base {
    public class HeadersToClaims : Dictionary<string,string> {
        public bool Enabled { get; set; } = false;
    }
}
