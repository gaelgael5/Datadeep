using Bb.DataDeep.Models.Manifests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siteweb
{
    public class Program
    {

        /// <summary>
        /// Inject the path of the folder where datas are stored.
        /// </summary>
        /// <param name="args"></param>
        public Program(string[] args)
        {

            this.Directory = args[0];
            this.Manifest = ManifestModel.Load(this.Directory);

            Program.Instance = this;

            CreateHostBuilder(args).Build().Run();

        }

        public static void Main(string[] args)
        {
            new Program(args);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static Program Instance { get; private set; }

        public string Directory { get; }

        public ManifestModel Manifest { get; }
    }
}
