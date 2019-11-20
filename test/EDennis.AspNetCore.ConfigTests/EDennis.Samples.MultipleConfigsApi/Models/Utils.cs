using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.Samples.MultipleConfigsApi.Models {
    public static class Utils {

        /// <summary>
        /// from https://stackoverflow.com/a/17376472
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CSharpName(this Type type) {
            var sb = new StringBuilder();
            var name = type.Name;
            if (!type.IsGenericType) return name;
            sb.Append(name.Substring(0, name.IndexOf('`')));
            sb.Append("<");
            sb.Append(string.Join(", ", type.GetGenericArguments()
                                            .Select(t => t.CSharpName())));
            sb.Append(">");
            return sb.ToString();
        }
    }
}
