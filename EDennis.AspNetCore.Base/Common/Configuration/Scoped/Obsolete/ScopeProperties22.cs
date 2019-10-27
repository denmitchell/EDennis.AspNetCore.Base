using EDennis.AspNetCore.Base.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public class ScopeProperties22 {
        private readonly ILoggerChooser _loggerChooser;

        public ScopeProperties22(ILoggerChooser loggerChooser = null) {
            _loggerChooser = loggerChooser;
            UpdateLoggerIndex();
        }

        public int LoggerIndex { get; set; } = 0;
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public HttpHeaders Headers { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();


        public void UpdateLoggerIndex() {
            if (_loggerChooser == null)
                LoggerIndex = ILoggerChooser.DefaultIndex;
            else
                LoggerIndex = _loggerChooser.GetLoggerIndex(this);
        }



    }


    public static class HttpHeadersExtensions {
        public static void AddOrReplace(this HttpHeaders headers, string key, IEnumerable<string> values) {
            headers.Remove(key);
            headers.Add(key, values);
        }
        public static void AddOrReplace(this HttpHeaders headers, string key, string value) {
            headers.Remove(key);
            headers.Add(key, value);
        }
    }

}
