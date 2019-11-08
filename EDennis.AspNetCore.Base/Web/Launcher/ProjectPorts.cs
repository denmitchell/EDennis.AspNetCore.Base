using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Web {
    public class ProjectPorts : ConcurrentDictionary<string, ProjectPortAssignment> {

        public void GetOrAddRandomPorts(Type startupClass, out int[] ports, out string[] portArgs, out bool alreadyAssigned) {
            var assembly = startupClass.Assembly.GetName().Name;
            int[] portsArray = null;
            var projectPortAssignment = GetOrAdd(assembly, key => {
                portsArray = PortInspector.GetRandomAvailablePorts(2).ToArray();
                return new ProjectPortAssignment {
                    Version = GetAssemblyVersion(startupClass.Assembly),
                    HttpsPort = portsArray[0],
                    HttpPort = portsArray[1],
                    AlreadyAssigned = false
                };
            });
            ports = portsArray;
            alreadyAssigned = projectPortAssignment.AlreadyAssigned;
            portArgs = new string[] { $"ASPNETCORE_URLS=https://localhost:{ports[0]};http://localhost:{ports[1]}" };
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