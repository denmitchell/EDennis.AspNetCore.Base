using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class HeadersToClaims {
        public PreAuthenticationHeadersToClaims PreAuthentication { get; set;}
        public PostAuthenticationHeadersToClaims PostAuthentication { get; set; }
    }
}
