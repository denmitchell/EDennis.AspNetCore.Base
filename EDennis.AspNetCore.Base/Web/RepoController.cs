using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This type of controller is used to communicate
    /// with custom repositories -- classes that 
    /// encapsulate communication with a database.
    /// </summary>
    public abstract class RepoController : ControllerBase {

        /// <summary>
        /// The HEAD endpoint is used for testing purposes
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpHead] public virtual void Head() { }


        /// <summary>
        /// Replaces repos injected by the framework
        /// with repos from the relevant entry in the
        /// DbContextBaseCache (singleton).  This method
        /// is for testing purposes only.
        /// </summary>
        /// <param name="dict"></param>
        [ApiExplorerSettings(IgnoreApi = true)]
        public abstract void ReplaceRepos(Dictionary<string, DbContextBase> dict);
    }
}