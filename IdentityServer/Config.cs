// Adapted from ...
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;


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
                }
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
                    Name ="EDennis.Samples.DefaultPoliciesApi",
                    DisplayName="EDennis.Samples.DefaultPoliciesApi",
                    Scopes={
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
                        },
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients() {
                return new List<Client>
                {
                new Client
                {
                    ClientId = "Client1",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials, 

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi"
                    }
                },
                new Client
                {
                    ClientId = "Client2",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials, 

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.Person"
                    }
                },
                new Client
                {
                    ClientId = "Client3",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials, 

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {
                        "EDennis.Samples.DefaultPoliciesApi.Person.Get",
                        "EDennis.Samples.DefaultPoliciesApi.Position.Post"
                    }
                },
                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
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
                }
            };
            }
        }
    }