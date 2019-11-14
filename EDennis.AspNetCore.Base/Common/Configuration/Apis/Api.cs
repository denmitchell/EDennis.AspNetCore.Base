using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class Api  {

        private string[] _scopes;
        private ApiMappings _mappings;

        public string ProjectName { get; set; }
        public string Host { get; set; } = "localhost";
        public string Scheme { get; set; } = "https";
        public int? HttpsPort { get; set; }
        public int? HttpPort { get; set; }
        public decimal Version { get; set; } = 1.0M;
        public OAuth OAuth { get; set; }
        public Oidc Oidc { get; set; }


        public string[] Scopes { 
            get {
                return _scopes ?? new string[] { $"{ProjectName}*" };
            }
            set {
                _scopes = value;
            } 
        }
        public ApiMappings Mappings { 
            get {
                return _mappings ?? new ApiMappings {
                    ClaimsToHeaders = new ClaimsToHeaders {
                        { "name", "X-User" },
                        { "role", "X-Role" }
                    },
                    HeadersToHeaders = new HeadersToHeaders{
                        { "X-Testing-Rollback",  "X-Testing-Rollback" },
                        { "X-User",  "X-User" },
                        { "X-Role",  "X-Role" },
                    }
                }; 
            } 
            set {
                _mappings = value;
            } 
        }

        public int? MainPort {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpsPort;
                else
                    return HttpPort;
            }
        }

        public int? AltPort {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpPort;
                else
                    return HttpsPort;
            }
        }

        public string MainAddress {
            get {
                if (Scheme == null)
                    return null;
                return $"{Scheme}://{Host}:{MainPort}";
            }
        }

        public string AltAddress {
            get {
                if (Scheme == null)
                    return null;
                return $"{(Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) ? "http" : "https")}://{Host}:{AltPort}";
            }
        }


        public string[] Urls {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
                    return new string[] { MainAddress };
                else
                    return new string[] { MainAddress, AltAddress };
            }
        }



    }
}
