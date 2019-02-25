using Microsoft.EntityFrameworkCore;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// A read/write base repository class, backed by
    /// a DbSet, exposing basic CRUD methods, as well
    /// as methods exposed by QueryableRepo 
    /// </summary>
    /// <typeparam name="TEntity">The associated model class</typeparam>
    /// <typeparam name="TContext">The associated DbContextBase class</typeparam>
    public abstract class ReadonlyTemporalRepo<TEntity, TContext, THistoryContext>
        : ReadonlyRepo<TEntity,TContext>, ITemporalRepo<TEntity,TContext,THistoryContext>
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext: DbContext {


        public THistoryContext HistoryContext { get; set; }


        /// <summary>
        /// Constructs a new RepoBase object using the provided DbContext
        /// </summary>
        /// <param name="context">Entity Framework DbContext</param>
        public ReadonlyTemporalRepo(TContext context, THistoryContext historyContext) :
            base(context) {
            HistoryContext = historyContext;
        }


    }

}

