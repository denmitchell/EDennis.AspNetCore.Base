using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
    public class ResettableRepo<TEntity, TContext> : WriteableRepo<TEntity, TContext>, IDisposable
            where TEntity : class, new()
            where TContext : DbContextBase {

        private bool _testing = false;

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
            _testing = true;
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    //rollback any outstanding transactions
                    if (_testing)
                        if (Context.Database.CurrentTransaction != null) {
                            var cxn = Context.Database.GetDbConnection() as SqlConnection;
                            if (cxn.State == System.Data.ConnectionState.Open) {
                                Context.Database.RollbackTransaction();
                                if (Context.HasIdentities)
                                    cxn.ResetIdentities();
                                if (Context.HasSequences)
                                    cxn.ResetSequences();
                            }
                        }
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion

    }
}
