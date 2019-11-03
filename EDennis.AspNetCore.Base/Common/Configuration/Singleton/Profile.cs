using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class Profile {
        public Apis Apis { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public MockClient MockClient { get; set; }
        public AutoLogin AutoLogin { get; set; }
    }
}
