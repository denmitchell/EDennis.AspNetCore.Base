using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

