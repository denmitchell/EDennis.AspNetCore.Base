using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Interface for a LoggerChooser, which is a specification for singletons
    /// that can enable loggers in a class based upon information in ScopeProperties
    /// -- a Scope-lifetime object that can hold data such as user names and claims.
    /// </summary>
    public interface ILoggerChooser
    {
        /// <summary>
        /// The index to be used for the default logger (typically zero)
        /// </summary>
        static int DefaultIndex;

        /// <summary>
        /// Returns whether a specific logger is enabled
        /// </summary>
        /// <param name="loggerIndex"></param>
        /// <returns></returns>
        bool IsEnabled(int loggerIndex);

        /// <summary>
        /// Enables a specific logger, regardless of ScopeProperties data
        /// </summary>c
        /// <param name="loggerIndex"></param>
        void Enable(int loggerIndex);

        /// <summary>
        /// Disables a specific logger, regardless of ScopeProperties data
        /// </summary>
        /// <param name="loggerIndex"></param>
        void Disable(int loggerIndex);

        /// <summary>
        /// Adds a ScopeProperties-related criterion, which can later be used
        /// to determine which of the enabled loggers will write logs.
        /// </summary>
        /// <param name="scopePropertiesEntry">A key:value pair as a single string</param>
        /// <param name="loggerIndex"></param>
        void AddCriterion(string scopePropertiesEntry, int loggerIndex);

        /// <summary>
        /// Removes a ScopeProperties-related criterion.
        /// </summary>
        /// <param name="scopePropertiesEntry">A key:value pair as a single string</param>
        /// <param name="loggerIndex"></param>
        void RemoveCriterion(string scopePropertiesEntry, int loggerIndex);


        /// <summary>
        /// Clear all criteria for a logger
        /// </summary>
        void ClearCriteria(int loggerIndex);


        /// <summary>
        /// Clear all criteria for all loggers and enables only the default logger
        /// </summary>
        void Reset();


        /// <summary>
        /// Gets the logger to be used in the class, based upon ScopeProperties data
        /// </summary>
        /// <param name="scopeProperties"></param>
        /// <returns></returns>
        int GetLoggerIndex(ScopeProperties scopeProperties);


        /// <summary>
        /// Gets all settings for all loggers (used for printing purposes)
        /// </summary>
        /// <returns></returns>
        Dictionary<string, int> GetSettings();
    }
}