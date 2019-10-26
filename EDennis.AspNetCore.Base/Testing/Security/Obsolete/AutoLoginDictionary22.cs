using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Testing
{
    public class AutoLoginDictionary22 : Dictionary<string,AutoLoginProperties22> {}

    public class AutoLoginProperties22 {
        public bool Default { get; set; }
        public MockClaim[] Claims { get; set; }
    }


}
