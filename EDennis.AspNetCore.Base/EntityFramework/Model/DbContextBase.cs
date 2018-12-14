using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This subclass of DbContext includes properties and
    /// methods that are helpful for unit and integration
    /// testing.
    /// </summary>
    public abstract class DbContextBase : DbContext {

        public DbContextBase(DbContextOptions options)
        : base(options) { }

        //the configuration key associated with the connection string
        public string ConnectionStringName { get; set; }

        //the in-memory database name or name assigned to the connection/transaction
        public string NamedInstance { get; set; }

        /// <summary>
        /// Provides the maximum value of the key for a model
        /// class having an integer Id property
        /// </summary>
        /// <typeparam name="TEntity">The model class</typeparam>
        /// <returns>the maximum value or zero</returns>
        public int GetMaxKeyValue<TEntity>()
                where TEntity : class, IHasIntegerId {
            var qry = Set<TEntity>();
            if (qry.Count() == 0)
                return 0;
            else
                return qry.Select(x => x.Id).Max();
        }

    }

}
