using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bb.DataDeep.Models.Manifests
{

    public class ManifestModel
    {

        public ManifestModel()
        {
            Items = new List<ManifestModelItem>();
        }

        [JsonIgnore]
        public string Path { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public List<ManifestModelItem> Items { get; set; }

        public void Save(string _outPath)
        {

            var file = new FileInfo(System.IO.Path.Combine(_outPath, "summary.json"));

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

            List<ManifestModelItem> _list = new List<ManifestModelItem>();
            var dirDataDeep = new DirectoryInfo(path);
            var manifest = new ManifestModel();

            foreach (var item in dirDataDeep.GetDirectories())
            {

                switch (item.Name)
                {

                    case DataDeepConstants.MpdFolder:
                        foreach (var MpdDir in item.GetFiles("*.dd.json", SearchOption.AllDirectories))
                        {
                            var package = Mpd.Package.Load(MpdDir.FullName);
                            var m = package.GetManifest();
                            _list.Add(m);
                            if (manifest.LastUpdateDate < m.LastUpdateDate)
                                manifest.LastUpdateDate = m.LastUpdateDate;
                        }
                        break;

                    case DataDeepConstants.DataReferentialFolder:
                        foreach (var MpdDir in item.GetFiles("*.dd.json", SearchOption.AllDirectories))
                        {
                            var data = Mcd.DataReferences.DataEntity.Load(MpdDir.FullName);
                            var m = data.GetManifest();
                            _list.Add(m);
                            if (manifest.LastUpdateDate < m.LastUpdateDate)
                                manifest.LastUpdateDate = m.LastUpdateDate;
                        }
                        break;

                    default:
                        break;

                }

            }

            manifest.Items.AddRange(_list.OrderBy(c => c.Name).OrderByDescending(c => c.Version));

            return manifest;

        }


        public static ManifestModel Load(string path)
        {

            string file = System.IO.Path.Combine(path, "summary.json");
            if (File.Exists(file))
            {
                var model = file.LoadContentFromFile().Deserialize<ManifestModel>();
                model.Path = path;
                return model;
            }

            return null;

        }

        public FileInfo ToSqlite()
        {

            string filePath = System.IO.Path.Combine(this.Path, "summary.db");
            var file = new FileInfo(filePath);

            var generator = new GenerateDb(file);
            generator.Generate(this);

            return file;

        }


    }

}
