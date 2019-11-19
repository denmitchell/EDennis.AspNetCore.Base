using System.Threading.Tasks;
using EDennis.AspNetCore.Base.Web;
using IdentityModel.Client;

namespace EDennis.AspNetCore.Base.Security {
    public interface ISecureTokenService {
        string ApplicationName { get; set; }

        void Dispose();
        Task<TokenResponse> GetTokenAsync<TClient>(TClient client) where TClient : SecureApiClient;
        Task<TokenResponse> GetTokenResponse(string apiKey);
        Task<TokenResponse> GetTokenResponse(string clientId, string clientSecret, string[] scopes);
    }
}