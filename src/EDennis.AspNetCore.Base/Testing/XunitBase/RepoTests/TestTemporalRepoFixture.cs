//using EDennis.AspNetCore.Base.EntityFramework;
//using Microsoft.EntityFrameworkCore;
//using System;


//namespace EDennis.AspNetCore.Base.Testing {
//    public class TestTemporalRepoFixture<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext> : IDisposable
//        where TTemporalRepo : ITemporalRepo<TEntity, THistoryEntity, TContext, THistoryContext>
//        where TEntity : class, IEFCoreTemporalModel, new()
//        where THistoryEntity : TEntity
//        where TContext : DbContext //ResettableDbContext<TContext>
//        where THistoryContext: DbContext //ResettableDbContext<THistoryContext> 
//        {

//        public TestTemporalRepoFactory<TTemporalRepo, TEntity, THistoryEntity,TContext,THistoryContext> Factory { get; }
//        public TTemporalRepo Repo { get; }

//        public TestTemporalRepoFixture() {
//            Factory = new TestTemporalRepoFactory<TTemporalRepo, TEntity, THistoryEntity, TContext, THistoryContext>();
//            Repo = Factory.CreateRepo();
//        }

//        public void Dispose() {
//            Factory.ResetRepo();
//        }

//    }
//}
