using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class Profile {
        public Dictionary<string,string> Apis { get; set; }
        public Dictionary<string,string> ConnectionStrings { get; set; }
        public string MockClientId { get; set; }
        public string AutoLoginId { get; set; }
    }
}
