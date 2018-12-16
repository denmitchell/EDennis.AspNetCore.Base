using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.EntityFramework {

    /// <summary>
    /// This class extends WriteableRepo by including a method
    /// that allows one to replace the dbcontext with another
    /// dbcontext.  This ability is important for certain 
    /// integration testing situations in which 
    /// (a) the controller's construction is still managed by
    ///     the framework through dependency injection AND
    /// (b) the test dbcontext must be built sometime after setup,
    ///     where dependency injection was already configured.
    /// Using the ApiLauncher class occasions this kind of 
    /// situation: the APIs are launched just once, before any
    /// tests are run; and then the dbcontexts are replaced
    /// during each test case.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class ResettableRepo<TEntity, TContext> : WriteableRepo<TEntity, TContext>
            where TEntity : class, new()
            where TContext : DbContextBase {

        /// <summary>
        /// Constructs a new ResettableRepo, based upon the
        /// provided DbContextBase
        /// </summary>
        /// <param name="context">DbContextBase object</param>
        public ResettableRepo(TContext context) : base(context) { }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public void ReplaceDbContext(dynamic dbContext) {
            Context = dbContext;

            //if (Context.Database.IsInMemory())
            //    Context.ResetValueGenerators();
        }

    }
}
