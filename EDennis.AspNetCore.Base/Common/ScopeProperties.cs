using EDennis.AspNetCore.Base.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base {
    public class ScopeProperties {
        private readonly ILoggerChooser _loggerChooser;

        public ScopeProperties(ILoggerChooser loggerChooser = null) {
            _loggerChooser = loggerChooser;
            UpdateLoggerIndex();
        }

        public int LoggerIndex { get; set; } = 0;
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();


        public void UpdateLoggerIndex() {
            if (_loggerChooser == null)
                LoggerIndex = ILoggerChooser.DefaultIndex;
            else
                LoggerIndex = _loggerChooser.GetLoggerIndex(this);
        }

        public void PopulateHttpClientHeadersWithStoredData(HttpClient client) {
            if(User != null)
                AddOrReplaceHeader(client, "X-User", User);

            var headers = Headers.GroupBy(c => c.Key)
                .Select(g => KeyValuePair.Create(g.Key, g.Select(v => v.Value)));

            foreach (var header in headers) {
                if (header.Value.Count() == 1)
                    AddOrReplaceHeader(client, header.Key, header.Value.First());
                else
                    AddOrReplaceHeader(client, header.Key, header.Value);
            }

            var claims = Claims.GroupBy(c => c.Type)
                .Select(g => KeyValuePair.Create(g.Key, g.Select(v => v.Value)));

            foreach (var claim in claims)  {
                if(claim.Value.Count() == 1)
                    AddOrReplaceHeader(client, $"X-Claim-{claim.Key}", claim.Value.First());
                else
                    AddOrReplaceHeader(client, $"X-Claim-{claim.Key}", claim.Value);
            }

        }

        private void AddOrReplaceHeader(HttpClient client, string headerKey, string headerValue) {
            if (client.DefaultRequestHeaders.Contains(headerKey))
                client.DefaultRequestHeaders.Remove(headerKey);

            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        }
        private void AddOrReplaceHeader(HttpClient client, string headerKey, IEnumerable<string> headerValue) {
            if (client.DefaultRequestHeaders.Contains(headerKey))
                client.DefaultRequestHeaders.Remove(headerKey);

            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        }
    }


}
