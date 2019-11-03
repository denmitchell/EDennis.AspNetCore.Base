using System.Collections.Generic;

namespace EDennis.AspNetCore.Base
{
    public class AutoLogin {
        public bool IsActive { get; set; }
        public MockClaim[] Claims { get; set; }
    }


}
