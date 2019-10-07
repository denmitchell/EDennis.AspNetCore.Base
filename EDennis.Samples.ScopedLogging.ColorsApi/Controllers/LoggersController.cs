using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Controllers {
    /// <summary>
    /// Sample subclass of LoggersControllerBase
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoggersController : LoggersControllerBase {

        public LoggersController(IEnumerable<ILogger<object>> loggers, ILoggerChooser loggerChooser)
            :base (loggers, loggerChooser){}

    }

}