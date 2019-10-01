﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security {
    public class DefaultPoliciesOptions {

        public string ScopeClaimType { get; set; } = "Scope";
        public string NamedClaimPatternsType { get; set; } = "Role";

        /// <summary>
        /// NOTE: Exclusions are evaluated after all included scopes.
        /// NOTE: When only exclusions are present, application-level scope
        ///       is used as the base from which exclusions are applied.
        /// </summary>
        public string ExclusionPrefix { get; set; } = "-";

        /// <summary>
        /// NOTE: This can be used to configure roles for users.
        /// </summary>
        public Dictionary<string,List<string>> NamedClaimPatterns { get; set; }

    }
}
