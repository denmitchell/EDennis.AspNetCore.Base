using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base.Logging {

    public class ScopedLoggingOptions {




        /// <summary>
        /// LoggerFactory to be used for generating scoped logger.
        /// Note that this must be used to set the LogLevel for the logger
        /// </summary>
        public ILoggerFactory LoggerFactory { get; set; } 
            = new ConsoleLoggerFactory(LogLevel.Trace);

        /// <summary>
        /// This must be true to propagate header to child APIs
        /// </summary>
        public bool PropagateHeader { get; set; } = false;

    }



}
