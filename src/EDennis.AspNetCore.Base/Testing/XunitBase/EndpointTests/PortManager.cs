using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Testing{

    public class PortPair {
        public int HttpsPort { get; set; }
        public int HttpPort { get; set; }
    }

    public static class PortManager {

        //private static readonly BlockingCollection<PortPair> portCollection 
        //    = new BlockingCollection<PortPair>();

        //private const int MAX_QUEUE_SIZE = 100;
        //private const int MIN_QUEUE_SIZE = 10;


        public static async Task<PortPair> GetPorts() {
            var ports = PortInspector.GetRandomAvailablePorts(2);
            return new PortPair { HttpsPort = ports[0], HttpPort = ports[1] };
            //if (portCollection.Count == 0)
            //    await RefreshQueue();
            //return portCollection.Take();
        }

        //private static async Task RefreshQueue() {
        //    await Task.Run(() => { 
        //        if (portCollection.Count <= MIN_QUEUE_SIZE) {
        //            var ports = PortInspector.GetRandomAvailablePorts(MAX_QUEUE_SIZE);
        //            for (int i=0;i<MAX_QUEUE_SIZE;i+=2)
        //                portCollection.Add(new PortPair { HttpsPort = ports[i], HttpPort = ports[i + 1] });
        //        }
        //    });
        //}


    }
}
