using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Base controller that can be used with ILoggerChooser to enable different
    /// loggers (at different log levels) for different users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class LoggersControllerBase : ControllerBase {
        private readonly IEnumerable<ILogger<object>> _loggers;
        private readonly ILoggerChooser _loggerChooser;

        /// <summary>
        /// Constructs a new Controller from the injected collection of loggers and logger chooser
        /// </summary>
        /// <param name="loggers">all available loggers in the DI</param>
        /// <param name="loggerChooser">a singleton for enabling different loggers for different users</param>
        public LoggersControllerBase(IEnumerable<ILogger<object>> loggers, ILoggerChooser loggerChooser) {
            _loggers = loggers;
            _loggerChooser = loggerChooser;
        }


        /// <summary>
        /// Adds a user/logger setting as enables the logger
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("[action]/{loggerNameOrIndex}/{user}")]
        public IActionResult Enable(string loggerNameOrIndex, string user) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.AddCriterion(user, loggerIndex);
            _loggerChooser.Enable(loggerIndex);
            return GetLoggers();
        }


        /// <summary>
        /// Enables a specific logger
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <returns></returns>
        [HttpPost("[action]/{loggerNameOrIndex}")]
        public IActionResult Enable(string loggerNameOrIndex) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.Enable(loggerIndex);
            return GetLoggers();
        }


        /// <summary>
        /// Disables a specific logger
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <returns></returns>
        [HttpPost("[action]/{loggerNameOrIndex}")]
        public IActionResult Disable(string loggerNameOrIndex) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.Disable(loggerIndex);
            return GetLoggers();
        }



        /// <summary>
        /// Adds a specific user/logger setting
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("[action]/{loggerNameOrIndex}/{user}")]
        public IActionResult Add(string loggerNameOrIndex, string user) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.AddCriterion(user, loggerIndex);
            return GetLoggers();
        }


        /// <summary>
        /// Removes a specific user/logger setting
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("[action]/{loggerNameOrIndex}/{user}")]
        public IActionResult Remove(string loggerNameOrIndex, string user) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.RemoveCriterion(user, loggerIndex);
            return GetLoggers();
        }


        /// <summary>
        /// Clears all criteria for a given logger
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <returns></returns>
        [HttpPost("[action]/{loggerNameOrIndex}")]
        public IActionResult Clear(string loggerNameOrIndex) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.ClearCriteria(loggerIndex);
            return GetLoggers();
        }




        /// <summary>
        /// Resets ILoggerChooser such that only the default logger is enabled
        /// and all criteria and wiped out.
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult Reset() {
            _loggerChooser.Reset();
            return GetLoggers();
        }



        /// <summary>
        /// Returns information for all loggers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetLoggers() {

            List<dynamic> settings = new List<dynamic>();
            for (int i = 0; i < _loggers.Count(); i++) {
                settings.Add(new {
                    Name = GetLoggerName(i),
                    Index = i,
                    LogLevel = _loggers.ElementAt(i).EnabledAt().ToString(),
                    Enabled = _loggerChooser.IsEnabled(i),
                    Criteria = _loggerChooser.GetSettings()
                        .Where(s => s.Value == i)
                        .Select(s => s.Key)
                        .ToList()
                }); 
            }
            return Ok(settings);
        }


        /// <summary>
        /// Gets the name of the logger, minus any type parameter stuff at the end
        /// </summary>
        /// <param name="loggerIndex"></param>
        /// <returns></returns>
        private string GetLoggerName(int loggerIndex) {
            var name = _loggers.ElementAt(loggerIndex).GetType().Name;
            var endOfName = name.IndexOf("`");
            if (endOfName == -1)
                endOfName = name.Length;
            return name.Substring(0,endOfName);
        }


        /// <summary>
        /// Gets the index associated with the logger name or index.
        /// (Allows for flexible params -- can use logger index or name
        /// </summary>
        /// <param name="loggerNameOrIndex"></param>
        /// <returns></returns>
        private int GetLoggerIndex(string loggerNameOrIndex) {

            var isInt = int.TryParse(loggerNameOrIndex, out int index);
            if (isInt)
                return index;

            for (int i = 0; i < _loggers.Count(); i++)
                if (GetLoggerName(i).Equals(loggerNameOrIndex, StringComparison.OrdinalIgnoreCase))
                    return i;

            return -1;
        }



    }

}