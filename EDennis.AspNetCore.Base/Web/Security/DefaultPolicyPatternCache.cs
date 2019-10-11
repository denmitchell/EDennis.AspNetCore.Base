using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Security {
    public class DefaultPolicyPatternCache : ConcurrentNestedDictionary<string,string,MatchType> {
    }
}
