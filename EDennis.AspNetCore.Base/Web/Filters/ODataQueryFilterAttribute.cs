using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web
{
    public class ODataQueryFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context) {

            if (!context.HttpContext.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var req = context.HttpContext.Request;
                var query = req.Query;
                Debug.WriteLine($"\n\n****PRE FILTER:\n{req.QueryString.Value}\n\n");

                var odataKeys = new string[] { "filter","sort","orderBy","select","skip","top","expand" };

                var kvs = query
                    .Where(x => odataKeys.Contains(x.Key) && !query.ContainsKey($"${x.Key}"))
                    .Select(x => KeyValuePair.Create($"${x.Key}", x.Value))
                    .Union(query.Where(x=> !odataKeys.Contains(x.Key)))
                    .ToDictionary(x => x.Key, x => x.Value);
                req.Query = new QueryCollection(kvs);

                Debug.WriteLine($"\n\n****POST FILTER:\n{req.QueryString.Value}\n\n");
            }
        }

    }

}

