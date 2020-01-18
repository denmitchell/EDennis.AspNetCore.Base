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
                    ClientId = "Hr.RazorApp",
                    Username="mike",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="carol",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="greg",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.Person.*"),
                        new Claim("user_scope","Hr.RazorApp.Address.*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="marcia",
                    Claims = new List<Claim> {
                        new Claim("user_scope","-Hr.RazorApp.*Delete*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="peter",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.Person.*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="jan",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.Address.*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="bobby",
                    Claims = new List<Claim> {
                        new Claim ("user_scope","Hr.RazorApp.Person.Index"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Details"),
                        new Claim ("user_scope","Hr.RazorApp.Address.*")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="cindy",
                    Claims = new List<Claim> {
                        new Claim ("user_scope","Hr.RazorApp.Person.Index"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Details"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Create"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Edit")
                    }
                },
                new UserClientClaimSpec {
                    ClientId = "Hr.RazorApp",
                    Username="alice",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.*Index*,Hr.RazorApp.*Details*")
                    }
                }
            };



        public static IEnumerable<IdentityResource> GetIdentityResources() {
            var name = new IdentityResource("name", new string[] { "name" });
            var userScope = new IdentityResource("user_scope", new string[] { "user_scope" });
            var role = new IdentityResource("role", new string[] { "role" });

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
                    Name ="Hr.Api",
                    DisplayName="Hr.Api",
                    Scopes={
                        new Scope {
                            Name = "Hr.Api.*",
                            DisplayName = "Hr.Api.*"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.*",
                            DisplayName = "Hr.Api.Person.*"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.Index",
                            DisplayName = "Hr.Api.Person.Index"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.Details",
                            DisplayName = "Hr.Api.Person.Details"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.Create",
                            DisplayName = "Hr.Api.Person.Create"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.Edit",
                            DisplayName = "Hr.Api.Person.Edit"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.Delete",
                            DisplayName = "Hr.Api.Person.Delete"
                        },
                        new Scope {
                            Name = "Hr.Api.Person.Exists",
                            DisplayName = "Hr.Api.Person.Exists"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.*",
                            DisplayName = "Hr.Api.Address.*"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.Index",
                            DisplayName = "Hr.Api.Address.Index"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.Details",
                            DisplayName = "Hr.Api.Address.Details"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.Create",
                            DisplayName = "Hr.Api.Address.Create"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.Edit",
                            DisplayName = "Hr.Api.Address.Edit"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.Delete",
                            DisplayName = "Hr.Api.Address.Delete"
                        },
                        new Scope {
                            Name = "Hr.Api.Address.Exists",
                            DisplayName = "Hr.Api.Address.Exists"
                        }
                    }
                },
                new ApiResource{
                    Name ="Hr.RazorApp",
                    DisplayName="Hr.RazorApp",
                    Scopes={
                        new Scope {
                            Name = "Hr.RazorApp.*",
                            DisplayName = "Hr.RazorApp.*"
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
                    ClientId = "Hr.Api.Client1",
                    AllowedScopes = {
                        "Hr.Api.*",
                        "EDennis.Samples.Hr.InternalApi.*"
                    },
                    Claims = {
                        new Claim("name","moe@stooges.org"),
                        new Claim("Some Claim Type","Some Claim Value")
                    }
                },
                new MockClient {
                    ClientId = "Hr.Api.Client2",
                    AllowedScopes = {
                        "Hr.Api.Person.*",
                        "EDennis.Samples.Hr.InternalApi.Employee.*"
                    },
                    Claims = {
                        new Claim("name","larry@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "Hr.Api.Client3",
                    AllowedScopes = {
                        "Hr.Api.Person.Index",
                        "Hr.Api.Person.Details",
                        "Hr.Api.Address.Create"
                    },
                    Claims = {
                        new Claim("name","curly@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "Hr.Api.Client4",
                    AllowedScopes = {
                        "N/A"
                    }
                },
                new Client
                {
                    ClientId = "Hr.RazorApp",
                    ClientName = "Hr.RazorApp",
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
                        "Hr.Api.*"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}