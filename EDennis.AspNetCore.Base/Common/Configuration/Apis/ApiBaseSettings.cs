using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiBaseSettings {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int HttpsPort { get; set; }
        public int HttpPort { get; set; }
        public decimal Version { get; set; }

    }
}
