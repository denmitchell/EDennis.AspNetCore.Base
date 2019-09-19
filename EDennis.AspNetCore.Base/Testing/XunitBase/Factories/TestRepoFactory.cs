using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {
    public class TestRepoFactory {

        public const string DEFAULT_USER = "tester@example.org";
        public const string READONLY_INSTANCE_NAME = "readonly";

        public static TRepo CreateReadonlyRepo<

            TRepo, TEntity, TContext, T>(ConfigurationFactory<T> factory)
            where TEntity : class, new()
            where TContext : DbContext
            where TRepo : ReadonlyRepo<TEntity, TContext>
            where T : class {


            var context = TestDbContextManager<TContext>.GetReadonlyDatabase(
                factory.Configuration);

            var repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { context }) as TRepo;

            return repo;

        }

        public static TRepo CreateReadonlyTemporalRepo<

            TRepo, TEntity, TContext, THistoryContext, T>(ConfigurationFactory<T> factory)
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext : DbContext
            where TRepo : ReadonlyTemporalRepo<TEntity, TContext, THistoryContext> 
            where T : class {


            var context = TestDbContextManager<TContext>.GetReadonlyDatabase(
                factory.Configuration);

            var historyContext = TestDbContextManager<THistoryContext>.GetReadonlyDatabase(
                factory.Configuration);

            var repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { context, historyContext }) as TRepo;

            return repo;

        }


        public static TRepo CreateWriteableRepo<

            TRepo, TEntity, TContext, T>(ConfigurationFactory<T> factory,
            string testUser = DEFAULT_USER
            )
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext
            where TRepo : WriteableRepo<TEntity, TContext>
            where T : class {

            var databaseName = factory.Configuration.GetDatabaseName<TContext>();
            var instanceName = Guid.NewGuid().ToString();

            TestDbContextManager<TContext>.CreateInMemoryDatabase(
                databaseName, instanceName, out DbContextOptions<TContext> options, 
                out TContext context);

            var scopeProperties = new ScopeProperties {
                User = testUser
            };


            scopeProperties.OtherProperties.Add("InstanceName", instanceName);
            scopeProperties.OtherProperties.Add("DbContextOptions", options);

            var repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { context, scopeProperties }) as TRepo;

            return repo;

        }


        public static TRepo CreateWriteableTemporalRepo<
            TRepo, TEntity, TContext, THistoryContext, T>(ConfigurationFactory<T> factory,
            string testUser = DEFAULT_USER
            )

            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext : DbContext
            where TRepo : WriteableTemporalRepo<TEntity, TContext, THistoryContext>
            where T : class {


            var databaseName = factory.Configuration.GetDatabaseName<TContext>();
            var instanceName = Guid.NewGuid().ToString();

            var historyDatabaseName = factory.Configuration.GetDatabaseName<THistoryContext>();
            var historyInstanceName = instanceName + Interceptor.HISTORY_INSTANCE_SUFFIX;


            TestDbContextManager<TContext>.CreateInMemoryDatabase(
                databaseName, instanceName, out DbContextOptions<TContext> options,
                out TContext context);

            TestDbContextManager<THistoryContext>.CreateInMemoryDatabase(
                historyDatabaseName, historyInstanceName, 
                out DbContextOptions<THistoryContext> historyOptions,
                out THistoryContext historyContext);

            var scopeProperties = new ScopeProperties {
                User = testUser
            };

            scopeProperties.OtherProperties.Add("InstanceName", instanceName);
            scopeProperties.OtherProperties.Add("HistoryInstanceName", historyInstanceName);
            scopeProperties.OtherProperties.Add("DbContextOptions", options);
            scopeProperties.OtherProperties.Add("HistoryDbContextOptions", historyOptions);


            var repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { context, historyContext, scopeProperties }) as TRepo;

            return repo;

        }

        public static string GetInstanceName<
            TRepo, TEntity, TContext>()
            where TEntity : class, new()
            where TContext : DbContext
            where TRepo : ReadonlyRepo<TEntity, TContext> {

            return READONLY_INSTANCE_NAME;
        }

        public static string GetInstanceName<
            TRepo, TEntity, TContext, THistoryContext>()
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where TRepo : ReadonlyRepo<TEntity, TContext> {

            return READONLY_INSTANCE_NAME;
        }

        public static string GetInstanceName<
            TRepo, TEntity, TContext>(TRepo repo)
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext
            where TRepo : WriteableRepo<TEntity, TContext> {

            var instanceName = repo.ScopeProperties.OtherProperties["InstanceName"].ToString();

            return instanceName;
        }

        public static string GetInstanceName<
            TRepo, TEntity, TContext, THistoryContext>(TRepo repo)
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext : DbContext
            where TRepo : WriteableTemporalRepo<TEntity, TContext, THistoryContext> {

            var instanceName = repo.ScopeProperties.OtherProperties["InstanceName"].ToString();

            return instanceName;
        }
    }

    public static class TestRepoExtensionMethods {
        public static string GetInstanceName<TEntity, TContext>(
            this ReadonlyRepo<TEntity, TContext> repo)
            where TEntity : class, new()
            where TContext : DbContext {
            return TestRepoFactory.READONLY_INSTANCE_NAME;
        }

        public static string GetInstanceName<TEntity, TContext, THistoryContext>(
            this ReadonlyTemporalRepo<TEntity, TContext, THistoryContext> repo)
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext : DbContext {
            return TestRepoFactory.READONLY_INSTANCE_NAME;
        }

        public static string GetInstanceName<TEntity, TContext>(
            this WriteableRepo<TEntity, TContext> repo)
            where TEntity : class, IHasSysUser, new()
            where TContext : DbContext {
            return GetInstanceName(repo.ScopeProperties);
        }

        public static string GetInstanceName<TEntity, TContext, THistoryContext>(
            this WriteableTemporalRepo<TEntity, TContext, THistoryContext> repo)
            where TEntity : class, IEFCoreTemporalModel, new()
            where TContext : DbContext
            where THistoryContext : DbContext
            {
            return GetInstanceName(repo.ScopeProperties);
        }

        private static string GetInstanceName(ScopeProperties scopeProperties) {
            return scopeProperties.OtherProperties["InstanceName"].ToString();
        }



    }



}


