using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Common.Configuration.MockClient {
    public class MockClient {
        public string ActiveMockClientKey { get; set; }
        public MockClientSettingsDictionary MockClients { get; set; }

        public MockClientSettings ActiveMockClient { get {
                if (ActiveMockClientKey == null)
                    throw new ApplicationException($"Missing Configuration Setting: MockClient:ActiveMockClientKey.");
                if (!MockClients.ContainsKey(ActiveMockClientKey))
                    throw new ApplicationException($"Missing Configuration Setting: MockClient:MockClients:{ActiveMockClientKey}.");

                return MockClients[ActiveMockClientKey];
            } }
    }
}
