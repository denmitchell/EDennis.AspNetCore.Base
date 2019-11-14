using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class OAuth : Api {

        private string _authority;
        private string _audience;
        private string _secret;
        public const string DEFAULT_LOCALHOST_SECRET = "secret";

        public string ClientSecret {
            get {
                return _secret
                    ?? (Host.Equals("localhost", StringComparison.OrdinalIgnoreCase) 
                        ? DEFAULT_LOCALHOST_SECRET : null);
            }
            set {
                _secret = value;
            } 
        }

        public string Authority {
            get {
                return _authority ?? MainAddress;
            }
            set {
                _authority = value;
            }
        }
        public string Audience { 
            get {
                return _audience ?? GetType().Assembly.GetName().Name;
            } 
            set {
                _audience = value;
            } 
        }
        public bool RequireHttpsMetadata { get; set; } = false;
        public bool SaveTokens { get; set; } = true;
        public string ExclusionPrefix { get; set; } = "-";
        public bool ClearDefaultInboundClaimTypeMap { get; set; } = true;
    }
}
