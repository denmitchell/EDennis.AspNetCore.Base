using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// Provides extension methods for the Type class.
    /// </summary>
    public static class TypeExtension {

        /// <summary>
        /// Provides an extension method that retrieves all
        /// extension methods for a given type.
        /// </summary>
        /// <param name="extendedType">the type to check for extension methods</param>
        /// <param name="assembly">the assembly to check</param>
        /// <returns>a collection of extension methods</returns>
        public static IEnumerable<MethodInfo> GetExtensionMethods(this Type extendedType, Assembly assembly) {
            List<MethodInfo> extensionMethods = new List<MethodInfo>();

            foreach (var type in assembly.GetTypes()) {
                if (type.IsDefined(typeof(ExtensionAttribute), false)) {
                    foreach (var method in type.GetMethods()) {
                        if (method.IsDefined(typeof(ExtensionAttribute), false)) {
                            if (method.GetParameters()[0].ParameterType == extendedType)
                                extensionMethods.Add(method);
                        }
                    }
                }
            }
            return extensionMethods;
        }
    }
}
