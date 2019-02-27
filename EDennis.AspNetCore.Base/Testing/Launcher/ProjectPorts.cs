using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class ProjectPorts{

        private BlockingCollection<int> _portPool
            = new BlockingCollection<int>();
        private ConcurrentDictionary<string, int> _projectPorts
            = new ConcurrentDictionary<string, int>();

        public const int DEFAULT_PORT_POOL_SIZE = 30;

        public const string WAIT_HANDLE_NAME = "0881f709-9f4f-4091-ae00-0e8ba541dc5a";


        public ProjectPorts(IConfiguration config) {
            var poolSizeStr = config["ApiLauncher:PortPoolSize"] ?? DEFAULT_PORT_POOL_SIZE.ToString();
            var poolSize = int.Parse(poolSizeStr);

            var ports = PortInspector.GetRandomAvailablePorts(poolSize);

            foreach (var port in ports)
                _portPool.Add(port);

        }

        public ProjectPortAssignment GetProjectPortAssignment(string projectName) {
            var newPort = _portPool.Take();
            var assignedPort = _projectPorts.GetOrAdd(projectName, newPort);
            var alreadyAssigned = (newPort != assignedPort);
            return new ProjectPortAssignment {
                Port = assignedPort,
                AlreadyAssigned = alreadyAssigned
            };
        }

    }
}
