using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Web
{
    public class WriteableControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) {
            var currentAssembly = typeof(WriteableControllerFeatureProvider).Assembly;
            var candidates = currentAssembly.GetExportedTypes().Where(x => x.GetCustomAttributes<WriteableControllerAttribute>().Any());

            foreach (var candidate in candidates) {
                feature.Controllers.Add(typeof(WriteableController<,>).MakeGenericType(candidate).GetTypeInfo());
            }
        }
    }
}
