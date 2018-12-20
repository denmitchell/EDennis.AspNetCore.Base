using EDennis.AspNetCore.Base.EntityFramework;
using EDennis.AspNetCore.Base.Web;
using System;
using System.Collections.Generic;
using System.Net.Http;
using static EDennis.AspNetCore.Base.Web.TestingActionFilter;


namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This class provides HttpClient extension methods
    /// that support integration testing scenarios.  For
    /// example, a common pattern is to 
    /// (a) POST to create a new record from a provided object
    /// (b) GET to retrieve the created object from the record (to verify the insert)
    /// (c) Drop the testing database
    /// This class provides a method that can accomplish the three
    /// steps above in just one line of code.
    /// </summary>
    public static partial class HttpClientExtensions {

        #region Helper Methods


        /// <summary>
        /// Explicitly adds an X-Testing-Use... header to an 
        /// HttpRequestMessage
        /// </summary>
        /// <param name="msg">the message to which a header will be added</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name for the test db or transaction</param>
        private static void UseTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            string key = "";
            if (testDbType == DbType.InMemory) {
                key = HDR_USE_INMEMORY;
            } else {
                key = HDR_USE_TRANSACTION;
            }
            msg.Headers.Add(key, testDb);
        }

        /// <summary>
        /// Explicitly adds an X-Testing-Drop/Rollback... 
        /// header to an HttpRequestMessage
        /// </summary>
        /// <param name="msg">the message to which a header will be added</param>
        /// <param name="testDbType">InMemory or Transaction</param>
        /// <param name="testDb">The name for the test db or transaction</param>
        private static void DropTestDbHeader(this HttpRequestMessage msg, DbType testDbType, string testDb) {
            string key = "";
            if (testDbType == DbType.InMemory) {
                key = HDR_DROP_INMEMORY;
            } else {
                key = HDR_ROLLBACK_TRANSACTION;
            }
            msg.Headers.Add(key, testDb);
        }

        #endregion


    }


}


