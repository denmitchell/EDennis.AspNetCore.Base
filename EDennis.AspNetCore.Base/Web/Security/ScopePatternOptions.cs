using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security {
    public class ScopePatternOptions {

        public string UserScopePrefix { get; set; }

        public bool IsOidc { get; set; } = false;

        /// <summary>
        /// NOTE: Exclusions are evaluated after all included scopes.
        /// NOTE: When only exclusions are present, application-level scope
        ///       is used as the base from which exclusions are applied.
        /// </summary>
        public string ExclusionPrefix { get; set; } = "-";

    }
}
