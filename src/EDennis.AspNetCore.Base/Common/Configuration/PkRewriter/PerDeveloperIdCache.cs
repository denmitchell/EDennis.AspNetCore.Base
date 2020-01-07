using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class PerDeveloperIdCache : ConcurrentDictionary<string,ConcurrentDictionary<string,int>> {
    }
}
