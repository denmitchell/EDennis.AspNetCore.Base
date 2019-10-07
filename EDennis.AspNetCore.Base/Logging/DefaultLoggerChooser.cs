using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Default implementation of LoggerChooserSpec, which produces a
    /// key:value pair (as string) for User.  For example: "User:Moe"
    /// </summary>
    public class DefaultLoggerChooser : LoggerChooser
    {
        /// <summary>
        /// Construct a new DefaultLoggerChooser
        /// </summary>
        /// <param name="loggers"></param>
        public DefaultLoggerChooser(IEnumerable<ILogger<object>> loggers) : base(loggers) {
        }

        /// <summary>
        /// Implementation of GetInputData, returning User:{user}
        /// </summary>
        /// <param name="scopeProperties"></param>
        /// <returns></returns>
        protected override IEnumerable<string> GetInputData(ScopeProperties scopeProperties) {
            return new string[] { $"User:{scopeProperties.User}" };
        }

        /// <summary>
        /// Simplification of AddCriterion where the user is passed in, rather than "User:{user}"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loggerIndex"></param>
        public override void AddCriterion(string user, int loggerIndex) => base.AddCriterion($"User:{user}", loggerIndex);

        /// <summary>
        /// Simplification of RemoveCriterion where the user is passed in, rather than "User:{user}"
        /// </summary>
        /// <param name="user"></param>
        /// <param name="loggerIndex"></param>
        public override void RemoveCriterion(string user, int loggerIndex) => base.RemoveCriterion($"User:{user}", loggerIndex);


    }
}
