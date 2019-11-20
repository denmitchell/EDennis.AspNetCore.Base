using IdentityModel;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base.Testing.XunitBase.FixturesAndFactories {
    public static class TestScopePropertiesFactory {
        
        public const string DEFAULT_USER = "tester@example.org";
        public const string DEFAULT_ROLE = "user";


        public static IScopeProperties GetScopeProperties()
            => GetScopeProperties(DEFAULT_USER, DEFAULT_ROLE);



        public static IScopeProperties GetScopeProperties(string userName, string role) {

        var claims = new Claim[] {
            new Claim(JwtClaimTypes.Name,userName),
            new Claim(JwtClaimTypes.Role,role)
        };

        var headerDictionary = new HeaderDictionary {
            { Constants.TESTING_INSTANCE_KEY, userName },
            { Constants.USER_KEY, userName },
            { Constants.ROLE_KEY, role }
        };

            var scopeProperties = new ScopeProperties {
                Claims = claims,
                Headers = headerDictionary,
                User = userName
            };

            return scopeProperties;

        }


    }
}
