using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Testing
{
    public class AutoLoginDictionary : Dictionary<string,AutoLoginProperties> {}

    public class AutoLoginProperties {
        public bool Default { get; set; }
        public MockClaim[] Claims { get; set; }
    }


}
