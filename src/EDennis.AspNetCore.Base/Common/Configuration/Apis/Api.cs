using EDennis.AspNetCore.Base.Web;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base {
    public class Api {


        public string ProjectName { get; set; }
        public string Host { get; set; } = "localhost";
        public string Scheme { get; set; } = "https";
        public int? HttpsPort { get; set; }
        public int? HttpPort { get; set; }
        public decimal Version { get; set; } = 1.0M;
        public OAuth OAuth { get; set; }
        public Oidc Oidc { get; set; }


        /// <summary>
        /// One or more security scopes for accessing the API
        /// NOTE: the implementation contains a workaround to replace default scope when explicitly set
        /// </summary>
        public string[] Scopes { get; set; }



        public static readonly ClaimsToHeaders DEFAULT_CLAIMS_TO_HEADERS =
                    new ClaimsToHeaders {
                        { JwtClaimTypes.Name, Constants.USER_KEY },
                        { JwtClaimTypes.Role, Constants.ROLE_KEY }
                    };

        public static readonly HeadersToHeaders DEFAULT_HEADERS_TO_HEADERS =
                    new HeadersToHeaders {
                        { Constants.TESTING_INSTANCE_KEY, Constants.TESTING_INSTANCE_KEY },
                        { Constants.SET_SCOPEDLOGGER_KEY, Constants.SET_SCOPEDLOGGER_KEY },
                        { Constants.CLEAR_SCOPEDLOGGER_KEY, Constants.SET_SCOPEDLOGGER_KEY },
                        { Constants.USER_KEY, Constants.USER_KEY },
                        { Constants.ROLE_KEY, Constants.ROLE_KEY },
                        { Constants.HOSTPATH_KEY, Constants.HOSTPATH_KEY }
                    };

        private ApiMappings _mappings;

        public ApiMappings Mappings { 
            get {
                return _mappings ?? new ApiMappings {
                    ClaimsToHeaders = DEFAULT_CLAIMS_TO_HEADERS,
                    HeadersToHeaders = DEFAULT_HEADERS_TO_HEADERS
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
