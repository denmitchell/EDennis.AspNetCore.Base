using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.EntityFramework {


    public class UndoRecord {
        public Operation Operation { get; set; }
        public dynamic Entity { get; set; }
        public object[] KeyValues { get; set; }
        public DateTime SysStart { get; set; }
        public DateTime Expiration { get; set; }

    }

    public class UndoStack<TEntity, TContext> : IDisposable 
        where TEntity : class, IHasSysUser, IHasKeyValues, new()
        where TContext :  DbContext {

        private readonly ConcurrentDictionary<string, List<UndoRecord>> _userStacks
            = new ConcurrentDictionary<string, List<UndoRecord>>();

        private IOptionsMonitor<AppSettings> _appSettings;

        public UndoStack(IOptionsMonitor<AppSettings> appSettings) {
            _appSettings = appSettings;
        }

        public void Push(string sysUser, dynamic entity, Operation operation, params object[] keyValues) {
            var userStack = _userStacks.GetOrAdd(sysUser, new List<UndoRecord>());
            userStack.Add(new UndoRecord {
                Operation = operation,
                Entity = entity,
                KeyValues = keyValues,
                SysStart = DateTime.Now,
                Expiration = DateTime.Now.Add(_appSettings.CurrentValue.UndoTimeSpan)
            });            
        }

        public async Task UndoAsync(IRepo<TEntity, TContext> repo, string sysUser, TimeSpan timeSpan, params object[] keyValues) {
            var found = _userStacks.TryGetValue(sysUser, out List<UndoRecord> userStack);
            if (found) {
                var MinSysStart = DateTime.Now.Subtract(timeSpan);
                var undoRecords = userStack
                    .Where(s => s.KeyValues.SequenceEqual(keyValues)
                        && s.Expiration < DateTime.Now
                        && s.SysStart > MinSysStart)
                    .Reverse();
                foreach(var undoRecord in undoRecords) {
                    userStack.Remove(undoRecord);
                    await repo.ExecuteAsync(undoRecord.Operation, undoRecord.Entity, undoRecord.KeyValues);
                }
            }
        }



        class Lock {
            public EventWaitHandle EventWaitHandle { get; set; }
            public Timer Timer { get; set; }

        }






        public void LockRecord(int lockTimeout, params object[] keyValues) {
            var keyValuesString = string.Join(',', keyValues.Select(v => v.ToString()));
            var lockKey = $"{GetType().Name}({keyValuesString})";

            var recordLock = _userStacks.GetOrAdd(lockKey, new Lock {
                EventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, lockKey),
                Timer = new Timer(OnTimeout, keyValues, lockTimeout, Timeout.Infinite)
            }) ;
            recordLock.EventWaitHandle.WaitOne();
        }

        private void OnTimeout(object state) {
            UnlockRecord(state);
        }


        public void UnlockRecord(params object[] keyValues) {
            var keyValuesString = string.Join(',', keyValues.Select(v => v.ToString()));
            var lockKey = $"{GetType().Name}({keyValuesString})";
            var found = _userStacks.TryGetValue(lockKey, out Lock recordLock);
            if (found) {
                recordLock.EventWaitHandle.Set();
                _userStacks.TryRemove(lockKey, out _);
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    //ensure EventWaitHandles are disposed
                    foreach (var entry in _userStacks) {
                        entry.Value.EventWaitHandle.Close();
                    }
                }
                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}
