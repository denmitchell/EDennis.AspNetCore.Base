using EDennis.AspNetCore.Base.Security;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using M = IdentityServer4.Models;
using S = System.Security.Claims;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This class is designed to load mock configurations
    /// from appsettings.Development.json and based upon
    /// generated DefaultPolicies for controllers and
    /// actions.
    /// *** Special Note: this class is not yet tested.
    /// </summary>
    public class IdentityServerMockConfigLoader {


        public static async Task LoadMockConfig(IConfiguration config, DateTime lastSaveDate) {
            var appName = GetAppName(config);
            var identityServerConnectionString = config["IdentityServer:ConnectionString"];
            var apiResource = GetApiResource(config,appName);
            var identityResources = GetIdentityResources(config);
            var clients = GetClients(config, appName);
            await LoadMockData(identityServerConnectionString, 
                apiResource, identityResources, clients, lastSaveDate);
        }

        private static async Task LoadMockData(string identityServerConnectionString,
            M.ApiResource apiResource,
            IEnumerable<M.IdentityResource> identityResources,
            IEnumerable<M.Client> clients,
            DateTime lastSaveDate) {

            var builderOptions =
                new DbContextOptionsBuilder()
                .UseSqlServer(identityServerConnectionString)
                .Options;

            var storeOptions = new ConfigurationStoreOptions();

            using (var context = new IdentityServerContext(builderOptions)) {
                await Task.WhenAll(
                    LoadApiResource(apiResource, context, lastSaveDate),
                    LoadIdentityResources(identityResources, context, lastSaveDate),
                    LoadClients(clients, context, lastSaveDate));
            }

        }


        public static async Task LoadApiResource(M.ApiResource apiResource,
                IdentityServerContext context, DateTime lastSavedDate) {

            var entityToAddOrUpdate = apiResource.ToEntity().ToTemporal();
            var existingEntity = context.ApiResources
                .Where(e => e.Name == entityToAddOrUpdate.Name).FirstOrDefault();

            if (existingEntity == null) {
                context.Entry(entityToAddOrUpdate).State = EntityState.Added;
            } else if (
                lastSavedDate > existingEntity.Created &&
                    lastSavedDate > (existingEntity.Updated ?? DateTime.MinValue)) {
                entityToAddOrUpdate.Updated = lastSavedDate;
                context.Attach(entityToAddOrUpdate);
                context.Entry(entityToAddOrUpdate).State = EntityState.Modified;
            }
            await context.SaveChangesAsync();
        }


        public static async Task LoadIdentityResources(IEnumerable<M.IdentityResource> identityResources,
                IdentityServerContext context, DateTime lastSavedDate) {
            foreach (var identityResource in identityResources) {

                var entityToAddOrUpdate = identityResource.ToEntity().ToTemporal();
                var existingEntity = context.IdentityResources
                    .Where(e => e.Name == entityToAddOrUpdate.Name).FirstOrDefault();

                if (existingEntity == null) {
                    context.Entry(entityToAddOrUpdate).State = EntityState.Added;
                } else if (StandardIdentityResourceScopes.ContainsKey(entityToAddOrUpdate.Name)) { 
                    //do nothing if a standard identity resource scope has been added already
                } else if (
                    lastSavedDate > existingEntity.Created &&
                        lastSavedDate > (existingEntity.Updated ?? DateTime.MinValue)) {
                    entityToAddOrUpdate.Updated = lastSavedDate;
                    context.Attach(entityToAddOrUpdate);
                    context.Entry(entityToAddOrUpdate).State = EntityState.Modified;
                }
            }
            await context.SaveChangesAsync();
        }

        public static async Task LoadClients(IEnumerable<M.Client> clients,
                IdentityServerContext context, DateTime lastSavedDate) {
            foreach (var client in clients) {

                var entityToAddOrUpdate = client.ToEntity().ToTemporal();
                var existingEntity = context.Clients
                    .Where(e => e.ClientName == entityToAddOrUpdate.ClientName).FirstOrDefault();

                if (existingEntity == null) {
                    context.Entry(entityToAddOrUpdate).State = EntityState.Added;
                } else if (
                    lastSavedDate > existingEntity.Created &&
                        lastSavedDate > (existingEntity.Updated ?? DateTime.MinValue)) {
                    entityToAddOrUpdate.Updated = lastSavedDate;
                    context.Attach(entityToAddOrUpdate);
                    context.Entry(entityToAddOrUpdate).State = EntityState.Modified;
                }
            }
            await context.SaveChangesAsync();
        }

        public static M.ApiResource GetApiResource(IConfiguration config, string appName) {
            var apiResource = new M.ApiResource {
                Name = appName
            };
            apiResource.DisplayName = apiResource.Name;
            apiResource.Scopes = new List<M.Scope> {
                new M.Scope { Name = apiResource.Name, DisplayName = apiResource.DisplayName }
            };
            var policies = GetPolicies(config);
            foreach (var policy in policies) {
                apiResource.Scopes.Add(
                    new M.Scope { Name = policy, DisplayName = policy }
                    );
            }
            return apiResource;
        }

        public static IEnumerable<M.IdentityResource> GetIdentityResources(IConfiguration config) {

            var resources = new List<M.IdentityResource>();
            var sections = config.GetSection("IdentityResources").GetChildren().AsEnumerable();


            foreach (var section in sections) {
                M.IdentityResource resource = null;

                if (StandardIdentityResourceScopes.ContainsKey(section.Key))
                    resource = StandardIdentityResourceScopes[section.Key];
                else {
                    var mockIdentityResource = new MockIdentityResourceProperties();
                    section.Bind(mockIdentityResource);
                    resource = new M.IdentityResource {
                        Name = $"{section.Key}",
                        DisplayName = mockIdentityResource.DisplayName,
                        Required = mockIdentityResource.Required,
                        Emphasize = mockIdentityResource.Emphasize
                    };

                    var claims = mockIdentityResource.Claims;
                    if (claims == null && mockIdentityResource.Claim != null) {
                        claims = mockIdentityResource.Claim.Split(" ");
                    }
                    if (claims != null)
                        resource.UserClaims = claims;

                }
                resources.Add(resource);
            }

            return resources;
        }


        public static IEnumerable<M.Client> GetClients(IConfiguration config, string appName) {
            var clients = new List<M.Client>();

            var mockClientSection = config.GetSection("MockClient");
            MockClientDictionary mockClientDictionary = new MockClientDictionary();
            mockClientSection.Bind(mockClientDictionary);


            foreach (var entry in mockClientDictionary) {

                M.Client client = new M.Client {
                    ClientId = $"{appName}.{entry.Key}",
                    ClientName = $"{appName}.{entry.Key}",
                    Enabled = entry.Value.Enabled
                };

                var grantTypes = entry.Value.GrantTypes;
                if (grantTypes == null && entry.Value.GrantType != null) {
                    grantTypes = entry.Value.GrantType.Split(" ");
                    if (grantTypes.Count() == 1)
                        grantTypes = grantTypes[0].ToEnum<GrantTypes>().ToStringList().ToArray();
                }
                if (grantTypes != null)
                    client.AllowedGrantTypes = grantTypes;

                client.ClientSecrets = (new M.Secret[] {
                    new M.Secret {
                        Type = "SharedSecret",
                        Value = (entry.Value.Secret ?? entry.Key)
                    } }).ToList();

                var scopes = entry.Value.Scopes;
                if (scopes == null && entry.Value.Scope != null) {
                    scopes = entry.Value.Scope.Split(" ");
                }
                if (scopes != null)
                    client.AllowedScopes = scopes;

                var claims = entry.Value.Claims;
                if (claims != null) {
                    var normalized = claims.Select(e => 
                        new KeyValuePair<string, string[]>(e.Key, e.Value.Split(' ')))
                            .SelectMany(p => p.Value,
                                (parent, child) => new S.Claim(parent.Key,child)).ToArray();
                    client.Claims = normalized;
                }

                var postLogoutRedirectUris = entry.Value.PostLogoutRedirectUris;
                if (postLogoutRedirectUris == null && entry.Value.PostLogoutRedirectUri != null) {
                    postLogoutRedirectUris = entry.Value.PostLogoutRedirectUri.Split(" ");
                }
                if (postLogoutRedirectUris != null)
                    client.PostLogoutRedirectUris = postLogoutRedirectUris;

                var redirectUris = entry.Value.RedirectUris;
                if (redirectUris == null && entry.Value.RedirectUri != null) {
                    redirectUris = entry.Value.RedirectUri.Split(" ");
                }
                if (redirectUris != null)
                    client.RedirectUris = redirectUris;


                clients.Add(client);
            }

            return clients;
        }


        private static List<string> GetListOrSingleMember(IConfigurationSection config,
            string key, string defaultValue = null) {
            var list = config.GetSection("key")?.GetChildren()?.Select(s => s.Value)?.ToList();
            if (list == null) {
                if (config["key"] != null)
                    list = new List<string> { config["key"] };
                else if (defaultValue != null)
                    list = new List<string> { defaultValue };
            }
            return list;
        }


        private static IEnumerable<string> GetPolicies(IConfiguration config) {
            var policies = config.GetSection("DefaultPolicies").AsEnumerable().Select(e => e.Key);
            return policies;
        }

        private static string GetAppName(IConfiguration config) {
            var defaultPolicies = config.GetSection("DefaultPolicies").AsEnumerable()
                .Select(e => new { e.Key, e.Value })
                .ToDictionary(e => e.Key, e => e.Value);
            return defaultPolicies.Where(e => e.Value == "application").Select(e => e.Key).FirstOrDefault();
        }

        private static readonly Dictionary<string, M.IdentityResource> StandardIdentityResourceScopes
            = new Dictionary<string, M.IdentityResource> {
                { IdentityServerConstants.StandardScopes.Address, new M.IdentityResources.OpenId() },
                { IdentityServerConstants.StandardScopes.Email, new M.IdentityResources.Email() },
                { IdentityServerConstants.StandardScopes.OpenId, new M.IdentityResources.OpenId() },
                { IdentityServerConstants.StandardScopes.Phone, new M.IdentityResources.Phone() },
                { IdentityServerConstants.StandardScopes.Profile, new M.IdentityResources.Profile() },
            };

    }

    [Flags]
    public enum GrantTypes {
        Implicit = 0x1,
        Code = 0x2,
        Hybrid = 0x4,
        ClientCredentials = 0x8,
        ResourceOwnerPassword = 0x16,
        ImplicitAndClientCredentials = Implicit | ClientCredentials,
        CodeAndClientCredentials = Code | ClientCredentials,
        HybridAndClientCredentials = Hybrid | ClientCredentials
    }

    static class EnumExtensions {
        public static T ToEnum<T>(this string value)
            where T : struct, IConvertible {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<string> ToStringList<T>(this T enumVal)
            where T : struct, IConvertible {
            var list = new List<string>();
            var enumVals = ((T[])Enum.GetValues(typeof(T)));
            foreach (var val in enumVals.Where(v => IsBaseBit(v))) {
                if (IsSetBit(val, enumVal))
                    list.Add(enumVal.ToString());
            }
            return list;
        }

        private static bool IsBaseBit<T>(T enumVal)
            where T : struct, IConvertible {
            var bit = Convert.ToInt32(enumVal);
            return (bit & (bit - 1)) == 0;
        }

        private static bool IsSetBit<T>(T testVal, T refVal)
            where T : struct, IConvertible {
            var testValInt = Convert.ToInt32(testVal);
            var refValInt = Convert.ToInt32(testVal);
            return (testValInt & refValInt) == refValInt;
        }

    }
}
