using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using Microsoft.Extensions.Configuration;
using EDennis.AspNetCore.Base;

namespace EDennis.AspNetCore.ConfigTests {
    public class BindingTests {
        private readonly ITestOutputHelper _output;
        public BindingTests(ITestOutputHelper output) {
            _output = output;
        }

        private static ScopePropertiesSettings[] sps =
            new ScopePropertiesSettings[] {
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtNameClaim },
                    CopyHeaders = true,
                    CopyClaims = true,
                    AppendHostPath = true
                },
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtSubjectClaim, UserSource.JwtNameClaim, UserSource.SessionId },
                    CopyHeaders = false,
                    CopyClaims = false,
                    AppendHostPath = false
                },

            };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void ScopeProperties(int testCase) {
            var path = $"ScopeProperties/{testCase}.json";
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            var actual = new ScopePropertiesSettings();
            config.Bind("ScopeProperties",actual);

            var expected = sps[testCase];

            Assert.Equal(expected.UserSource, actual.UserSource);
            Assert.Equal(expected.CopyHeaders, actual.CopyHeaders);
            Assert.Equal(expected.CopyClaims, actual.CopyClaims);
            Assert.Equal(expected.AppendHostPath, actual.AppendHostPath);

        }


        private static MockHeaderSettingsCollection[] mhsc =
            new MockHeaderSettingsCollection[] {
                new MockHeaderSettingsCollection {
                    {"X-User", new MockHeaderSettings {
                        Values = new string[] { "moe@stooges.org" },
                        ConflictResolution = MockHeaderConflictResolution.Overwrite }
                    },
                    {"X-Role", new MockHeaderSettings {
                        Values = new string[] { "admin", "user" },
                        ConflictResolution = MockHeaderConflictResolution.Union }
                    },
                },
                new MockHeaderSettingsCollection {
                    {"X-User", new MockHeaderSettings {
                        Values = new string[] { "larry@stooges.org" },
                        ConflictResolution = MockHeaderConflictResolution.Overwrite }
                    },
                    {"X-Role", new MockHeaderSettings {
                        Values = new string[] { "user" },
                        ConflictResolution = MockHeaderConflictResolution.Union }
                    },
                }
            };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void MockHeaders(int testCase) {
            var path = $"MockHeaders/{testCase}.json";
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            var actual = new MockHeaderSettingsCollection();
            config.Bind("MockHeaders",actual);

            var expected = mhsc[testCase];

            foreach(var key in expected.Keys) {
                var e = expected[key];
                var a = actual[key];
                Assert.Equal(e.Values, a.Values);
                Assert.Equal(e.ConflictResolution, a.ConflictResolution);
            }
        }


    }
}
