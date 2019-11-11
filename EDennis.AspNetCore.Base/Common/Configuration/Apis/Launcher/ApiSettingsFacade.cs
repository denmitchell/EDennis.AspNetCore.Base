using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiSettingsFacade {

        public IConfiguration Configuration { get; set; }
        public string ConfigurationKey { get; set; }

        public string ProjectName {
            get {
                return Configuration[$"{ConfigurationKey}:ProjectName"];
            }
            set {
                Configuration[$"{ConfigurationKey}:ProjectName"] = value;
            } 
        }
        public string Scheme {
            get {
                return Configuration[$"{ConfigurationKey}:Scheme"];
            }
            set {
                Configuration[$"{ConfigurationKey}:Scheme"] = value;
            }
        }
        public string Host {
            get {
                return Configuration[$"{ConfigurationKey}:Host"];
            }
            set {
                Configuration[$"{ConfigurationKey}:Host"] = value;
            }
        }
        public int HttpsPort {
            get {
                return int.Parse(Configuration[$"{ConfigurationKey}:HttpsPort"]);
            }
            set {
                Configuration[$"{ConfigurationKey}:HttpsPort"] = value.ToString();
            }
        }
        public int HttpPort {
            get {
                return int.Parse(Configuration[$"{ConfigurationKey}:HttpPort"]);
            }
            set {
                Configuration[$"{ConfigurationKey}:HttpPort"] = value.ToString();
            }
        }
        public decimal Version {
            get {
                return decimal.Parse(Configuration[$"{ConfigurationKey}:Version"]);
            }
            set {
                Configuration[$"{ConfigurationKey}:Version"] = value.ToString();
            }
        }
        public string[] Scopes {
            get {
                return Configuration.GetSection($"{ConfigurationKey}:Scopes")
                    .GetChildren().ToArray().Select(c => c.Value).ToArray();
            }
            set {
                for (int i = 0; i < value.Length; i++)
                    Configuration[$"{ConfigurationKey}:Scopes:{i}"] = value[i];
            }
        }
        public bool NeedsLaunched {
            get {
                return bool.Parse(Configuration[$"{ConfigurationKey}:NeedsLaunched"]);
            }
            set {
                Configuration[$"{ConfigurationKey}:NeedsLaunched"] = value.ToString();
            }
        }

        public string[] PortArgs {
            get {
                if (Scheme.Equals("https",StringComparison.OrdinalIgnoreCase))
                    return new string[] { $"ASPNETCORE_URLS=https://localhost:{HttpsPort};http://localhost:{HttpPort}" };
                else
                    return new string[] { $"ASPNETCORE_URLS=http://localhost:{HttpPort}" };
            }
        }

        public int MainPort {
            get {
                if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpsPort;
                else
                    return HttpPort;
            }
        }

        public string BaseAddress {
            get {
                return $"{Scheme}://{Host}:{MainPort}";
            }
        }


    }
}
