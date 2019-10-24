using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ProfileConfiguration {
        public Profile Profile { get; set; }
        public MockClient MockClient { get; set; }
        public AutoLogin AutoLogin { get; set; }
    }
}
