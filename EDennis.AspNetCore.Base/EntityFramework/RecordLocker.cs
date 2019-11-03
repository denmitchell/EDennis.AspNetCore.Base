using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EDennis.AspNetCore.Base.EntityFramework {


    /// <summary>
    /// This class uses EventWaitHandles and Timers to "lock"
    /// records -- cause a thread to block which attempts to
    /// lock a record that is already locked.  The Timer 
    /// automatically releases the lock after a provided
    /// timeout period.  All locks are disposed when this
    /// class is disposed.
    /// NOTE: This class should be instantiated as a singleton
    /// </summary>
    public class RecordLocker : IDisposable {

        class Lock {
            public EventWaitHandle EventWaitHandle { get; set; }
            public Timer Timer { get; set; }

        }



        private readonly ConcurrentDictionary<string, Lock> _recordLocks
            = new ConcurrentDictionary<string, Lock>();


        public void LockRecord(int lockTimeout, params object[] keyValues) {
            var keyValuesString = string.Join(',', keyValues.Select(v => v.ToString()));
            var lockKey = $"{GetType().Name}({keyValuesString})";

            var recordLock = _recordLocks.GetOrAdd(lockKey, new Lock {
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
            var found = _recordLocks.TryGetValue(lockKey, out Lock recordLock);
            if (found) {
                recordLock.EventWaitHandle.Set();
                _recordLocks.TryRemove(lockKey, out _);
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    //ensure EventWaitHandles are disposed
                    foreach (var entry in _recordLocks) {
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
