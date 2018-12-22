using System;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Web {
    /// <summary>
    /// Implementations of this interface can be
    /// used to communicate with REST Service APIs.
    /// <para>
    ///   HTTP Service proxies (or API proxies) are classes
    ///   that provide access to HTTP services defined outside
    ///   of the current project -- either services hosted
    ///   by other organizations or services defined in 
    ///   other projects/solutions.  An HttpClient 
    ///   provides methods to communicate with the
    ///   service.
    /// </para>
    /// </summary>
    public interface IApiProxy {
        //object used to communicate with REST Service API
        HttpClient HttpClient { get; set; }

        //the URI for sending a HEAD request for resetting
        //a database/connection during testing
        Uri ResetUri { get; }
    }
}
