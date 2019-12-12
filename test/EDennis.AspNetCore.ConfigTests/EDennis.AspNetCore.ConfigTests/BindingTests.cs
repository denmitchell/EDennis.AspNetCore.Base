using EDennis.AspNetCore.Base.Testing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using Microsoft.Extensions.Configuration;
using EDennis.AspNetCore.Base;
using EDennis.NetCoreTestingUtilities.Extensions;
using EDennis.JsonUtils;
using System.Text.Json;
using System.Linq;
using System.ComponentModel;

namespace EDennis.AspNetCore.ConfigTests {
    public class BindingTests {
        private readonly ITestOutputHelper _output;
        public BindingTests(ITestOutputHelper output) {
            _output = output;
        }

        private static readonly ScopePropertiesSettings[] sps =
            new ScopePropertiesSettings[] {
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtNameClaim },
                    CopyHeaders = true,
                    CopyClaims = true,
                    AppendHostPath = false
                },
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> { UserSource.XUserHeader },
                    CopyHeaders = false,
                    CopyClaims = false,
                    AppendHostPath = true
                },
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtNameClaim, UserSource.SessionId },
                    CopyHeaders = true,
                    CopyClaims = true,
                    AppendHostPath = true
                },
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtSubjectClaim, UserSource.SessionId },
                    CopyHeaders = false,
                    CopyClaims = false,
                    AppendHostPath = false
                },
                new ScopePropertiesSettings {
                    UserSource = new HashSet<UserSource> {
                        UserSource.JwtNameClaim,
                        UserSource.JwtPreferredUserNameClaim,
                        UserSource.JwtSubjectClaim,
                        UserSource.SessionId,
                        UserSource.XUserHeader,
                        UserSource.XUserQueryString,
                        UserSource.OasisNameClaim,
                        UserSource.OasisEmailClaim,
                        UserSource.JwtEmailClaim,
                        UserSource.JwtPhoneClaim,
                        UserSource.JwtClientIdClaim
                    },
                    CopyHeaders = false,
                    CopyClaims = false,
                    AppendHostPath = true
                },

            };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
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


        private static readonly MockHeaderSettingsCollection[] mhsc =
            new MockHeaderSettingsCollection[] {
                new MockHeaderSettingsCollection {
                    {"X-Role", new MockHeaderSettings {
                        Values = new string[] { "readonly" },
                        ConflictResolution = MockHeaderConflictResolution.Overwrite }
                    },
                    {"X-User", new MockHeaderSettings {
                        Values = new string[] { "curly@stooges.org" },
                        ConflictResolution = MockHeaderConflictResolution.Overwrite }
                    }
                },
                new MockHeaderSettingsCollection {
                    {"X-Role", new MockHeaderSettings {
                        Values = new string[] { "user" },
                        ConflictResolution = MockHeaderConflictResolution.Union }
                    },
                    {"X-User", new MockHeaderSettings {
                        Values = new string[] { "larry@stooges.org" },
                        ConflictResolution = MockHeaderConflictResolution.Overwrite }
                    }
                },
                new MockHeaderSettingsCollection {
                    {"X-Role", new MockHeaderSettings {
                        Values = new string[] { "admin", "user" },
                        ConflictResolution = MockHeaderConflictResolution.Union }
                    },
                    {"X-User", new MockHeaderSettings {
                        Values = new string[] { "moe@stooges.org" },
                        ConflictResolution = MockHeaderConflictResolution.Overwrite }
                    }
                }
            };



        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void MockHeaders(int testCase) {
            var path = $"MockHeaders/{testCase}.json";
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            var actual = new MockHeaderSettingsCollection();
            config.Bind("MockHeaders",actual);

            var expected = mhsc[testCase];

            Assert.True(actual.IsEqualOrWrite(expected,_output, true));

        }

        private static readonly UserLoggerSettings[] ul =
            new UserLoggerSettings[] {
                new UserLoggerSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtNameClaim }
                },
                new UserLoggerSettings {
                    UserSource = new HashSet<UserSource> { UserSource.XUserHeader }
                },
                new UserLoggerSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtNameClaim, UserSource.SessionId }
                },
                new UserLoggerSettings {
                    UserSource = new HashSet<UserSource> { UserSource.JwtSubjectClaim, UserSource.SessionId }
                },
                new UserLoggerSettings {
                    UserSource = new HashSet<UserSource> {
                        UserSource.JwtNameClaim,
                        UserSource.JwtPreferredUserNameClaim,
                        UserSource.JwtSubjectClaim,
                        UserSource.SessionId,
                        UserSource.XUserHeader,
                        UserSource.XUserQueryString,
                        UserSource.OasisNameClaim,
                        UserSource.OasisEmailClaim,
                        UserSource.JwtEmailClaim,
                        UserSource.JwtPhoneClaim,
                        UserSource.JwtClientIdClaim
                    }
                },

            };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void UserLogger(int testCase) {
            var path = $"UserLogger/{testCase}.json";
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            var actual = new UserLoggerSettings();
            config.Bind("UserLogger", actual);

            var expected = ul[testCase];

            Assert.Equal(expected.UserSource, actual.UserSource);

        }



        private static readonly ActiveMockClientSettings[] amcs =
            new ActiveMockClientSettings[] {
                new ActiveMockClientSettings {
                    ActiveMockClientKey = "MockClient1",
                    MockClients = new MockClientSettingsDictionary {
                        {
                            "MockClient1", new MockClientSettings {
                            ClientId = "EDennis.Samples.SomeApi1",
                            ClientSecret = "some secret 1",
                            Scopes = new string[] { "some scope 1" }}
                        },
                        {
                            "MockClient2", new MockClientSettings {
                            ClientId = "EDennis.Samples.SomeApi2",
                            ClientSecret = "some secret 2",
                            Scopes = new string[] { "some scope 2a", "some scope 2b" }}
                        }
                    }
                },
                new ActiveMockClientSettings {
                    ActiveMockClientKey = "MockClient2",
                    MockClients = new MockClientSettingsDictionary {
                        {
                            "MockClient1", new MockClientSettings {
                            ClientId = "EDennis.Samples.SomeApi1",
                            ClientSecret = "some secret 1",
                            Scopes = new string[] { "some scope 1" }}
                        },
                        {
                            "MockClient2", new MockClientSettings {
                            ClientId = "EDennis.Samples.SomeApi2",
                            ClientSecret = "some secret 2",
                            Scopes = new string[] { "some scope 2a", "some scope 2b" }}
                        }
                    }
                }
            };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void MockClients(int testCase) {
            var path = $"MockClient/{testCase}.json";
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            var actual = new ActiveMockClientSettings();
            config.Bind("MockClient", actual);

            var expected = amcs[testCase];

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));

        }


        private static readonly HeadersToClaims[] htc =
            new HeadersToClaims[] {
                new HeadersToClaims {
                    PreAuthentication = new PreAuthenticationHeadersToClaims{
                        { "X-Role", "role" },
                        { "X-User", "name" }
                    }
                },
                new HeadersToClaims {
                    PreAuthentication = new PreAuthenticationHeadersToClaims{
                        { "X-UserScope", "user_scope" },
                    },
                    PostAuthentication = new PostAuthenticationHeadersToClaims{
                        { "X-Role", "role" },
                        { "X-User", "name" }

                    },
                }
            };

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void HeadersToClaims(int testCase) {
            var path = $"HeadersToClaims/{testCase}.json";
            var config = new ConfigurationBuilder()
                .AddJsonFile(path)
                .Build();
            var actual = new HeadersToClaims();
            config.Bind("HeadersToClaims", actual);

            var expected = htc[testCase];

            Assert.True(actual.IsEqualOrWrite(expected, _output, true));

        }



    }

}
