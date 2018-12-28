using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This type of websocket hub is used to communicate
    /// with custom repositories -- classes that 
    /// encapsulate communication with a database.
    /// </summary>
    public abstract class RepoHub : Hub {

        /// <summary>
        /// Replaces repos injected by the framework
        /// with repos from the relevant entry in the
        /// DbContextBaseCache (singleton).  This method
        /// is for testing purposes only.
        /// </summary>
        /// <param name="dict"></param>
        public abstract void ReplaceRepos(Dictionary<string, DbContextBase> dict);
    }
}