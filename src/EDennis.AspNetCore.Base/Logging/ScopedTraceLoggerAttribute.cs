using EDennis.AspNetCore.Base.Logging;
using MethodBoundaryAspect.Fody.Attributes;
using Microsoft.Extensions.Configuration;
using M = Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using System;
using Serilog.Extensions.Logging;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using EDennis.AspNetCore.Base.Web;

namespace EDennis.AspNetCore.Base.Logging {
    public class ScopedTraceLoggerAttribute : OnMethodBoundaryAspect {

        /// <summary>
        /// A static logger that can be replaced, but which defaults to
        /// a Serilog logger defined in appsettings.json or 
        /// appsettings.Development.json with the configuration key
        /// equal to Loggers:Logging:ScopedTraceLogger
        /// </summary>
        public static M.ILogger Logger { get; set; }

        /// <summary>
        /// Holds registered users for the trace logger.
        /// Method entrances and/or exits will be logged only if
        /// the current user (defined in ScopeProperties) is 
        /// a registered user (and if entrance and exit logging
        /// is enabled)
        /// </summary>
        public static ConcurrentDictionary<string, bool> RegisteredKeys { get; set; }

        /// <summary>
        /// The maximum length of JSON objects and long strings in log messages.
        /// This value can be changed during runtime, but it will be changed for
        /// all
        /// </summary>
        public static int DefaultMaxLengthOfValues { get; set; } = 1000;

        public const LogLevel SCOPED_LOGGING_LEVEL = LogLevel.Trace;
        public const string SCOPED_TRACE_LOGGER_CONFIG_KEY = "Logging:Loggers:ScopedTraceLogger";
        public const string CATEGORY = "General";


        private readonly bool logEntry;
        private readonly bool logExit;
        private readonly int maxLengthOfValues = default;


        public ScopedTraceLoggerAttribute(bool logEntry = false, bool logExit = false, int maxLengthOfValues = 0) {
            this.logExit = logExit;
            this.logEntry = logEntry;
            if (maxLengthOfValues >= 0)
                this.maxLengthOfValues = DefaultMaxLengthOfValues;
            else
                this.maxLengthOfValues = maxLengthOfValues;
        }


        /// <summary>
        /// Setup default static logger
        /// </summary>
        static ScopedTraceLoggerAttribute() {

            RegisteredKeys = new ConcurrentDictionary<string, bool>();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile($"appsettings.{env}.json", true)
                .Build();

            if (!configuration.ContainsKey(SCOPED_TRACE_LOGGER_CONFIG_KEY))
                throw new ApplicationException($"Could not find '{SCOPED_TRACE_LOGGER_CONFIG_KEY}' key in appsettings files");

            //create a Serilog.Core logger
            var slogger = new LoggerConfiguration()
                .SetLogLevel(SCOPED_LOGGING_LEVEL)
                .ReadFrom.Configuration(configuration, SCOPED_TRACE_LOGGER_CONFIG_KEY)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", env)
                .SetLogLevel(SCOPED_LOGGING_LEVEL)
                .CreateLogger();

            //create the ILoggerFactory for Serilog from the Serilog.Core logger
            using var serilogLoggerFactory = new SerilogLoggerFactory(slogger);

            //create the Microsoft.Extensions.Logging.ILogger from the factory
            Logger = serilogLoggerFactory.CreateLogger(CATEGORY);

        }



        public override void OnEntry(MethodExecutionArgs args) {
            if (!logEntry || args.Method.Name.Contains("ScopeProperties"))
                return;
            var instance = args.Instance;
            var key = GetScopedTraceLoggerKey(instance);
            if (RegisteredKeys.ContainsKey(key ?? ""))
                LogEntry(args, key);
        }

        /// <summary>
        /// Logs the exit of a method.  Method exits (including the first 
        /// are logged
        /// </summary>
        /// <param name="args"></param>
        public override void OnExit(MethodExecutionArgs args) {
            if (!logExit)
                return;
            var instance = args.Instance;
            var key = GetScopedTraceLoggerKey(instance);
            if (RegisteredKeys.ContainsKey(key ?? ""))
                LogExit(args, key);            
        }

        /// <summary>
        /// Exceptions are logged regardless of user registration
        /// </summary>
        /// <param name="args"></param>
        public override void OnException(MethodExecutionArgs args) {
            var instance = args.Instance;
            var key = GetScopedTraceLoggerKey(instance);
            LogException(args,key);
        }


        public virtual void LogEntry(MethodExecutionArgs args, string key) {
            var scope = Logger.GetScope(args,maxLengthOfValues);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var message = "For {Key}, entering {Method} with arguments: {Arguments}";
            using (Logger.BeginScope(scope)) {
                LogIt(Logger.LogTrace, message, key, method, formatted);
            }
        }

        public virtual void LogExit(MethodExecutionArgs args, string key) {
            var scope = Logger.GetScope(args,maxLengthOfValues);
            var formatted = args.Arguments.FormatCompact();
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var message = "For {Key}, exiting {Method} with arguments: {Arguments}";
            using (Logger.BeginScope(scope)) {
                LogIt(Logger.LogTrace, message, key, method, formatted);
            }
        }


        public virtual void LogException(MethodExecutionArgs args, string key) {
            var scope = Logger.GetScope(args,maxLengthOfValues);
            var method = (args.Method as MethodInfo).GetFriendlyName();
            var message = "For {key}, failing {Method} with exception: {Message}";
            using (Logger.BeginScope(scope)) {
                Logger.LogError(args.Exception, message, key, method, args.Exception.Message);
            }
        }

        private bool LogIt(Action<string, string[]> action, string message, string user, string method, string args) {
            action(message, new string[] { user, method, args });
            return true;
        }



        private string GetScopedTraceLoggerKey(object instance) {
            try {
                var type = instance.GetType();
                if (type.Name.StartsWith("<"))
                    return null;
                var methodInfo = type.GetMethod("GetScopedTraceLoggerKey");
                var key = (string)methodInfo.Invoke(instance, new object[] { });
                return key;
                //var propInfo = instance.GetType().GetProperty("ScopeProperties");
                //scopeProperties = (ScopeProperties)propInfo.GetValue(instance);
                //return scopeProperties;
            } catch {
                throw new ApplicationException($"{instance.GetType().FullName} does not have method '[DisableWeaving] public string GetScopedTraceLoggerKey()=>...'.  It must have this method and the method must be decorated with [DisableWeaving] if the class or a method in the class uses the attribute [ScopedTraceLogger].");
            }
        }


    }

    internal static class LogLevelExtensions {
        internal static LoggerConfiguration SetLogLevel(this LoggerConfiguration lconfig, M.LogLevel logLevel) {
            switch (logLevel) {
                case M.LogLevel.Trace:
                    lconfig.MinimumLevel.Verbose();
                    break;
                case M.LogLevel.Debug:
                    lconfig.MinimumLevel.Debug();
                    break;
                case M.LogLevel.Information:
                    lconfig.MinimumLevel.Information();
                    break;
                case M.LogLevel.Warning:
                    lconfig.MinimumLevel.Warning();
                    break;
                case M.LogLevel.Error:
                    lconfig.MinimumLevel.Error();
                    break;
                case M.LogLevel.Critical:
                    lconfig.MinimumLevel.Fatal();
                    break;
                default:
                    break;
            }

            return lconfig;
        }

    }


}