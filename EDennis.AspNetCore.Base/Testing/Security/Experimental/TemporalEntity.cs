using System;
using E = IdentityServer4.EntityFramework.Entities;

/// <summary>
/// These classes are designed to enable use of temporal tables
/// with IdentityServer.
/// *** Special Note: these classes are not yet tested.
/// </summary>
namespace EDennis.AspNetCore.Base.Security {
    public class ApiResource : E.ApiResource { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ApiResourceClaim : E.ApiResourceClaim { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ApiResourceProperty : E.ApiResourceProperty { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ApiScope : E.ApiScope { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ApiScopeClaim : E.ApiScopeClaim { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ApiSecret : E.ApiSecret { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class Client : E.Client { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientClaim : E.ClientClaim { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientCorsOrigin : E.ClientCorsOrigin { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientGrantType : E.ClientGrantType { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientIdPRestriction : E.ClientIdPRestriction { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientPostLogoutRedirectUri : E.ClientPostLogoutRedirectUri { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientProperty : E.ClientProperty { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientRedirectUri : E.ClientRedirectUri { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientScope : E.ClientScope { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class ClientSecret : E.ClientSecret { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class DeviceFlowCodes : E.DeviceFlowCodes { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class IdentityClaim : E.IdentityClaim { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class IdentityResource : E.IdentityResource { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class IdentityResourceProperty : E.IdentityResourceProperty { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class PersistedGrant : E.PersistedGrant { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class Property : E.Property { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class Secret : E.Secret { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
    public class UserClaim : E.UserClaim { public int SysUserId { get; set; } public DateTime SysStart { get; set; } public DateTime SysEnd { get; set; } }
}
