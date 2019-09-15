using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public enum LogLevelRange {
        None = -1,
        TraceOnly = LogLevel.Trace,
        DebugAndBelow = LogLevel.Debug,
        InformationAndBelow = LogLevel.Information,
        WarningAndBelow = LogLevel.Warning,
        ErrorAndBelow = LogLevel.Error,
        All = LogLevel.Critical
    }
    public enum StatusCodeRange {
        All = 0,
        OkAndAbove = HttpStatusCode.OK,
        RedirectionAndAbove = HttpStatusCode.MultipleChoices,
        BadRequestAndAbove = HttpStatusCode.BadRequest,
        InternalServerErrorAndAbove = HttpStatusCode.InternalServerError,
        None = 999,
    }
    public struct Thresholds {
        public LogLevelRange LogLevelRange { get; set; }
        public StatusCodeRange StatusCodeRange { get; set; }
    }
    public class RequestLoggingOptions {
        public LogLevelRange LogLevelRange { get; set; }
            = LogLevelRange.WarningAndBelow;
        public StatusCodeRange StatusCodeRange { get; set; }
            = StatusCodeRange.BadRequestAndAbove;
        public Thresholds Headers { get; set; } =
            new Thresholds {
                LogLevelRange = LogLevelRange.WarningAndBelow,
                StatusCodeRange = StatusCodeRange.BadRequestAndAbove
            };
        public Thresholds UserClaims { get; set; } =
            new Thresholds {
                LogLevelRange = LogLevelRange.WarningAndBelow,
                StatusCodeRange = StatusCodeRange.BadRequestAndAbove
            };
        public LogLevelRange RequestBodyLogLevel { get; set; } =
            LogLevelRange.WarningAndBelow;
        public LogLevelRange ResponseBodyLogLevel { get; set; } =
            LogLevelRange.WarningAndBelow;
        public Thresholds ExceptionMessage { get; set; } =
            new Thresholds {
                LogLevelRange = LogLevelRange.WarningAndBelow,
                StatusCodeRange = StatusCodeRange.BadRequestAndAbove
            };
        public Thresholds StackTrace { get; set; } = 
            new Thresholds {
                LogLevelRange = LogLevelRange.WarningAndBelow,
                StatusCodeRange = StatusCodeRange.BadRequestAndAbove
            };
    }

    static class HttpStatusCodeExtensions {
        public static bool In(this HttpStatusCode code, StatusCodeRange range) {
            return (int)code >= (int)range;
        }
    }

}