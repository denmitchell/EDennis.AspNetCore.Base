using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDennis.AspNetCore.Base {
    public class ApiSettingsFacade {

        public IConfiguration Configuration { get; set; }
        public string ParentConfigurationKey { get; set; }

        public string ProjectName {
            get {
                return Configuration[$"{ParentConfigurationKey}:ProjectName"];
            }
            set {
                Configuration[$"{ParentConfigurationKey}:ProjectName"] = value;
            } 
        }
        public string Scheme {
            get {
                return Configuration[$"{ParentConfigurationKey}:Scheme"];
            }
            set {
                Configuration[$"{ParentConfigurationKey}:Scheme"] = value;
            }
        }
        public string Host {
            get {
                return Configuration[$"{ParentConfigurationKey}:Host"];
            }
            set {
                Configuration[$"{ParentConfigurationKey}:Host"] = value;
            }
        }
        public int HttpsPort {
            get {
                return int.Parse(Configuration[$"{ParentConfigurationKey}:HttpsPort"]);
            }
            set {
                Configuration[$"{ParentConfigurationKey}:HttpsPort"] = value.ToString();
            }
        }
        public int HttpPort {
            get {
                return int.Parse(Configuration[$"{ParentConfigurationKey}:HttpPort"]);
            }
            set {
                Configuration[$"{ParentConfigurationKey}:HttpPort"] = value.ToString();
            }
        }
        public decimal Version {
            get {
                return decimal.Parse(Configuration[$"{ParentConfigurationKey}:Version"]);
            }
            set {
                Configuration[$"{ParentConfigurationKey}:Version"] = value.ToString();
            }
        }
        public string[] Scopes {
            get {
                return Configuration.GetSection($"{ParentConfigurationKey}:Scopes")
                    .GetChildren().ToArray().Select(c => c.Value).ToArray();
            }
            set {
                for (int i = 0; i < value.Length; i++)
                    Configuration[$"{ParentConfigurationKey}:Scopes:{i}"] = value[i];
            }
        }
        public bool NeedsLaunched {
            get {
                return bool.Parse(Configuration[$"{ParentConfigurationKey}:NeedsLaunched"]);
            }
            set {
                Configuration[$"{ParentConfigurationKey}:NeedsLaunched"] = value.ToString();
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


    }
}
