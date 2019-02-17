using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Testing {
    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class MockIdentityResourceDictionary : Dictionary<string,MockIdentityResourceProperties> {}

    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class MockIdentityResourceProperties {
        public string DisplayName { get; set; }
        public bool Required { get; set; } = false;
        public bool Emphasize { get; set; } = false;
        public string Claim { get; set; } //for space-delimited array
        public string[] Claims { get; set; }
    }
}
