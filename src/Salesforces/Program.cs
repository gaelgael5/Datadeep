using Bb;
using Bb.DataDeep.Models.Mpd;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Salesforces
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = @"C:\Users\g.beard\Desktop\Test_salesforce\CRMV2\force-app\main\default\objects";
            string _outPath = @"C:\Users\g.beard\Desktop\datadeep";
            string applicationName = "crm v2";

            Package package = GetModel(path, applicationName);
            package.Save(_outPath);

            var i = Bb.DataDeep.Models.Manifests.ManifestModel.Create(_outPath);
            i.Save(_outPath);

        }

        private static Package GetModel(string path, string applicationName)
        {

            SalesforceMpdBuilder builder = new SalesforceMpdBuilder();

            Package package = new Package()
            {
                Name = "CRM2 (saas)",
                Label = "saas CRM v2",
                Description = "pudo referential",
                Version = new Version("1.0.0"),
                LastUpdateDate = DateTime.Now,
                Application = applicationName,
            };

            package.Id = Crc32.Calculate(package.Name + package.Version).ToString();


            Library lib = package.AddLib(new Library());
            builder.Parse(path, lib);

            return package;

        }


    }
}
