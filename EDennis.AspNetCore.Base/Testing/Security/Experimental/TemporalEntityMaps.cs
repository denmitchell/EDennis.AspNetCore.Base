using E = IdentityServer4.EntityFramework.Entities;
using AutoMapper;

/// <summary>
/// This class is designed to enable use of temporal tables
/// with IdentityServer.
/// *** Special Note: this class is not yet tested.
/// </summary>
namespace EDennis.AspNetCore.Base.Security {
    public static class TemporalEntityMaps {
        #region From IS4 Entity to Temporal Entity
        public static ApiResource ToTemporal(this E.ApiResource obj) => Mapper.Map<ApiResource>(obj);
        public static ApiResourceClaim ToTemporal(this E.ApiResourceClaim obj) => Mapper.Map<ApiResourceClaim>(obj);
        public static ApiResourceProperty ToTemporal(this E.ApiResourceProperty obj) => Mapper.Map<ApiResourceProperty>(obj);
        public static ApiScope ToTemporal(this E.ApiScope obj) => Mapper.Map<ApiScope>(obj);
        public static ApiScopeClaim ToTemporal(this E.ApiScopeClaim obj) => Mapper.Map<ApiScopeClaim>(obj);
        public static ApiSecret ToTemporal(this E.ApiSecret obj) => Mapper.Map<ApiSecret>(obj);
        public static Client ToTemporal(this E.Client obj) => Mapper.Map<Client>(obj);
        public static ClientClaim ToTemporal(this E.ClientClaim obj) => Mapper.Map<ClientClaim>(obj);
        public static ClientCorsOrigin ToTemporal(this E.ClientCorsOrigin obj) => Mapper.Map<ClientCorsOrigin>(obj);
        public static ClientGrantType ToTemporal(this E.ClientGrantType obj) => Mapper.Map<ClientGrantType>(obj);
        public static ClientIdPRestriction ToTemporal(this E.ClientIdPRestriction obj) => Mapper.Map<ClientIdPRestriction>(obj);
        public static ClientPostLogoutRedirectUri ToTemporal(this E.ClientPostLogoutRedirectUri obj) => Mapper.Map<ClientPostLogoutRedirectUri>(obj);
        public static ClientProperty ToTemporal(this E.ClientProperty obj) => Mapper.Map<ClientProperty>(obj);
        public static ClientRedirectUri ToTemporal(this E.ClientRedirectUri obj) => Mapper.Map<ClientRedirectUri>(obj);
        public static ClientScope ToTemporal(this E.ClientScope obj) => Mapper.Map<ClientScope>(obj);
        public static ClientSecret ToTemporal(this E.ClientSecret obj) => Mapper.Map<ClientSecret>(obj);
        public static DeviceFlowCodes ToTemporal(this E.DeviceFlowCodes obj) => Mapper.Map<DeviceFlowCodes>(obj);
        public static IdentityClaim ToTemporal(this E.IdentityClaim obj) => Mapper.Map<IdentityClaim>(obj);
        public static IdentityResource ToTemporal(this E.IdentityResource obj) => Mapper.Map<IdentityResource>(obj);
        public static IdentityResourceProperty ToTemporal(this E.IdentityResourceProperty obj) => Mapper.Map<IdentityResourceProperty>(obj);
        public static PersistedGrant ToTemporal(this E.PersistedGrant obj) => Mapper.Map<PersistedGrant>(obj);
        public static Property ToTemporal(this E.Property obj) => Mapper.Map<Property>(obj);
        public static Secret ToTemporal(this E.Secret obj) => Mapper.Map<Secret>(obj);
        public static UserClaim ToTemporal(this E.UserClaim obj) => Mapper.Map<UserClaim>(obj);
        #endregion
        #region From Temporal Entity to IS4 Entity
        public static E.ApiResource ToEntity(this ApiResource obj) => Mapper.Map<E.ApiResource>(obj);
        public static E.ApiResourceClaim ToEntity(this ApiResourceClaim obj) => Mapper.Map<E.ApiResourceClaim>(obj);
        public static E.ApiResourceProperty ToEntity(this ApiResourceProperty obj) => Mapper.Map<E.ApiResourceProperty>(obj);
        public static E.ApiScope ToEntity(this ApiScope obj) => Mapper.Map<E.ApiScope>(obj);
        public static E.ApiScopeClaim ToEntity(this ApiScopeClaim obj) => Mapper.Map<E.ApiScopeClaim>(obj);
        public static E.ApiSecret ToEntity(this ApiSecret obj) => Mapper.Map<E.ApiSecret>(obj);
        public static E.Client ToEntity(this Client obj) => Mapper.Map<E.Client>(obj);
        public static E.ClientClaim ToEntity(this ClientClaim obj) => Mapper.Map<E.ClientClaim>(obj);
        public static E.ClientCorsOrigin ToEntity(this ClientCorsOrigin obj) => Mapper.Map<E.ClientCorsOrigin>(obj);
        public static E.ClientGrantType ToEntity(this ClientGrantType obj) => Mapper.Map<E.ClientGrantType>(obj);
        public static E.ClientIdPRestriction ToEntity(this ClientIdPRestriction obj) => Mapper.Map<E.ClientIdPRestriction>(obj);
        public static E.ClientPostLogoutRedirectUri ToEntity(this ClientPostLogoutRedirectUri obj) => Mapper.Map<E.ClientPostLogoutRedirectUri>(obj);
        public static E.ClientProperty ToEntity(this ClientProperty obj) => Mapper.Map<E.ClientProperty>(obj);
        public static E.ClientRedirectUri ToEntity(this ClientRedirectUri obj) => Mapper.Map<E.ClientRedirectUri>(obj);
        public static E.ClientScope ToEntity(this ClientScope obj) => Mapper.Map<E.ClientScope>(obj);
        public static E.ClientSecret ToEntity(this ClientSecret obj) => Mapper.Map<E.ClientSecret>(obj);
        public static E.DeviceFlowCodes ToEntity(this DeviceFlowCodes obj) => Mapper.Map<E.DeviceFlowCodes>(obj);
        public static E.IdentityClaim ToEntity(this IdentityClaim obj) => Mapper.Map<E.IdentityClaim>(obj);
        public static E.IdentityResource ToEntity(this IdentityResource obj) => Mapper.Map<E.IdentityResource>(obj);
        public static E.IdentityResourceProperty ToEntity(this IdentityResourceProperty obj) => Mapper.Map<E.IdentityResourceProperty>(obj);
        public static E.PersistedGrant ToEntity(this PersistedGrant obj) => Mapper.Map<E.PersistedGrant>(obj);
        public static E.Property ToEntity(this Property obj) => Mapper.Map<E.Property>(obj);
        public static E.Secret ToEntity(this Secret obj) => Mapper.Map<E.Secret>(obj);
        public static E.UserClaim ToEntity(this UserClaim obj) => Mapper.Map<E.UserClaim>(obj);
        #endregion
    }
}
