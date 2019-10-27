using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class Profile {
        public Dictionary<string,string> ApiKeys { get; set; }
        public Dictionary<string,string> ConnectionStringKeys { get; set; }
        public string MockClientKey { get; set; }
        public string AutoLoginKey { get; set; }
    }
}
