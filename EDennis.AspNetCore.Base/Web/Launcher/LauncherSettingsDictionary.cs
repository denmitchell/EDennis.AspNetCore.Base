using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class LauncherSettingsDictionary : ConcurrentDictionary<string, LauncherSettings> {

        public LauncherSettings GetOrAddLauncherSettings(ApiSettings apiSettings) {
            int[] portsArray = null;
            var launcherSettings = GetOrAdd(apiSettings.ProjectName, key => {
                portsArray = PortInspector.GetRandomAvailablePorts(2).ToArray();
                return new LauncherSettings {
                    ProjectName = apiSettings.ProjectName,
                    Scheme = apiSettings.Scheme,
                    Host = apiSettings.Host,
                    HttpsPort = portsArray[0],
                    HttpPort = portsArray[1],
                    Scopes = apiSettings.Scopes,
                    Version = apiSettings.Version,
                    ApiLauncher = apiSettings.ApiLauncher,
                    Launched = false
                };
            });
            return launcherSettings;
        }


        public static decimal GetAssemblyVersion(Assembly assembly) {
            return decimal.Parse(
                string.Join('.',
                    assembly
                        .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
                        .Cast<AssemblyInformationalVersionAttribute>()
                        .FirstOrDefault()
                        .InformationalVersion
                        .Split('.')
                        .Take(2)
                    ));
        }
    }
}