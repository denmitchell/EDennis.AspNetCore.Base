using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ResolvedProfile {
        public string ProfileName { get; set; }
        public Profile Profile { get; set; }
        public string MockClientId { get; set; }
        public MockClient MockClient { get; set; }
        public string AutoLoginId { get; set; }
        public AutoLogin AutoLogin { get; set; }

        public void Load(string profileName, Profile profile, MockClients mockClients, AutoLogins autoLogins) {

            ProfileName = profileName;
            MockClientId = profile.MockClientId;
            AutoLoginId = profile.AutoLoginId;

            try {
                if (MockClientId != null)
                    MockClient = mockClients[MockClientId];
            } catch {
                throw new ApplicationException($"Profiles section in Configuration does not contain a valid MockClient section with key {MockClientId}");
            }

            try {
                if (AutoLoginId != null)
                    AutoLogin = autoLogins[AutoLoginId];                
            } catch {
                throw new ApplicationException($"Profiles section in Configuration does not contain a valid profile section with key {AutoLoginId}");
            }

        }

    }

}
