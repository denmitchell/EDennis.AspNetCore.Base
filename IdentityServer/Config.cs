// Adapted from ...
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IdentityServer {
    public static class Config {
        public static List<TestUser> GetUsers() {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Alice Rodriguez"),
                        new System.Security.Claims.Claim ("email","alice@example.com"),
                        new System.Security.Claims.Claim ("role","admin")                        
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Bob Jones"),
                        new System.Security.Claims.Claim ("email","bob@example.com"),
                        new System.Security.Claims.Claim ("role","user")
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "huey",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Huey"),
                        new System.Security.Claims.Claim ("email","huey@example.com"),
                        new System.Security.Claims.Claim ("role","Admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "4",
                    Username = "dewey",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Dewey"),
                        new System.Security.Claims.Claim ("email","dewey@example.com"),
                        new System.Security.Claims.Claim ("role","Readonly")
                    }
                },
                new TestUser
                {
                    SubjectId = "5",
                    Username = "luis",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Luis"),
                        new System.Security.Claims.Claim ("email","luis@example.com"),
                        new System.Security.Claims.Claim ("role","NoDelete")
                    }
                },
                new TestUser
                {
                    SubjectId = "6",
                    Username = "luis2",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Luis2"),
                        new System.Security.Claims.Claim ("email","luis2@example.com"),
                        new System.Security.Claims.Claim ("scope","-EDennis.Samples.DefaultPoliciesApi.*.Delete*")
                    }
                },
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources() {
            var userDataResource = new IdentityResource("user_data",
                new[] { "name", "email", "role" });

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                userDataResource
            };
        }

        public static IEnumerable<ApiResource> GetApis() {
            return new List<ApiResource>
            {
                new ApiResource{
                    Name ="EDennis.Samples.Hr.InternalApi2",
                    DisplayName="EDennis.Samples.Hr.InternalApi2",
                    Scopes={
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.AgencyInvestigatorCheck",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.AgencyInvestigatorCheck"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.AgencyOnlineCheck",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.AgencyOnlineCheck"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.FederalBackgroundCheck",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.FederalBackgroundCheck"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.StateBackgroundCheck",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.StateBackgroundCheck"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.FederalBackgroundCheck.GetLastCheck",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.FederalBackgroundCheck.GetLastCheck"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.StateBackgroundCheck.GetLastCheck",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.StateBackgroundCheck.GetLastCheck"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi2.PreEmployment.GetLastChecks",
                            DisplayName = "EDennis.Samples.Hr.InternalApi2.PreEmployment.GetLastChecks"
                        }
                    }
                },
                new ApiResource{
                    Name ="EDennis.Samples.Hr.InternalApi1",
                    DisplayName="EDennis.Samples.Hr.InternalApi1",
                    Scopes={
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi1",
                            DisplayName = "EDennis.Samples.Hr.InternalApi1"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi1.Employee",
                            DisplayName = "EDennis.Samples.Hr.InternalApi1.Employee"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployee",
                            DisplayName = "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployee"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployees",
                            DisplayName = "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployees"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Hr.InternalApi1.Employee.CreateEmployee",
                            DisplayName = "EDennis.Samples.Hr.InternalApi1.Employee.CreateEmployee"
                        }
                    }
                },
                new ApiResource{
                    Name ="EDennis.Samples.DefaultPoliciesApi",
                    DisplayName="EDennis.Samples.DefaultPoliciesApi",
                    Scopes={
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.ViaClaims"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.GetAll",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.GetAll"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Get",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Get"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Post",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Post"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Put",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Put"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Delete",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Delete"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.GetAll",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.GetAll"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Get",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Get"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Post",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Post"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Put",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Put"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Delete",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Delete"
                        }
                    }
                },
                new ApiResource{
                    Name ="EDennis.Samples.Colors2Api",
                    DisplayName="EDennis.Samples.Colors2Api",
                    Scopes={
                        new Scope {
                            Name = "EDennis.Samples.Colors2Api",
                            DisplayName = "EDennis.Samples.Colors2Api"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Colors2Api.RgbController",
                            DisplayName = "EDennis.Samples.Colors2Api.RgbController"
                        },
                        new Scope {
                            Name = "EDennis.Samples.Colors2Api.HslController",
                            DisplayName = "EDennis.Samples.Colors2Api.HslController"
                        }
                    }
                },
                new ApiResource("api1", "My API")
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
                    ClientId = "EDennis.Samples.Hr.ExternalApi",
                    AllowedScopes = {
                        "EDennis.Samples.Hr.InternalApi1.Employee.CreateEmployee",
                        "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployee",
                        "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployees",
                        "EDennis.Samples.Hr.InternalApi2"
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.Hr.ExternalApi.Client1",
                    AllowedScopes = {
                        "EDennis.Samples.Hr.InternalApi1",
                        "EDennis.Samples.Hr.InternalApi2",
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.ExternalApi.Client2",
                    AllowedScopes = {
                        "EDennis.Samples.Hr.InternalApi1.Employee",
                        "EDennis.Samples.Hr.InternalApi2.AgencyInvestigatorCheck",
                        "EDennis.Samples.Hr.InternalApi2.AgencyOnlineCheck"
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.Hr.ExternalApi.Client3",
                    AllowedScopes = {
                        "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployee",
                        "EDennis.Samples.Hr.InternalApi1.Employee.GetEmployees",
                        "EDennis.Samples.Hr.InternalApi2.FederalBackgroundCheck.GetLastCheck",
                        "EDennis.Samples.Hr.InternalApi2.StateBackgroundCheck.GetLastCheck",
                        "EDennis.Samples.Hr.InternalApi2.PreEmployment.GetLastChecks"
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client1",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi",
                        "EDennis.Samples.Hr.InternalApi"
                    },
                    Claims = {
                        new System.Security.Claims.Claim("name","moe@stooges.org"),
                        new System.Security.Claims.Claim("Some Claim Type","Some Claim Value"),
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client2",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.Person",
                        "EDennis.Samples.Hr.InternalApi.Employee"
                    },
                    Claims = {
                        new System.Security.Claims.Claim("name","larry@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client3",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.Person.Get",
                        "EDennis.Samples.DefaultPoliciesApi.Position.Post",
                        "EDennis.Samples.Hr.InternalApi.Employee.GetEmployee",
                        "EDennis.Samples.Hr.InternalApi.Employee.GetEmployees"
                    },
                    Claims = {
                        new System.Security.Claims.Claim("name","curly@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client4",
                    AllowedScopes = {
                        "N/A"
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client5",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Scope","EDennis.Samples.DefaultPoliciesApi.*Get*")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client6",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Role","Readonly")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client7",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Role","NoDelete")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client8",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Scope","-EDennis.Samples.DefaultPoliciesApi.*Delete*")
                    }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        "openid",
                        "profile",
                        "user_data"
                    }
                },
            // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:44337/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:44337/signout-callback-oidc" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "user_data",
                        "EDennis.Samples.DefaultPoliciesApi"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
        }
    }
}