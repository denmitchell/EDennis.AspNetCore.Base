using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class HeadersToClaims {
        private PostAuthenticationHeadersToClaims _postAuthentication;
        public PreAuthenticationHeadersToClaims PreAuthentication { get; set;}

        public PostAuthenticationHeadersToClaims PostAuthentication { 
            get {
                return _postAuthentication ?? new PostAuthenticationHeadersToClaims {
                    { "X-User", "name" },
                    { "X-Role", "role" }
                };
            }
            set {
                _postAuthentication = value;
            } 
        }

    }
}
