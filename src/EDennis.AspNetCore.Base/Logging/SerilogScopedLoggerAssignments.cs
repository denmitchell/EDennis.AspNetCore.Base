using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using M = Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging {
    /// <summary>
    /// Provides a cache of five loggers and different logging levels, as well 
    /// as a dictionary of assignments of users (or potentially other entities) to 
    /// particular loggers at a log level.
    /// </summary>
    public class SerilogScopedLoggerAssignments : IScopedLoggerAssignments {

        public Dictionary<M.LogLevel, M.ILogger> Loggers { get; private set; } = new Dictionary<M.LogLevel,M.ILogger>();
        public ConcurrentDictionary<string, M.LogLevel> Assignments { get; set; } = new ConcurrentDictionary<string, M.LogLevel>();

        public SerilogScopedLoggerAssignments(IConfiguration config, string scopedLoggerKey) {

            using var factory = new M.LoggerFactory();
            for(int i = (int)M.LogLevel.Trace; i < (int)M.LogLevel.None; i++) {
                Loggers.Add((M.LogLevel)i,CreateLogger(factory, config, scopedLoggerKey, (M.LogLevel)i));
            }
            //Assignments.AddOrUpdate("Anonymous", M.LogLevel.Trace, (k, v) => v);

        }


        public string Category { get; } = "General";


        public M.ILogger GetLogger(string key) {
            if (!Assignments.ContainsKey(key))
                return new NullScopedLogger().Logger;
            else
                return Loggers[Assignments[key]];
        }



        /// <summary>
        /// Constructs a new logger using the injected logger factory and configuration
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="configuration"></param>
        public M.ILogger CreateLogger(M.ILoggerFactory _, IConfiguration configuration, string sectionName, M.LogLevel logLevel) {

            //create a Serilog.Core logger
            var slogger = new LoggerConfiguration()
                .SetLogLevel(logLevel)
                .ReadFrom.Configuration(configuration, sectionName)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .SetLogLevel(logLevel)
                .CreateLogger();

            //create the ILoggerFactory for Serilog from the Serilog.Core logger
            using var serilogLoggerFactory = new SerilogLoggerFactory(slogger);

            //create the Microsoft.Extensions.Logging.ILogger from the factory
            return serilogLoggerFactory.CreateLogger(Category);
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
