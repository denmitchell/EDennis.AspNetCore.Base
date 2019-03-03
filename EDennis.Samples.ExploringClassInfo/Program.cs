using EDennis.Samples.Colors.InternalApi;
using System;
using Newtonsoft.Json.Linq;

namespace EDennis.Samples.ExploringClassInfo {
    class Program {
        static void Main(string[] args) {
            var info = new ClassInfo<Startup>();
            var json = JToken.FromObject(info).ToString();
            Console.WriteLine(json);
            Console.WriteLine("\n\nPress any key to quit");
            Console.ReadKey();
        }
    }
}
