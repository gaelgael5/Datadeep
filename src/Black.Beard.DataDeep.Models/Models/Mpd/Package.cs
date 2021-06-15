using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bb.DataDeep.Models.Mpd
{

    public class Package : StructureBase
    {

        public Package()
        {
            Libraries = new List<Library>();
        }

        public List<Library> Libraries { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string Id { get; set; }

        public string Application { get; set; }

        public Library AddLib(Library library)
        {
            Libraries.Add(library);
            library.SetParent(this);
            return library;
        }

        public Manifests.ManifestModelItem GetManifest()
        {

            return new Manifests.ManifestModelItem()
            {
                Name = this.Name,
                LastUpdateDate = this.LastUpdateDate,
                Version = this.Version.ToString(),
                Path = Getfilename(string.Empty),
                Application = this.Application,
                Id = this.Id,
            };

        }

        public void Save(string _outPath)
        {

            var file = new FileInfo(Getfilename(_outPath));

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (File.Exists(file.FullName))
                File.Delete(file.FullName);

            var result = JsonConvert.SerializeObject(this, Formatting.Indented);

            file.FullName.Save(result);

            File.WriteAllText(file.FullName, result);

        }

        private string Getfilename(string _outPath)
        {
            var dir = !string.IsNullOrEmpty(_outPath) 
                ? Path.Combine(_outPath, this.Name) 
                : this.Name;

            string filename = this.Name + "." + this.Version.ToString() + ".json";
            var file = Path.Combine(dir, filename);

            return file;

        }

        public static Package Load(string file)
        {
            if (File.Exists(file))
            {
                var model = file.LoadContentFromFile().Deserialize<Package>();
                return model;
            }

            return null;

        }



    }


}
