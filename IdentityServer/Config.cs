// Adapted from ...
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;

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
                    Claims = new List<Claim> {
                        new Claim ("name","Mike"),
                        new Claim ("email","mike@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "carol",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Carol"),
                        new Claim ("email","carol@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "3",
                    Username = "greg",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Greg"),
                        new Claim ("email","greg@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "4",
                    Username = "marcia",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Marcia"),
                        new Claim ("email","marcia@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "5",
                    Username = "peter",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Peter"),
                        new Claim ("email","peter@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "6",
                    Username = "jan",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Jan"),
                        new Claim ("email","jan@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "7",
                    Username = "bobby",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Bobby"),
                        new Claim ("email","bobby@example.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "8",
                    Username = "cindy",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Cindy"),
                        new Claim ("email","cindy@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "9",
                    Username = "alice",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Alice"),
                        new Claim ("email","alice@example.com"),
                    }
                },
            };
        }

        public class UserClientClaimSpec {
            public string ClientId { get; set; }
            public string Username { get; set; }
            public List<Claim> Claims { get; set; }
        }

        public static IEnumerable<UserClientClaimSpec> UserClientClaims { get; } =
            new UserClientClaimSpec[] {
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="mike",
                    Claims = new List<Claim> { 
                        new Claim("role","EDennis.Samples.DefaultPoliciesMvc.Admin")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="carol",
                    Claims = new List<Claim> {
                        new Claim("user_scope","EDennis.Samples.DefaultPoliciesMvc")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="greg",
                    Claims = new List<Claim> {
                        new Claim("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person"),
                        new Claim("user_scope","EDennis.Samples.DefaultPoliciesMvc.Position")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="marcia",
                    Claims = new List<Claim> {
                        new Claim("role","EDennis.Samples.DefaultPoliciesMvc.NoDelete")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="peter",
                    Claims = new List<Claim> {
                        new Claim("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="jan",
                    Claims = new List<Claim> {
                        new Claim("user_scope","EDennis.Samples.DefaultPoliciesMvc.Position")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="bobby",
                    Claims = new List<Claim> {
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Index"),
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Details"),
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Position")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="cindy",
                    Claims = new List<Claim> {
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Index"),
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Details"),
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Create"),
                        new Claim ("user_scope","EDennis.Samples.DefaultPoliciesMvc.Person.Edit")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "EDennis.Samples.DefaultPoliciesMvc",
                    Username="alice",
                    Claims = new List<Claim> {
                        new Claim("role","EDennis.Samples.DefaultPoliciesMvc.Readonly")
                    }
                }
            };
        


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
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Index",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Index"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Details",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Details"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Create",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Create"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Edit",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Edit"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Delete",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Delete"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Person.Exists",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Person.Exists"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Index",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Index"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Details",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Details"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Create",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Create"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Edit",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Edit"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Delete",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Delete"
                        },
                        new Scope {
                            Name = "EDennis.Samples.DefaultPoliciesApi.Position.Exists",
                            DisplayName = "EDennis.Samples.DefaultPoliciesApi.Position.Exists"
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
                        "EDennis.Samples.DefaultPoliciesApi",
                        "EDennis.Samples.Hr.InternalApi"
                    },
                    Claims = {
                        new Claim("name","moe@stooges.org"),
                        new Claim("Some Claim Type","Some Claim Value")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client2",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.Person",
                        "EDennis.Samples.Hr.InternalApi.Employee"
                    },
                    Claims = {
                        new Claim("name","larry@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client3",
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.Person.Index",
                        "EDennis.Samples.DefaultPoliciesApi.Person.Details",
                        "EDennis.Samples.DefaultPoliciesApi.Position.Create"
                    },
                    Claims = {
                        new Claim("name","curly@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "EDennis.Samples.DefaultPoliciesApi.Client4",
                    AllowedScopes = {
                        "N/A"
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
                        //"EDennis.Samples.DefaultPoliciesMvc",
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