using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EDennis.AspNetCore.Base.Web {
    public static class IMvcBuilderExtensions {

        public static IMvcBuilder ExcludeReferencedProjectControllers<TStartup>(this IMvcBuilder builder)
            where TStartup : class {

            var projectName = Assembly.GetAssembly(typeof(TStartup)).FullName;
            projectName = projectName.Substring(0, projectName.IndexOf(',')).TrimEnd();


            builder.ConfigureApplicationPartManager(apm => {
                 var dependentLibrary = apm.ApplicationParts
                     .FirstOrDefault(part => part.Name == projectName);

                 if (dependentLibrary != null) {
                     apm.ApplicationParts.Remove(dependentLibrary);
                 }
             });

            return builder;

        }

    }
}
