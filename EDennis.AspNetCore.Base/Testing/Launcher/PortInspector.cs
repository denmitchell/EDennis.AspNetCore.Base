using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;

namespace EDennis.AspNetCore.Base.Testing {

    /// <summary>
    /// This class finds available ports.
    /// Adapted from https://gist.github.com/jrusbatch/4211535
    /// </summary>
    public class PortInspector {

        public static List<int> GetRandomAvailablePorts(int portCount) {
            Random _rand = new Random();
            var startingPorts = Enumerable.Range(0, portCount)
                                    .Select(r => _rand.Next(10001,63000)).ToList();
            return GetAvailablePorts(startingPorts, portCount);
        }


        /// <summary>
        /// checks for used ports and retrieves the first free port
        /// </summary>
        /// <returns>the free port or 0 if it did not find a free port</returns>
        public static List<int> GetAvailablePorts(int startingPort, int portCount) {

            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();
            List<int> availablePorts = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               select n.Port);

            portArray.Sort();

            //build the list of available ports
            for (int i = startingPort; i < UInt16.MaxValue; i++)
                if (!portArray.Contains(i)) {
                    availablePorts.Add(i);
                    if (availablePorts.Count == portCount)
                        return availablePorts;
                }

            throw new KeyNotFoundException($"Insufficient number of available ports.  Ports available: {availablePorts.Count}");
        }



        /// <summary>
        /// checks for used ports and retrieves the first free port
        /// </summary>
        /// <returns>the free port or 0 if it did not find a free port</returns>
        public static List<int> GetAvailablePorts(List<int> startingPorts, int portCount) {

            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();
            List<int> availablePorts = new List<int>();

            var startingPort = startingPorts.Min();
            var endingPort = startingPorts.Max();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= startingPort
                               && n.LocalEndPoint.Port <= endingPort
                               select n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               && n.Port <= endingPort
                               select n.Port);

            //getting active udp listeners
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= startingPort
                               && n.Port <= endingPort
                               select n.Port);

            portArray.Sort();

            //build the list of available ports
            foreach (var port in startingPorts) {
                for (int i = 0; i < 100; i++) {
                    if (!portArray.Contains(port + i)) {
                        availablePorts.Add(port);
                        if (availablePorts.Count == portCount)
                            return availablePorts;
                        break;
                    }
                }
            }
            throw new KeyNotFoundException($"Insufficient number of available ports.  Ports available: {availablePorts.Count}");
        }


    }
}
