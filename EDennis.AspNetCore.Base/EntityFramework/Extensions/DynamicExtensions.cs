using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace EDennis.AspNetCore.Base.EntityFramework
{
    //TODO: Replace with ObjectExtensions -- no dependency on Json.NET

    public static class DynamicExtensions {

        public static bool PropertyExists(dynamic obj, string property)
            => ((Type)obj.GetType()).GetProperties().Any(p => p.Name.Equals(property));

        public static List<string> GetProperties(dynamic obj) {

            if (obj is IDynamicMetaObjectProvider tTarget) {
                return tTarget.GetMetaObject(Expression.Constant(tTarget)).GetDynamicMemberNames().ToList();
            }
            return new List<string>();
        }

        public static void Populate<T>(T destination, dynamic source)
            where T: class, new() {

            var json = JToken.FromObject(source).ToString();
            JsonConvert.PopulateObject(json, destination);
        }

    }
}
