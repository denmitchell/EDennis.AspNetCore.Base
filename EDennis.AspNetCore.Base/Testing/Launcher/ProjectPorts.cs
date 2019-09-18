using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Base.Testing {
    public class ProjectPorts{

        //holds a list of available ports.
        //only one thread can take a given port,
        //providing thread-safe access
        private readonly BlockingCollection<int> _portPool
            = new BlockingCollection<int>();

        //holds a mapping of project names to ports.
        //only on thread can assign a port to a project name,
        //and once assigned, it cannot be modified
        private readonly ConcurrentDictionary<string, int> _projectPorts
            = new ConcurrentDictionary<string, int>();

        public const int DEFAULT_PORT_POOL_SIZE = 30;

        public const string WAIT_HANDLE_NAME = "0881f709-9f4f-4091-ae00-0e8ba541dc5a";


        /// <summary>
        /// Constructs a new ProjectPorts instance, populating
        /// the portPool with a collection of available port numbers
        /// </summary>
        /// <param name="config">Configuration object.  If
        /// ApiLauncher:PortPoolSize is present in config, it
        /// will use that integer value to set the pool size; otherwise,
        /// it will default to ProjectPorts.DEFAULT_PORT_POOL_SIZE</param>
        public ProjectPorts(IConfiguration config) {
            var poolSizeStr = config["ApiLauncher:PortPoolSize"] ?? DEFAULT_PORT_POOL_SIZE.ToString();
            var poolSize = int.Parse(poolSizeStr);

            var ports = PortInspector.GetRandomAvailablePorts(poolSize);

            foreach (var port in ports)
                _portPool.Add(port);

        }

        /// <summary>
        /// In a thread-safe way, gets the port assigned to a project
        /// or assigns a new port to the project
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public ProjectPortAssignment GetProjectPortAssignment(string projectName) {
            var newPort = _portPool.Take();
            var assignedPort = _projectPorts.GetOrAdd(projectName, newPort);
            var alreadyAssigned = (newPort != assignedPort);
            if (alreadyAssigned)
                _portPool.Add(newPort); //return port to the pool
            return new ProjectPortAssignment {
                Port = assignedPort,
                AlreadyAssigned = alreadyAssigned
            };
        }

    }
}
