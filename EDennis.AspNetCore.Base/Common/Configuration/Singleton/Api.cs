using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class Api {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Embedded { get; set; }
        public string[] Scopes { get; set; }
    }
}
