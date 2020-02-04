using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace EDennis.AspNetCore.Base.Web {
    public static class LauncherUtils {

        //used to parse ewh argument, which carries the name of the synchronization event as a GUID
        static readonly Regex pattern = new Regex("(?<=ewh[= ])[A-Za-z0-9\\-]+");


        /// <summary>
        /// Non-interactive mode blocking (e.g, for automated integration tests)
        /// </summary>
        /// <param name="args">arguments, one of which should be a guid event wait handle key</param>
        public static void Block(string[] args) {

            //if the ewh argument exists, create the EventWaitHandle and block on it
            foreach (var match in args.Where(a => pattern.IsMatch(a)).Select(a => pattern.Match(a))) {
                var guid = match.Value;
                using EventWaitHandle ewh = new EventWaitHandle(
                                true, EventResetMode.ManualReset, guid);
                Console.WriteLine($"{new string('-', 80)}\nRunning until EventWaitHandle {guid} is set\n{new string('-', 80)}");
                ewh.WaitOne();
                return;
            }

            return;
        }

        /// <summary>
        /// Interactive mode blocking
        /// </summary>
        public static void Block() {
            Console.WriteLine($"{ new string('-', 60)}\nRunning until any key is pressed\n{new string('-', 60)}");
            Console.ReadKey();
        }


        public static Dictionary<string,string> ToCommandLineArgs(this string[] args) {
            var dict = new Dictionary<string, string>();
            foreach(var arg in args) {
                var item = arg.Split('=', ' ');
                dict.Add(item[0].Replace("/","").Replace("-",""), item[^1]);
            }
            return dict;
        }



    }
}
