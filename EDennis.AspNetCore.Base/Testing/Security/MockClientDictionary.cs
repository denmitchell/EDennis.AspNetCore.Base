using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class MockClientDictionary : Dictionary<string,MockClientProperties> {}

    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class MockClientProperties {

        public string ClientId { get; set; }
        public bool Default { get; set; } = false;
        public bool Enabled { get; set; } = true;

        public Dictionary<string,string> Claims { get; set; } //each value is space-delimited array

        public string Scope { get; set; } //for space-delimited array
        public string[] Scopes { get; set; }

        public string Secret { get; set; }

        public string PostLogoutRedirectUri { get; set; } //for space-delimited array
        public string[] PostLogoutRedirectUris { get; set; }

        public string RedirectUri { get; set; } //for space-delimited array
        public string[] RedirectUris { get; set; }

        public string GrantType { get; set; } //for space-delimited array
        public string[] GrantTypes { get; set; }
    }

}
