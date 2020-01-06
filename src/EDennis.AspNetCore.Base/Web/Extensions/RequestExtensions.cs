using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web {
    public static partial class RequestExtensions {
        public static bool ContainsPathHeaderOrQueryKey(this HttpRequest request, string key, out string value, string defaultValue = null) {
            if (request.Path.Value.Contains(key, StringComparison.OrdinalIgnoreCase)) {
                value = defaultValue;
                return true;
            } else if (request.Headers.Keys.Any(k => k.Equals(key, StringComparison.OrdinalIgnoreCase))) {
                value = request.Headers[key];
                return true;
            } else if (request.Query.Keys.Any(k => k.Equals(key, StringComparison.OrdinalIgnoreCase))) {
                value = request.Query[key];
                return true;
            } else {
                value = null;
                return false;
            }
        }
    }

}
