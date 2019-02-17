using EDennis.AspNetCore.Base.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EDennis.AspNetCore.Base.Testing {
    public class CloneConnections : Dictionary<string, SqlConnectionAndTransaction[]>, IDisposable {

        public int CloneCount { get; set; } = 1;
        public bool AutomatedTest { get; set; } = false;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing && !AutomatedTest) {
                    foreach (var context in Keys) {
                        foreach (var cxn in this[context]) {
                            if (cxn.SqlConnection.State == ConnectionState.Open) {
                                cxn.SqlTransaction.Rollback();
                                cxn.SqlConnection.ResetIdentities();
                                cxn.SqlConnection.ResetSequences();
                                cxn.SqlConnection.Close();
                            }
                        }
                    }
                    disposedValue = true;
                }
            }
        }
        public void Dispose() {
            Dispose(true);
        }
        #endregion

    }
}

