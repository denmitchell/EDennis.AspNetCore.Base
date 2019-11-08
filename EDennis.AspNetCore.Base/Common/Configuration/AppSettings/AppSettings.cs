using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base {
    public class AppSettings {

        public string Instruction { get; set; }
        public bool PreAuthentication { get; set; }

        public Apis Apis { get; set; }

        public EFContexts EFContexts { get; set; }

        public string ActiveMockClientKey { get; set; }
        public MockClient ActiveMockClient {
            get {
                return MockClients[ActiveMockClientKey];
            }
        }

        public MockClients MockClients { get; set; }
        public MockClaim[] MockClaims { get; set; }
    }
}
