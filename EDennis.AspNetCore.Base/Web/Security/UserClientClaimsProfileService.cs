using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web.Security {
    public abstract class UserClientClaimsProfileService : IProfileService {

        public abstract IEnumerable<Claim> GetUserClientClaims(string clientId, string subjectName);


        public Task GetProfileDataAsync(ProfileDataRequestContext context) {
            var clientId = context.Client.ClientId;
            var subjectName = context.Subject.Identity.Name;
            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(subjectName)) {
                var claimsToAdd = GetUserClientClaims(clientId, subjectName);
                context.IssuedClaims.AddRange(claimsToAdd);
            }
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context) {
            context.IsActive = true;
            return Task.FromResult(true);
        }
    }
}
