using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class provides a value generator which can be
    /// reset to a specific value (e.g., the maximum value
    /// for a Id).
    /// 
    /// This class is based upon the like-named value generator
    /// by Author Vickers (https://github.com/aspnet/EntityFrameworkCore/issues/6872#issuecomment-258025241)
    /// </summary>
    public class MaxPlusOneValueGenerator : ValueGenerator<int> {

        private static MethodInfo method;

        static MaxPlusOneValueGenerator() {
            method = typeof(DbContextBase).GetMethod("GetMaxKeyValue");
        }

        //values are stored
        public override bool GeneratesTemporaryValues => false;

        //gets the next value
        public override int Next(EntityEntry entry) {
            var context = entry.Context as DbContextBase;
            var type = entry.Entity.GetType();
            MethodInfo generic = method.MakeGenericMethod(type);
            var max = (int)generic.Invoke(context, null);
            return max + 1;
        }

    }
}