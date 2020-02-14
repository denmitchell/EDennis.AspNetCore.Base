using EDennis.AspNetCore.Base;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer {


    /// <summary>
    /// NOTE: This Config class has been modified to build
    /// ApiResources, Clients, and UserClientClaims 
    /// (app-specific role/claims) from Configuration data
    /// provided by the Configuration API.
    /// </summary>
    public static class Config {

        public static HttpClient HttpClient { get; }

        /// <summary>
        /// Statically build a new HttpClient that points to
        /// the Configuration Api
        /// </summary>
        static Config() {
            HttpClient = new HttpClient();
            var url = Environment.GetEnvironmentVariable("ConfigurationApiUrl");
            HttpClient.BaseAddress = new Uri(url);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"identityserver4_log.txt")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

        }

        static List<string> _projects;
        static readonly Dictionary<string, ActiveMockClientSettings> _activeMockClientSettings = new Dictionary<string, ActiveMockClientSettings>();
        static readonly Dictionary<string, Apis> _apis = new Dictionary<string, Apis>();
        static readonly Dictionary<string, RoleClaims> _roleClaims = new Dictionary<string, RoleClaims>();
        static readonly Dictionary<string, TestUserRoles> _testUserRoles = new Dictionary<string, TestUserRoles>();

        internal class ProjectConfigData {
            public string ProjectName { get; set; }
            public string SettingKey { get; set; }
            public string SettingValue { get; set; }
        }

        /// <summary>
        /// Retrieve configuration data across all projects
        /// that have configuration data in the Configuration API.
        /// </summary>
        public static void GetDataFromConfigurationApi() {

            //retrieve the raw data from the endpoint
            var result = HttpClient.GetAsync("Configuration/identity-server-configs").Result;
            var content = result.Content.ReadAsStringAsync().Result;

            //deserialize to a list
            var projectConfigData = JsonSerializer.Deserialize<List<ProjectConfigData>>(
                content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            //get a distinct list of projects
            _projects = projectConfigData.Select(p => p.ProjectName).Distinct().ToList();

            //iterate over the projects, populating each dictionary
            //with config data for the project
            foreach (var project in _projects) {
                Log.Logger.Debug("Getting Identity-Related Configs for {Project}", project);
                var configData = projectConfigData
                    .Where(p => p.ProjectName == project)
                    .Select(p => KeyValuePair.Create(p.SettingKey, p.SettingValue));

                var config = new ConfigurationBuilder()
                        .AddInMemoryCollection(configData)
                        .Build();

                //build the apis dictionary
                var apis = new Apis();
                config.GetSection("Apis").Bind(apis);
                _apis.Add(project, apis);
                Log.Logger.Debug("Bound Api Configs for {Project}: {Apis}", project, string.Join(',', apis.Select(a=>a.Value.ProjectName)));

                //build the mock clients dictionary
                var activeMockClientSettings = new ActiveMockClientSettings();
                config.GetSection("MockClient").Bind(activeMockClientSettings);
                if (activeMockClientSettings.MockClients?.Count > 0) {
                    _activeMockClientSettings.Add(project, activeMockClientSettings);
                    Log.Logger.Debug("Bound MockClient Configs for {Project}: {MockClients}", project, string.Join(',', activeMockClientSettings.MockClients.Select(a => a.Value.ClientId)));
                }

                //build the role claims dictionary
                var roleClaims = new RoleClaims();
                config.GetSection("RoleClaims").Bind(roleClaims);
                if (roleClaims.Count > 0) {
                    _roleClaims.Add(project, roleClaims);
                    Log.Logger.Debug("Bound RoleClaim Configs for {Project}: {RoleClaims}", project, string.Join(',', roleClaims.Keys));
                }

                //build the test user roles dictionary
                var testUserRoles = new TestUserRoles();
                config.GetSection("TestUserRoles").Bind(testUserRoles);
                if (testUserRoles.Count > 0) {
                    _testUserRoles.Add(project, testUserRoles);
                    Log.Logger.Debug("Bound TestUserRole Configs for {Project}: {TestUserRole}", project, string.Join(',', testUserRoles.Keys));
                }
            }

        }

        /// <summary>
        /// Use the Allen-Baier-provided method to configure Identity resources
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Use the Allen-Baier-provided method to build a list of test users
        /// </summary>
        /// <returns></returns>
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
                new TestUser
                {
                    SubjectId = "10",
                    Username = "keenan",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Keenan"),
                        new Claim ("email","keenan@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "11",
                    Username = "damon",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Damon"),
                        new Claim ("email","damon@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "12",
                    Username = "kim",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Kim"),
                        new Claim ("email","kim@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "13",
                    Username = "shawn",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Shawn"),
                        new Claim ("email","shawn@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "14",
                    Username = "marlon",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Marlon"),
                        new Claim ("email","marlon@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "15",
                    Username = "dwayne",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Dwayne"),
                        new Claim ("email","dwayne@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "16",
                    Username = "nadia",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Nadia"),
                        new Claim ("email","nadia@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "17",
                    Username = "elvira",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Elvira"),
                        new Claim ("email","elvira@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "18",
                    Username = "dierdre",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Dierdre"),
                        new Claim ("email","dierdre@example.com"),
                    }
                },
                new TestUser
                {
                    SubjectId = "19",
                    Username = "vonnie",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim ("name","Vonnie"),
                        new Claim ("email","vonnie@example.com"),
                    }
                },



            };
        }

        /// <summary>
        /// Build a list of Apis from the Apis entry in Configuration.  
        /// Add scopes from relevant MockClient entries.
        /// NOTE: the configuration data represent only
        /// projects that use the Configuration API.  
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis() {
            var apiResources = new List<ApiResource>();

            //iterate over all projects
            foreach (var key in _apis.Keys) {
                var apis = _apis[key];

                //iterate over all apis in the project's configuration
                foreach (var api in apis.Values) {
                    //retrieve the apiResource if it already exists
                    ApiResource apiResource = apiResources.FirstOrDefault(a => a.Name == api.ProjectName);

                    //otherwise, create it
                    if (apiResource == null)
                        apiResource = new ApiResource {
                            Name = api.ProjectName,
                            DisplayName = api.ProjectName,
                            Scopes = new List<Scope>()
                        };

                    //add main scope, if not already present
                    if (!apiResource.Scopes.Any(a => a.Name == $"{api.ProjectName}.*"))
                        apiResource.Scopes.Add(new Scope($"{api.ProjectName}.*"));

                    //add scopes from all relevant mock client scopes
                    foreach (var mockClients in _activeMockClientSettings.Where(a => a.Key == api.ProjectName)) {
                        foreach (var mockClient in mockClients.Value.MockClients)
                            foreach (var scope in mockClient.Value.Scopes)
                                if (!apiResource.Scopes.Any(a => a.Name == scope))
                                    apiResource.Scopes.Add(new Scope(scope));
                    }

                    apiResources.Add(apiResource);
                }
            }

            return apiResources;
        }

        /// <summary>
        /// Build a list of clients from configuration --
        /// either MockClient entries (OAuth) or the Identity
        /// Server entry in Apis (OIDC) 
        /// NOTE: the configuration data represent only
        /// projects that use the Configuration API.  
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients() {
            var clients = new List<Client>();

            //iterate over all projects
            foreach (var project in _projects) {

                //get a reference to the project's own API entry
                var selfApi = _apis[project].FirstOrDefault(a => a.Value.ProjectName == project).Value;

                //check the identity server API entry for the existence of an Oidc section
                var isOidc = _apis[project].Any(a => a.Value.Oidc != null);

                //add OIDC settings, if relevant
                if (isOidc) {
                    Client client = new Client {
                        ClientId = project,
                        ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                        ClientClaimsPrefix = "",
                        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                        RedirectUris = new string[] { $"http://localhost:{selfApi.HttpPort}/signin-oidc" },
                        PostLogoutRedirectUris = new string[] { $"http://localhost:{selfApi.HttpPort}/signout-callback-oidc" },
                        AllowOfflineAccess = true,
                        AlwaysIncludeUserClaimsInIdToken = true,
                        AllowAccessTokensViaBrowser = true,
                        AllowedScopes = new string[] {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            IdentityServerConstants.StandardScopes.Email,
                            "user_scope", "role", "name"
                        }
                    };

                    clients.Add(client);

                } else {

                    //iterate over all relevant mockclients
                    foreach (var mockClients in _activeMockClientSettings.Where(a => a.Key == project)) {
                        foreach (var mockClient in mockClients.Value.MockClients) {

                            //build a new client
                            Client client = new Client {
                                ClientId = mockClient.Value.ClientId,
                                ClientSecrets = new List<Secret> { new Secret(mockClient.Value.ClientSecret.Sha256()) },
                                ClientClaimsPrefix = "",
                                AllowedScopes = new List<string>(),
                                AllowedGrantTypes = GrantTypes.ClientCredentials
                            };

                            //add a project-level scope
                            client.AllowedScopes.Add($"{project}.*");

                            //get scoped from mock clients
                            foreach (var scope in mockClient.Value.Scopes)
                                if (!client.AllowedScopes.Any(a => a == scope))
                                    client.AllowedScopes.Add(scope);

                            clients.Add(client);
                        }

                    }
                }
            }

            return clients;

        }

        /// <summary>
        /// Retrieve a list of AppUserClaim entries for the
        /// StaticClaimsProfileService.  Use the entries in the
        /// Configuration API for RoleClaims and TestUserRoles
        /// to construct the UserClientClaims collection.
        /// </summary>
        public static IEnumerable<AppUserClaim> UserClientClaims {
            get {
                var auClaims = new List<AppUserClaim>();

                //iterate over all projects
                foreach (var project in _projects) {

                    //iterate over all test users defined for the project
                    foreach (var user in _testUserRoles[project].Keys) {

                        //build the base object
                        var auClaim = new AppUserClaim {
                            AppClientId = project,
                            Username = user,
                            Claims = new List<Claim>()
                        };

                        //iterate over all roles defined for the test user 
                        foreach (var role in _testUserRoles[project][user]) {
                            
                            //add a role claim
                            auClaim.Claims.Add(new Claim("role", role));

                            //iterate over all role claims to add each
                            //claim associated with the current role
                            foreach (var kvp in _roleClaims[project].Where(r => r.Key == role)) {
                                var cRoles = kvp.Value;
                                foreach (var cRole in cRoles)
                                    auClaim.Claims.Add(new Claim(cRole.Key, cRole.Value));
                            }

                        }


                    }
                }
                return auClaims;
            }
        }

        /// <summary>
        /// Encapsulates claims associated with
        /// an application and user (often roles or
        /// role-related scopes)
        /// </summary>
        public class AppUserClaim {
            public string AppClientId { get; set; }
            public string Username { get; set; }
            public List<Claim> Claims { get; set; }
        }

    }
}