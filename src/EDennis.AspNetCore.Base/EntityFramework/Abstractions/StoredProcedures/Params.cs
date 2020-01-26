using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class Params { 
        
        public static Dictionary<string,object> ToStoredProcedureParameters(this HttpContext httpContext, params string[] keys) {

            var dict = new Dictionary<string, object>();
            
            foreach(var key in keys) {
                foreach (var qKey in httpContext.Request.Query.Where(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)))
                    dict.Add(qKey.Key, qKey.Value.ToString());
            }

            return dict;

        }
    }
}
