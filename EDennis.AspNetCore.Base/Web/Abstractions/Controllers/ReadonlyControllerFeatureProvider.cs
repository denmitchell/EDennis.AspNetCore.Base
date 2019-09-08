using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Web
{
    public class ReadonlyControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) {
            var currentAssembly = typeof(ReadonlyControllerFeatureProvider).Assembly;
            var candidates = currentAssembly.GetExportedTypes().Where(x => x.GetCustomAttributes<ReadonlyControllerAttribute>().Any());

            foreach (var candidate in candidates) {
                feature.Controllers.Add(typeof(ReadonlyController<,>).MakeGenericType(candidate).GetTypeInfo());
            }
        }
    }
}
