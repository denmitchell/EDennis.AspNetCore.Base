﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EDennis.Samples.DefaultPoliciesMvc.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.DefaultPoliciesMvc
{
    public class Program
    {
        public static void Main(string[] args) {
            Task.Run(() => {
                CurrentDirectoryHelpers.SetCurrentDirectory();
            });
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}