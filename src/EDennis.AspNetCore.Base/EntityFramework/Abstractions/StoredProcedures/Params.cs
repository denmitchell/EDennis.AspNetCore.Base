using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class Params { 
        
        public static Dictionary<string,object> Create(dynamic parms) {
            Type type = parms.GetType();
            PropertyInfo[] props = type.GetProperties();
            var dict = new Dictionary<string, object>();
            foreach (var prop in props)
                dict.Add(prop.Name, prop.GetValue(parms));
            return dict;
        }

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
