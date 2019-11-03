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
        public ConnectionStrings ConnectionStrings { get; set; }
        public MockClient ActiveMockClient {
            get {
                return MockClients?.FirstOrDefault(x => x.Value.IsActive).Value;
            }
        }
        public AutoLogin ActiveAutoLogin {
            get {
                return AutoLogins?.FirstOrDefault(x => x.Value.IsActive).Value;
            }
        }

        public MockClients MockClients { get; set; }
        public AutoLogins AutoLogins { get; set; }
    }
}
