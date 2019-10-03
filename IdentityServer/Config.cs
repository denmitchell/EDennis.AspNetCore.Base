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
                    Username = "mike",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Mike"),
                        new System.Security.Claims.Claim ("email","mike@example.com"),
                        new System.Security.Claims.Claim ("role","EDennis.Samples.DefaultPoliciesMvc.Admin")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "carol",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Carol"),
                        new System.Security.Claims.Claim ("email","carol@example.com"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc")
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "greg",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Greg"),
                        new System.Security.Claims.Claim ("email","greg@example.com"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Position")
                    }
                },
                new TestUser
                {
                    SubjectId = "4",
                    Username = "marcia",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Marcia"),
                        new System.Security.Claims.Claim ("email","marcia@example.com"),
                        new System.Security.Claims.Claim ("role","EDennis.Samples.DefaultPoliciesMvc.NoDelete")
                    }
                },
                new TestUser
                {
                    SubjectId = "5",
                    Username = "peter",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Peter"),
                        new System.Security.Claims.Claim ("email","peter@example.com"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Position")
                    }
                },
                new TestUser
                {
                    SubjectId = "6",
                    Username = "jan",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Jan"),
                        new System.Security.Claims.Claim ("email","jan@example.com"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person")
                    }
                },
                new TestUser
                {
                    SubjectId = "7",
                    Username = "bobby",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Bobby"),
                        new System.Security.Claims.Claim ("email","bobby@example.com"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Index"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Details"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Position")
                    }
                },
                new TestUser
                {
                    SubjectId = "8",
                    Username = "cindy",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Cindy"),
                        new System.Security.Claims.Claim ("email","cindy@example.com"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Index"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Details"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Create"),
                        new System.Security.Claims.Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Edit")
                    }
                },
                new TestUser
                {
                    SubjectId = "9",
                    Username = "alice",
                    Password = "password",
                    Claims = new List<System.Security.Claims.Claim> {
                        new System.Security.Claims.Claim ("name","Alice"),
                        new System.Security.Claims.Claim ("email","alice@example.com"),
                        new System.Security.Claims.Claim ("role","EDennis.Samples.DefaultPoliciesMvc.Readonly")
                    }
                },
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources() {
            var name = new IdentityResource("name", new string[] { "name" });
            var userScope = new IdentityResource("user_scope", new string[] { "user_scope" });
            var role = new IdentityResource("role", new string[] { "role" });
            //var userDataResource = new IdentityResource("user_data",
            //    new[] { "name", "email", "user_scope", "role" });

            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                userScope,
                role,
                name
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
                new ApiResource{
                    Name ="EDennis.Samples.DefaultPoliciesMvc",
                    DisplayName="EDennis.Samples.DefaultPoliciesMvc",
                    Scopes={
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesMvc",
                            DisplayName = "EDennis.Samples.DefaultPoliciesMvc"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesMvc.ViaClaims",
                            DisplayName = "EDennis.Samples.DefaultPoliciesMvc.ViaClaims"
                        },
                    }
                },

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
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Role","Admin")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client2",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Role","Readonly")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client3",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.ViaClaims",
                    },
                    Claims = {
                        new System.Security.Claims.Claim("Role","NoDelete")
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
                        "user_scope",
                        "role",
                        "name"
                    }
                },
                new Client
                {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    ClientName = "EDennis.Samples.DefaultPoliciesMvc",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:65474/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:65474/signout-callback-oidc" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "user_scope",
                        "role",
                        "name",
                        "EDennis.Samples.DefaultPoliciesMvc",
                        "EDennis.Samples.DefaultPoliciesApi"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}