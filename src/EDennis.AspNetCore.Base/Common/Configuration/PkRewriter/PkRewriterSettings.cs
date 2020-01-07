using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base {
    public class PkRewriterSettings {
        public bool Enabled { get; set; } = false;
        public string DeveloperNameEnvironmentVariable { get; set; }
        public int BasePrefix { get; set; } = PkRewriter.BASE_PREFIX_DEFAULT;
        public Dictionary<string,int> DeveloperPrefixes { get; set; }
        public Dictionary<string, ExplicitIdSettings> ExplicitIds { get; set; }
    }
}
