using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security {
    public class ScopePatternOptions {

        public string UserScopePrefix { get; internal set; }

        public bool IsOidc { get; internal set; } = false;

        public string PatternClaimType { get; set; } = "role";

        /// <summary>
        /// NOTE: Exclusions are evaluated after all included scopes.
        /// NOTE: When only exclusions are present, application-level scope
        ///       is used as the base from which exclusions are applied.
        /// </summary>
        public string ExclusionPrefix { get; set; } = "-";

        public string[] GloballyIgnoredScopes { get; set; }

        /// <summary>
        /// NOTE: This can be used to configure roles for users.
        /// </summary>
        public Dictionary<string,string[]> NamedPatterns { get; set; }

    }
}
