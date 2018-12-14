using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Linq;
using System.Reflection;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class provides methods helpful for testing
    /// purposes.
    /// </summary>
    public static class DbContextExtensions {

        /// <summary>
        /// Resets all value generators in a context to their
        /// maximum key values.  This method can be called 
        /// in between tests in order to reset Id backed by 
        /// sequences or identities
        /// </summary>
        /// <param name="context">The DbContext subclass</param>
        public static void ResetValueGenerators(this DbContextBase context) {

            //obtain a reference to the cache of value generators
            var cache = context.GetService<IValueGeneratorCache>();

            //loop through all entities that have an integer
            //primary key with a value generator defined for OnAdd,
            foreach (var keyProperty in context.Model.GetEntityTypes()
                    .Where(x=>!x.IsQueryType)
                    .Select(e => e.FindPrimaryKey().Properties[0])
                    .Where(p => p.ClrType == typeof(int)                            
                            && p.ValueGenerated == ValueGenerated.OnAdd)) {

                //get the current entity's type
                var type = keyProperty.DeclaringEntityType.ClrType;

                //invoke the method to obtain the current maximum value for the integer Id
                MethodInfo method = typeof(DbContextBase).GetMethod("GetMaxKeyValue");
                MethodInfo generic = method.MakeGenericMethod(type);
                var baseValue = (int)generic.Invoke(context, null);

                // retrieve a reference to a ResettableValueGenerator for  the Id
                var generator = (ResettableValueGenerator)cache.GetOrAdd(
                    keyProperty,
                    keyProperty.DeclaringEntityType,
                    (p, e) => new ResettableValueGenerator());

                //reset the Id to the current maximum value
                generator.Reset(baseValue);
            }
        }
    }
}
