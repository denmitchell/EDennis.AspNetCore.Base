using EDennis.AspNetCore.Base.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base
{
    public class ScopeProperties
    {
        private readonly ILoggerChooser _loggerChooser;

        public ScopeProperties(ILoggerChooser loggerChooser = null) {
            _loggerChooser = loggerChooser;
            UpdateLoggerIndex();
        }

        public int LoggerIndex { get; set; } = 0;
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();


        public void UpdateLoggerIndex() {
            if (_loggerChooser == null)
                LoggerIndex = ILoggerChooser.DefaultIndex;
            else
                LoggerIndex = _loggerChooser.GetLoggerIndex(this);
        }


    }
}
