using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace EDennis.AspNetCore.Base.Common {
    public class EmbeddedResourceUtils {
        /// <summary>
        /// Reads in a stored procedure definition, which has been stored as
        /// embedded resources so that it can be distributed with the NuGet assembly
        /// </summary>
        /// <param name="fileName">The name of the file to read in</param>
        /// <returns>The string contents of the file.</returns>
        public static string GetEmbeddedResource(Assembly assembly, string resourceName) {
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            string result = reader.ReadToEnd();
            return result;
        }

    }
}
