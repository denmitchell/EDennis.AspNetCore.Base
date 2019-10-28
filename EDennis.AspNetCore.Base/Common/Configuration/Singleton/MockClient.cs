using System.Collections.Generic;

namespace EDennis.AspNetCore.Base
{

    /// <summary>
    /// This class is designed to support IdentityServerMockDataLoader.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class MockClient {

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] Scopes { get; set; }

    }

}
