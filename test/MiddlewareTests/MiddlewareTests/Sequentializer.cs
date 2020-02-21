using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MiddlewareTests {
    public static class Sequentializer {
        public static BlockingCollection<bool> Gatekeeper {get; set;}
            = new BlockingCollection<bool>();

        static Sequentializer() {
            Gatekeeper.Add(true);
        }
    }
}
