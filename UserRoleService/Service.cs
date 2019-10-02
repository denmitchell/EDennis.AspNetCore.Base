using System;
using System.Collections.Generic;
using System.Linq;

namespace UserRoleService {
    public static class Service {

        private static UserRole[] userRoles = new UserRole[] {

            new UserRole {
                    SubjectId = "1",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.Admin"
             },
            new UserRole {
                    SubjectId = "2",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.Admin"
             },
            new UserRole {
                    SubjectId = "3",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.Admin"
             },
            new UserRole {
                    SubjectId = "4",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.NoDelete"
             },
            new UserRole {
                    SubjectId = "5",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.NoDelete"
             },
            new UserRole {
                    SubjectId = "6",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.NoDelete"
             },
            new UserRole {
                    SubjectId = "7",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.Readonly"
             },
            new UserRole {
                    SubjectId = "8",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.Readonly"
             },
            new UserRole {
                    SubjectId = "9",
                    AppName = "EDennis.Samples.DefaultPoliciesMvc",
                    RoleName = "EDennis.Samples.DefaultPoliciesMvc.Readonly"
             }
        };

        public static string[] GetRoles(string subjectId, string appName) {
            return userRoles
                .Where(r => r.SubjectId == subjectId && r.AppName == appName)
                .Select(r=>r.RoleName)
                .ToArray();
        }
    }
}
