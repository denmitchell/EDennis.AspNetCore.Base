using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace EDennis.AspNetCore.Base.Web {
    public static class LauncherUtils {
        public static void Block(string[] args) {
            //parse ewh argument, which carries the name of the synchronization event as a GUID
            Regex pattern = new Regex("(?<=ewh[= ])[A-Za-z0-9\\-]+");

            //if the ewh argument exists, create the EventWaitHandle and block on it
            foreach (var match in args.Where(a => pattern.IsMatch(a)).Select(a => pattern.Match(a))) {
                var guid = match.Value;
                using EventWaitHandle ewh = new EventWaitHandle(
                                true, EventResetMode.ManualReset, guid);
                Console.WriteLine($"{new string('-', 80)}\nRunning until EventWaitHandle {guid} is set\n{new string('-', 80)}");
                ewh.WaitOne();
                return;
            }

            //otherwise, block on ReadKey
            Console.WriteLine($"{ new string('-', 60)}\nRunning until any key is pressed\n{new string('-', 60)}");
            Console.ReadKey();
            return;
        }


    }
}
