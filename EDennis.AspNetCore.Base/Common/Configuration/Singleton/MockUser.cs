using System.Collections.Generic;

namespace EDennis.AspNetCore.Base
{
    public class MockUser {
        public bool IsActive { get; set; }
        public MockClaim[] Claims { get; set; }
    }


}
