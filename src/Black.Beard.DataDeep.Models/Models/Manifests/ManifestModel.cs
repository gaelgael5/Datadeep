using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Bb.DataDeep.Models.Manifests
{

    public class ManifestModel
    {

        public ManifestModel()
        {
            Items = new List<ManifestModelItem>();
        }

        public DateTime LastUpdateDate { get; set; }

        public List<ManifestModelItem> Items { get; set; }
        
        public void Save(string _outPath)
        {

            var file = new FileInfo(Path.Combine(_outPath, "summary.json"));

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (File.Exists(file.FullName))
                File.Delete(file.FullName);

            var result = JsonConvert.SerializeObject(this, Formatting.Indented);

            file.FullName.Save(result);

            File.WriteAllText(file.FullName, result);

        }


        public static ManifestModel Create(string path)
        {

            var dir = new DirectoryInfo(path);
           
            var manifest = new ManifestModel();
            List<ManifestModelItem> _list = new List<ManifestModelItem>();
            foreach (var item in dir.GetFiles("*.json", SearchOption.AllDirectories))
            {
                var package = Mpd.Package.Load(item.FullName);
                var m = package.GetManifest();
                _list.Add(m);
                if (manifest.LastUpdateDate < m.LastUpdateDate)
                    manifest.LastUpdateDate = m.LastUpdateDate;
            }

            manifest.Items.AddRange(_list.OrderBy(c => c.Name).OrderByDescending(c => c.Version));

            return manifest;

        }


        public static ManifestModel Load(string path)
        {

            string file = Path.Combine(path, "summary.json");
            if (File.Exists(file))
            {
                var model = file.LoadContentFromFile().Deserialize<ManifestModel>();
                return model;
            }

            return null;

        }


    }

}
