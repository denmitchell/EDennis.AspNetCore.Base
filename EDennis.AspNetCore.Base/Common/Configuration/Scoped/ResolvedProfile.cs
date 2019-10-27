using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ResolvedProfile {
        public string ProfileName { get; set; }
        public Profile Profile { get; set; }
        public Apis Apis { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public MockClient MockClient { get; set; }
        public AutoLogin AutoLogin { get; set; }

        public void Load(string profileKey, Profiles profiles, Apis apis, ConnectionStrings connectionStrings, MockClients mockClients, AutoLogins autoLogins) {

            ProfileName = profileKey;
            Profile profile = null;

            try {
                profile = profiles[profileKey];
            } catch { 
                throw new ApplicationException($"Cannot find configuration entry for Profiles:{profileKey}.");
            }

            if (profile.ApiKeys != null && profile.ApiKeys.Count > 0) {
                Apis = new Apis();
                foreach (var entry in profile.ApiKeys) {
                    try {
                        Apis.Add(entry.Key, Apis[entry.Value]);
                    } catch {
                        throw new ApplicationException($"Error trying to find Api with key = {entry.Value} in Apis section of Configuration");
                    }
                }
            }


            if (profile.ConnectionStringKeys != null && profile.ConnectionStringKeys.Count > 0) {
                ConnectionStrings = new ConnectionStrings();
                foreach (var entry in profile.ConnectionStringKeys) {
                    try {
                        ConnectionStrings.Add(entry.Key, ConnectionStrings[entry.Value]);
                    } catch {
                        throw new ApplicationException($"Error trying to find ConnectionString with key = {entry.Value} in ConnectionStrings section of Configuration");
                    }
                }
            }


            if (profile.MockClientKey != null)
                try {
                    MockClient = mockClients[profile.MockClientKey];
                } catch {
                    throw new ApplicationException($"Profiles section in Configuration does not contain a valid MockClient section with key {profile.MockClientKey}");
                }

            if (profile.AutoLoginKey != null)
                try {
                    AutoLogin = autoLogins[profile.AutoLoginKey];
                } catch {
                    throw new ApplicationException($"Profiles section in Configuration does not contain a valid profile section with key {profile.MockClientKey}");
                }

        }

    }

}
