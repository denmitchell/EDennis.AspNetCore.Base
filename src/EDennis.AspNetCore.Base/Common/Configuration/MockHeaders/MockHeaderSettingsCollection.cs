using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base {
    public class MockHeaderSettingsCollection : Dictionary<string,MockHeaderSettings>, IDictionary {

    }
}
