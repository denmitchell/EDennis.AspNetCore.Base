using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer {
    public class UserRoleProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context) {
            var subjectName = context.Subject?.Identity?.Name;
            var appName = context.Client?.ClientId;
        }

        public Task IsActiveAsync(IsActiveContext context) {
            return Task.CompletedTask;
        }
    }
}
