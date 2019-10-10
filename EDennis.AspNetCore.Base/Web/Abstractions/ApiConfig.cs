using IdentityModel;

namespace EDennis.AspNetCore.Base.Web {
    public class ApiConfig {
        public string ProjectName { get; set; }
        public string SolutionName { get; set; }
        public string ProjectDirectory { get; set; }
        public string BaseAddress { get; set; }
        public bool ExternallyLaunched { get; set; } = false;
        public string[] Scopes { get; set; }
        public bool Pingable { get; set; }
        public ScopePropertiesPropagationOptions ScopePropertiesOptions { get; set; } 
            = new ScopePropertiesPropagationOptions();
    }

    public class ScopePropertiesPropagationOptions {
        public bool PropagateUser = true;
        public string PropagateClaimTypesWithPattern = $"^({JwtClaimTypes.Role}|{JwtClaimTypes.ClientId}|{JwtClaimTypes.Subject}$";
        public string PropagateHeadersWithPattern = "^X-";
    }
}
