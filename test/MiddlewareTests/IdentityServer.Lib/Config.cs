// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static List<TestUser> GetUsers() {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "mike",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Mike"),
                        new Claim ("email","mike@example.com")
                    }
                }
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResources() {
            var name = new IdentityResource("name", new string[] { "name" });
            var role = new IdentityResource("role", new string[] { "role" });
            //var userDataResource = new IdentityResource("user_data",
            //    new[] { "name", "email", "user_scope", "role" });

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                role,
                name
            };
        }

        public static IEnumerable<ApiResource> GetApis() {
            return new List<ApiResource>
            {
                new ApiResource {
                    Name ="MockClientApi",
                    DisplayName="MockClientApi",
                    Scopes={
                        new Scope {
                            Name = "MockClientApi.*",
                            DisplayName = "MockClientApi.*"
                        },
                        new Scope {
                            Name = "MockClientApi.Person.*",
                            DisplayName = "MockClientApi.Person.*"
                        },
                        new Scope {
                            Name = "MockClientApi.Person.GetA",
                            DisplayName = "MockClientApi.Person.GetA"
                        },
                        new Scope {
                            Name = "MockClientApi.Person.GetB",
                            DisplayName = "MockClientApi.Person.GetB"
                        },
                        //configs tests...
                        new Scope {
                            Name = "MockClientApi.MockClient.*",
                            DisplayName = "MockClientApi.MockClient.*"
                        },
                        new Scope {
                            Name = "MockClientApi.MockClient.Get",
                            DisplayName = "MockClientApi.MockClient.Get"
                        },
                    }
                }
            };
        }

        internal class MockClient : Client {
            public MockClient() : base() {
                ClientClaimsPrefix = "";
                AllowedGrantTypes = GrantTypes.ClientCredentials;
                ClientSecrets = new Collection<Secret> { new Secret("secret".Sha256()) };
            }
        };

        public static IEnumerable<Client> GetClients() {
            return new List<Client> {
                new MockClient {
                    ClientId = "MockClientApi.Client1",
                    AllowedScopes = {
                        "MockClientApi.*",
                        "MockClientApi.Person.*",
                        "MockClientApi.Person.GetA",
                        "MockClientApi.Person.GetB"
                    }
                },
                new MockClient {
                    ClientId = "MockClientApi.Client2",
                    AllowedScopes = {
                        "MockClientApi.Person.*",
                    }
                },
                new MockClient {
                    ClientId = "MockClientApi.Client3",
                    AllowedScopes = {
                        "MockClientApi.Person.GetA",
                    }
                },
                new MockClient {
                    ClientId = "MockClientApi.Client4",
                    AllowedScopes = {
                        "MockClientApi.Person.GetB",
                    }
                }
            };
        }
    }
}