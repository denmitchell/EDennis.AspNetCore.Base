using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace EDennis.AspNetCore.Base {


    public static class ILoggerExtensions {

        public static void LogConstructor(this ILogger logger,
            string className, string user, object data = null) {

            //note: additional wrapper included to prevent object serialization of data parameter, unless needed
            if (logger.IsEnabled(LogLevel.Information))
                logger.Log(LogLevel.Information, "{Class} constructed, User: {User}, Data: {Data}",
                    className, user, (data == null) ? "" : JToken.FromObject(data).ToString());
        }

        public static void LogMethodEnter(this ILogger logger,
            string methodName, string user, object data = null) {

            //note: additional wrapper included to prevent object serialization of data parameter, unless needed
            if (logger.IsEnabled(LogLevel.Trace))
                logger.Log(LogLevel.Trace, "METH ENTR: {Method}, User: {User}, Data: {Data:l}",
                methodName, user, (data == null) ? "" : JToken.FromObject(data).ToString());
        }

        public static void LogMethodExit(this ILogger logger, 
            string methodName, string user, object data = null) {

            //note: additional wrapper included to prevent object serialization of data parameter, unless needed
            if (logger.IsEnabled(LogLevel.Trace))
                logger.Log(LogLevel.Trace, "METH EXIT: {Method}, User: {User}, Data: {Data:l}",
                methodName, user, (data == null) ? "" : JToken.FromObject(data).ToString());            
        }

        public static void LogStatement(this ILogger logger,
            string methodName, string statement, string user, object data = null,
            [CallerLineNumber]int? lineNumber = null) {

            //note: additional wrapper included to prevent object serialization of data parameter, unless needed
            if (logger.IsEnabled(LogLevel.Trace))
                logger.Log(LogLevel.Debug, "METH STMT: {Method}, {Statement} @ Line: {Line}, User: {User}, Data: {Data:l}",
                methodName, statement, user, lineNumber ?? -1, 
                (data == null) ? "" : JToken.FromObject(data).ToString());
        }

        public static void LogException(this ILogger logger, 
            string methodName, string user, Exception ex, object data,
            [CallerLineNumber]int? lineNumber = null) {

            logger.Log(LogLevel.Error, "EXCEPTION: {Method} @ Line: {Line}, User: {User}, Data: {Data:l}; Message: {Message}, StackTrace: {StackTrace}",
                methodName, lineNumber ?? -1, user, 
                (data == null) ? "" : JToken.FromObject(data).ToString(),
                ex.Message, ex.StackTrace);
        }

    }
}
