using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {

    //X-Testing-Config: YY1-A0    -- only used for testing scenarios where entire db can be restored
    //X-Testing-Config: YY1-R0-U  -- new read-uncommitted transaction; preceded by rollback if R1 is active
    //                            -- most performant; useful for single-connection dbs and SQL Server RLS  
    //X-Testing-Config: YY1-R1-C  -- new read-committed transaction; preceeded by rollback if R0 is active
    //                            -- SQL Server default  
    //X-Testing-Config: YY1-R1    -- transaction-level not specified -- use database default
    //                            
    //X-Testing-Config: YY1-R0-S  -- new serializable transaction; preceeded by rollback if R1 is active
    //                            -- most likely to create deadlocks; SQLite default  
    //X-Testing-Config: YY1-R*    -- rollback only; no new transaction
    //X-Testing-Config: YY1-M0    -- new in
    //X-Testing-Config: YY1-M1
    //X-Testing-Config: YY1-M*


    /// <summary>
    /// Holds Test Configuration data transmitted via claims or headers.
    /// </summary>
    public class TestConfigParser {

        public TestConfigParser() { }

        public TestConfig Parse(string testConfigUnparsed) {

            var testConfig = new TestConfig();

            var components = testConfigUnparsed.Split('-');

            testConfig.ProfileName = components[0];


            if (components.Length > 1) {
                try {
                    testConfig.ConnectionType = ConnectionTypeExtensions.EnumValue(components[1][0]);
                } catch {
                    throw new ApplicationException($"For the provided TestConfig ({testConfigUnparsed}), the second component (connection type) does not have a valid value (A, R, or M)");
                }
                try {
                    testConfig.ToggleValue = ToggleValueExtensions.EnumValue(components[1][1]);
                } catch {
                    throw new ApplicationException($"For the provided TestConfig ({testConfigUnparsed}), the second component does not have a valid toggle value (0, 1, or *)");
                }
            }

            if (components.Length > 2) {
                try {
                    testConfig.IsolationLevel = IsolationLevelExtensions.EnumValue(components[1][0]);
                } catch {
                    throw new ApplicationException($"For the provided TestConfig ({testConfigUnparsed}), the third component (isolation level) does not have a valid value (U, C, or S)");
                }
            }

            return testConfig;

        }


        //TODO: explore use of IOptionsMonitor here, rather than direct access to IConfiguration
        public ProfileConfiguration GetProfileConfiguration(TestConfig testConfig, IConfiguration appConfig) {

            var profileConfiguration = new ProfileConfiguration();
            var profile = new Profile();

            try {
                appConfig.Bind($"Profiles:{testConfig.ProfileName}", profile);
            } catch {
                throw new ApplicationException($"Profiles section in Configuration does not contain a valid profile section with key {testConfig.ProfileName}");
            }


            try {
                if (profile.MockClient != null) {
                    profileConfiguration.MockClient = new MockClient();
                    appConfig.Bind($"MockClients:{profile.MockClient}", profileConfiguration.MockClient);
                }
            } catch {
                throw new ApplicationException($"Profiles section in Configuration does not contain a valid MockClient section with key {profile.MockClient}");
            }

            try {
                if (profile.AutoLogin != null) {
                    profileConfiguration.AutoLogin = new AutoLogin();
                    appConfig.Bind($"AutoLogins:{profile.AutoLogin}", profileConfiguration.AutoLogin);
                }
            } catch {
                throw new ApplicationException($"Profiles section in Configuration does not contain a valid profile section with key {profile.AutoLogin}");
            }

            return profileConfiguration;

        }


    }
}
