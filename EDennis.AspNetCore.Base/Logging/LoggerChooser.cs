using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Base class for a LoggerChooser, which is a singleton that can enable
    /// loggers in a class based upon information in ScopeProperties -- a 
    /// scope-lifetime object that can hold data such as user names and claims.
    /// </summary>
    public abstract class LoggerChooser : ILoggerChooser
    {

        /// <summary>
        /// The index to be used for the default logger (typically zero)
        /// </summary>
        public static int DefaultIndex = 0;


        private readonly IEnumerable<ILogger<object>> _loggers;

        private readonly Dictionary<string, int> _settings
            = new Dictionary<string, int>();

        private readonly Dictionary<int, bool> _enabled
            = new Dictionary<int, bool>();

        //performance optimization to reduce later processing work
        private readonly Dictionary<string, int> _enabledSettings
            = new Dictionary<string, int>();


        private bool _isReset = true;

        /// <summary>
        /// Constructs a new LoggerChooser using the DI loggers for some base settings
        /// </summary>
        /// <param name="loggers"></param>
        public LoggerChooser(IEnumerable<ILogger<object>> loggers) {
            _loggers = loggers;
            Reset();
        }

        /// <summary>
        /// Returns whether a specific logger is enabled
        /// </summary>
        /// <param name="loggerIndex"></param>
        /// <returns></returns>
        public virtual bool IsEnabled(int loggerIndex) => _enabled[loggerIndex];


        /// <summary>
        /// Enables a specific logger, regardless of ScopeProperties data
        /// </summary>c
        /// <param name="loggerIndex"></param>
        public virtual void Enable(int loggerIndex) {
            _enabled[loggerIndex] = true;
            ResetEnabledSettings();
            _isReset = false;
        }


        /// <summary>
        /// Disables a specific logger, regardless of ScopeProperties data
        /// </summary>
        /// <param name="loggerIndex"></param>
        public virtual void Disable(int loggerIndex) {
            _enabled[loggerIndex] = false;
            ResetEnabledSettings();
            if (!_enabled.Where(e => e.Key != DefaultIndex).Any(e => e.Value))
                _isReset = true;
        }

        private void ResetEnabledSettings() {
            _enabledSettings.Clear();
            _settings
                .Where(s => IsEnabled(s.Value))
                .Select(s => s.Key)
                .ToList()
                .ForEach(k => _enabledSettings.Add(k, _settings[k]));
        }

        /// <summary>
        /// This abstract method must be implemented in concrete subclasses.  
        /// The method should extract relevant key:value pairs (as strings) 
        /// from ScopeProperties.  An example of a key:value pair is
        /// "User:Moe"
        /// </summary>
        /// <param name="scopeProperties"></param>
        /// <returns></returns>
        protected abstract IEnumerable<string> GetInputData(ScopeProperties scopeProperties);


        /// <summary>
        /// Adds a ScopeProperties-related criterion, which can later be used
        /// to determine which of the enabled loggers will write logs.
        /// </summary>
        /// <param name="scopePropertiesEntry">A key:value pair as a single string</param>
        /// <param name="loggerIndex"></param>
        public virtual void AddCriterion(string scopePropertiesEntry, int loggerIndex) {
            if (!_settings.ContainsKey(scopePropertiesEntry)) {
                _settings.Add(scopePropertiesEntry, loggerIndex);
            } else
                _settings[scopePropertiesEntry] = loggerIndex;
        }

        /// <summary>
        /// Removes a ScopeProperties-related criterion.
        /// </summary>
        /// <param name="scopePropertiesEntry">A key:value pair as a single string</param>
        /// <param name="loggerIndex"></param>
        public virtual void RemoveCriterion(string scopePropertiesEntry, int loggerIndex) {
            if (_settings.ContainsKey(scopePropertiesEntry))
                _settings.Remove(scopePropertiesEntry);
        }


        /// <summary>
        /// Clear all criteria for a logger
        /// </summary>
        public virtual void ClearCriteria(int loggerIndex) {
            foreach (var entry in _settings.Where(s => s.Value == loggerIndex)) {
                _settings.Remove(entry.Key);
            }
        }


        /// <summary>
        /// Clear all criteria for all loggers and enables only the default logger
        /// </summary>
        public virtual void Reset() {
            _settings.Clear();
            _loggers.ToList().ForEach(a => _enabled.Add(_enabled.Count(), false));
            _settings.Add("*", DefaultIndex);
            Enable(DefaultIndex);
            _isReset = true;
        }


        /// <summary>
        /// Gets the logger to be used in the class, based upon ScopeProperties data
        /// </summary>
        /// <param name="scopeProperties"></param>
        /// <returns></returns>
        public virtual int GetLoggerIndex(ScopeProperties scopeProperties) {

            //for speed, short-circuit all processing if default index will be returned.
            if (_isReset)
                return DefaultIndex;

            var scopePropertiesEntries = GetInputData(scopeProperties);
            foreach (var s in _enabledSettings)
                foreach (var sp in scopePropertiesEntries)
                    if (s.Key == sp)
                        return s.Value;

            return DefaultIndex;
        }

        /// <summary>
        /// Gets all settings for all loggers (used for printing purposes)
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetSettings() => _settings;


    }


}
