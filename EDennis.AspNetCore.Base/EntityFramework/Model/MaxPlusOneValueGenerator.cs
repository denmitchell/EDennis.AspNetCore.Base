using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class provides a value generator that bases the next
    /// value upon the existing maximum value for an ID.  This is
    /// suitable for testing scenarios only (e.g., in-memory database)
    /// 
    /// This class was inspired by the ResettableValueGenerator class
    /// by Author Vickers (https://github.com/aspnet/EntityFrameworkCore/issues/6872#issuecomment-258025241)
    /// </summary>
    public class MaxPlusOneValueGenerator : ValueGenerator<int> {

        private static MethodInfo method;

        //statically set the MethodInfo for GetMaxKeyValue
        static MaxPlusOneValueGenerator() {
            method = typeof(DbContextBase).GetMethod("GetMaxKeyValue");
        }

        //determines if values are overrided by the database 
        //(since this class is use for testing, values are not 
        //written to the database)
        public override bool GeneratesTemporaryValues => false;

        /// <summary>
        /// Gets the next value for the ID of the entity. The
        /// next value is always 1 + current maximum
        /// </summary>
        /// <param name="entry">The entry that will be written to the test database</param>
        /// <returns>max plus 1</returns>
        public override int Next(EntityEntry entry) {
            var context = entry.Context as DbContextBase;
            var type = entry.Entity.GetType();

            //use reflection to get the current maximum value
            MethodInfo generic = method.MakeGenericMethod(type);
            var max = (int)generic.Invoke(context, null);

            //return max plus one
            return max + 1;
        }

    }
}