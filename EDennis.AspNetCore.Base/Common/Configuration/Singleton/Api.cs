using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class Api {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int HttpsPort { get; set; }
        public int HttpPort { get; set; }
        public bool ApiLauncher { get; set; }
        public string[] Scopes { get; set; }
    }
}
