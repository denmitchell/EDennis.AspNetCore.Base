using EDennis.AspNetCore.Base.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Web {

    /// <summary>
    /// This action filter is used for testing purposes. The filter
    /// will perform one of two general actions.
    /// <para>
    ///   For RepoControllers, the filter will replace all injected
    ///   repos with new repos that are suitable for testing.  These 
    ///   new repos will either use in-memory databases or testing-
    ///   transaction databases (that can be rolled back).  In
    ///   addition, the filter will either create or drop/rollback
    ///   the database/connection, depending upon the nature of
    ///   the X-Testing- header passed in (see constant below)
    /// </para>
    /// <para>
    ///   For ProxyControllers, the filter will propagate all 
    ///   X-Testing headers to HttpClient instances.
    /// </para>
    /// </summary>
    public class TestingActionFilter : IActionFilter {

        //define X-Testing- header constants
        public const string HDR_PREFIX = "X-Testing-";
        public const string DEFAULT_NAMED_INSTANCE = "default";
        public const string HDR_USE_INMEMORY = HDR_PREFIX + "UseInMemory";
        public const string HDR_USE_TRANSACTION = HDR_PREFIX + "UseTransaction";
        public const string HDR_DROP_INMEMORY = HDR_PREFIX + "DropInMemory";
        public const string HDR_ROLLBACK_TRANSACTION = HDR_PREFIX + "RollbackTransaction";


        /// <summary>
        /// Constructs a new TestingActionFilter with the 
        /// provided HostingEnvironment injected in.
        /// </summary>
        /// <param name="env">hosting environment</param>
        public TestingActionFilter(IHostingEnvironment env) {
            HostingEnvironment = env;
        }

        /// The hosting environment (development, production, staging)
        public IHostingEnvironment HostingEnvironment { get; set; }


        /// <summary>
        /// Executes the filter prior to the Controller action executing.
        /// This method either replaces repos with test repos 
        /// (RepoControllers) or propagates headers (ProxyRepos).
        /// </summary>
        /// <param name="context">The context object, which provides access to the controller</param>
        public void OnActionExecuting(ActionExecutingContext context) {

            //short-circuit the method if this is not development
            if (HostingEnvironment.EnvironmentName != EnvironmentName.Development)
                return;

            //short-circuit the method if the controller is the wrong type
            if (!(context.Controller is ProxyController)
                && !(context.Controller is RepoController))
                return;

            //retrieve the X-Testing- header
            var header = GetTestingHeader(context);

            //if there is no X-Testing- header (which occurs in
            //Swagger spot-testing scenarios), create a new
            //X-Testing- header, using an in-memory database
            //with a default name
            if (header.Key == null) {
                context.HttpContext.Request.Headers.Add(HDR_USE_INMEMORY, "default");
                header = new KeyValuePair<string, string>(HDR_USE_INMEMORY, "default");
            }

            //if the target controller is a ProxyController ...
            if (context.Controller is ProxyController) {

                //propage the headers
                var ctlr = context.Controller as ProxyController;
                ctlr.PropagateHeaders(header);

            //otherwise, the controller is a RepoController ...
            } else {

                //get a reference to the DbContextBaseTestCache
                var cache =
                    context.HttpContext.RequestServices.GetService(
                        typeof(IDbContextBaseTestCache))
                        as DbContextBaseTestCache;

                //declare a dictionary to hold the relevant context classes
                Dictionary<string, DbContextBase> dict = null;

                //drop the in-memory database
                if (header.Key == HDR_DROP_INMEMORY)
                    cache.DropInMemoryContexts(header.Value);

                //rollback the current transaction
                else if (header.Key == HDR_ROLLBACK_TRANSACTION)
                    cache.DropTestingTransactionContexts(header.Value);

                //use an in-memory database (adding it if necessary)
                else if (header.Key == HDR_USE_INMEMORY)
                    dict = cache.GetOrAddInMemoryContexts(header.Value);

                //use a testing-transaction (adding it if necessary)
                else if (header.Key == HDR_USE_TRANSACTION)
                    dict = cache.GetOrAddTestingTransactionContexts(header.Value);

                //replace all of the repositories in the controller
                if (dict != null && dict.Count > 0) {
                    var ctlr = context.Controller as RepoController;
                    ctlr.ReplaceRepos(dict);
                }

            }

        }

        /// <summary>
        /// Retrieves the X-Testing- header from the request message
        /// </summary>
        /// <param name="context">provides access to the Request headers</param>
        /// <returns></returns>
        private KeyValuePair<string, string> GetTestingHeader(ActionExecutingContext context) {

            //retrieve a list of all X-Testing- headers 
            var headers = context.HttpContext.Request.Headers
                            .Where(h => h.Key.StartsWith(HDR_PREFIX));

            //if there is no X-Testing- header, return a default KeyValuePair
            if (headers.Count() == 0)
                return new KeyValuePair<string, string>(null, null);

            //if there are duplicate headers, throw an exception
            else if (headers.Count() > 1)
                throw new DuplicateHeaderException(headers);

            //set a reference to the X-Testing- header
            var header = headers.FirstOrDefault();

            //if there are duplicate header values, throw an exception
            if (header.Value.Count() > 1)
                throw new DuplicateHeaderException(header);

            //set a reference to the X-Testing- header and first value
            var simpleHeader = new KeyValuePair<string, string>(header.Key, 
                header.Value.FirstOrDefault());

            //return the header
            return simpleHeader;
        }


        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context) {
        }

    }
}
