using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Linq;
using System.Reflection;

namespace EDennis.AspNetCore.Base.Testing{

    /// <summary>
    /// This class provides a value generator that bases the next
    /// value upon the existing maximum value for an ID.  This is
    /// suitable for testing scenarios only (e.g., in-memory database)
    /// 
    /// This class was inspired by the ResettableValueGenerator class
    /// by Author Vickers (https://github.com/aspnet/EntityFrameworkCore/issues/6872#issuecomment-258025241)
    /// </summary>
    public class MaxPlusOneValueGenerator<TEntity> : ValueGenerator<int> 
        where TEntity : class, IHasIntegerId {

        private static readonly MethodInfo generic;

        //statically set the MethodInfo for GetMaxKeyValue
        static MaxPlusOneValueGenerator() {
            var method = typeof(DbContext).GetMethod("Set");
            generic = method.MakeGenericMethod(typeof(TEntity));
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
            var context = entry.Context;
            var qry = generic.Invoke(context, null) as DbSet<TEntity>;

            var max = 0;

            if (qry.AsNoTracking().Count() != 0)
                max = qry.Select(x => x.Id).Max();

            //return max plus one
            return max + 1;
        }

    }
}