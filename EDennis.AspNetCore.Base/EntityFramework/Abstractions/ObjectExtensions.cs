using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EDennis.AspNetCore.Base.EntityFramework {
    public static class ObjectExtensions {
        public static T DeepClone<T>(this T source) {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source));
        }
    }
}
