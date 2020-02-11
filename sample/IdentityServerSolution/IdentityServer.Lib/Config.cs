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

        public class AppUserClaim {
            public string AppClientId { get; set; }
            public string Username { get; set; }
            public List<Claim> Claims { get; set; }
        }

        public static IEnumerable<AppUserClaim> UserClientClaims { get; } =
            new AppUserClaim[] {
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="mike",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="carol",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="greg",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.Person.*"),
                        new Claim("user_scope","Hr.RazorApp.Address.*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="marcia",
                    Claims = new List<Claim> {
                        new Claim("user_scope","-Hr.RazorApp.*Delete*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="peter",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.Person.*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="jan",
                    Claims = new List<Claim> {
                        new Claim("user_scope","Hr.RazorApp.Address.*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="bobby",
                    Claims = new List<Claim> {
                        new Claim ("user_scope","Hr.RazorApp.Person.Index"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Details"),
                        new Claim ("user_scope","Hr.RazorApp.Address.*")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
                    Username="cindy",
                    Claims = new List<Claim> {
                        new Claim ("user_scope","Hr.RazorApp.Person.Index"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Details"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Create"),
                        new Claim ("user_scope","Hr.RazorApp.Person.Edit")
                    }
                },
                new AppUserClaim {
                    AppClientId = "Hr.RazorApp",
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
                    Name ="Hr.RepoApi",
                    DisplayName="Hr.RepoApi",
                    Scopes={
                        new Scope {
                            Name = "Hr.RepoApi.*",
                            DisplayName = "Hr.RepoApi.*"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.*",
                            DisplayName = "Hr.RepoApi.Person.*"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.Index",
                            DisplayName = "Hr.RepoApi.Person.Index"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.Details",
                            DisplayName = "Hr.RepoApi.Person.Details"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.Create",
                            DisplayName = "Hr.RepoApi.Person.Create"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.Edit",
                            DisplayName = "Hr.RepoApi.Person.Edit"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.Delete",
                            DisplayName = "Hr.RepoApi.Person.Delete"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Person.Exists",
                            DisplayName = "Hr.RepoApi.Person.Exists"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.*",
                            DisplayName = "Hr.RepoApi.Address.*"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.Index",
                            DisplayName = "Hr.RepoApi.Address.Index"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.Details",
                            DisplayName = "Hr.RepoApi.Address.Details"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.Create",
                            DisplayName = "Hr.RepoApi.Address.Create"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.Edit",
                            DisplayName = "Hr.RepoApi.Address.Edit"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.Delete",
                            DisplayName = "Hr.RepoApi.Address.Delete"
                        },
                        new Scope {
                            Name = "Hr.RepoApi.Address.Exists",
                            DisplayName = "Hr.RepoApi.Address.Exists"
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
                    ClientId = "Hr.RepoApi.Client1",
                    AllowedScopes = {
                        "Hr.RepoApi.*",
                        "EDennis.Samples.Hr.InternalApi.*"
                    },
                    Claims = {
                        new Claim("name","moe@stooges.org"),
                        new Claim("Some Claim Type","Some Claim Value")
                    }
                },
                new MockClient {
                    ClientId = "Hr.RepoApi.Client2",
                    AllowedScopes = {
                        "Hr.RepoApi.Person.*",
                    },
                    Claims = {
                        new Claim("name","larry@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "Hr.RepoApi.Client3",
                    AllowedScopes = {
                        "Hr.RepoApi.Person.Index",
                        "Hr.RepoApi.Person.Details",
                        "Hr.RepoApi.Address.Create"
                    },
                    Claims = {
                        new Claim("name","curly@stooges.org")
                    }
                },
                new MockClient {
                    ClientId = "Hr.RepoApi.Client4",
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
                        "Hr.RepoApi.*"
                    },
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }
}