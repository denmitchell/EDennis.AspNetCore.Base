using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ActiveMockClientSettings {

        private string _activeMockClientKey;

        /// <summary>
        /// The active mock client.
        /// NOTE: if left unspecified, this defaults to
        /// the first MockClient in the dictionary.
        /// </summary>
        public string ActiveMockClientKey { 
            get {                
                return _activeMockClientKey ?? MockClients?.Keys?.FirstOrDefault();
            }
            set {
                _activeMockClientKey = value;
            } 
        }
        public MockClientSettingsDictionary MockClients { get; set; }

        public MockClientSettings ActiveMockClient { 
            get {
                if (ActiveMockClientKey == null)
                    throw new ApplicationException($"Missing Configuration Setting: MockClient:ActiveMockClientKey.");
                if (!MockClients.ContainsKey(ActiveMockClientKey))
                    throw new ApplicationException($"Missing Configuration Setting: MockClient:MockClients:{ActiveMockClientKey}.");

                return MockClients[ActiveMockClientKey];
            } 
        }
    }
}
