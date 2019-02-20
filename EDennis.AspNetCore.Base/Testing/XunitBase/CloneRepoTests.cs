using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {

    public class CloneRepoTests<TRepo, TEntity, TContext> : IClassFixture<CloneClassFixture<TContext>>, IDisposable
        where TEntity : class, new()
        where TContext : DbContext
        where TRepo : SqlRepo<TEntity, TContext> {

        protected readonly ITestOutputHelper _output;
        protected readonly TContext _context;
        protected readonly TRepo _repo;

        protected readonly CloneConnections _cloneConnections;
        protected readonly BlockingCollection<int> _cloneIndexPool;
        protected readonly int _cloneIndex;

        public CloneRepoTests(ITestOutputHelper output, CloneClassFixture<TContext> cloneFixture) {
            _output = output;

            _cloneConnections = cloneFixture.CloneConnections;
            _cloneIndexPool = cloneFixture.CloneIndexPool;

            _cloneIndex = _cloneIndexPool.Take();

            _context = TestDbContextManager<TContext>.GetDatabaseClone(
                _cloneConnections,typeof(TContext).Name,_cloneIndex);

            //using reflection, instantiate the repo
            _repo = Activator.CreateInstance(typeof(TRepo),
                new object[] { _context }) as TRepo;

        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    DatabaseCloneManager.ReleaseClones(_cloneConnections,_cloneIndex);
                    _cloneIndexPool.Add(_cloneIndex);
                }
                disposedValue = true;
            }
        }

        public void Dispose() {
            Dispose(true);
        }
        #endregion
    }
}
