using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bb.DataDeep.Models.Mcd.DataReferences
{

    public class DataEntity : Structure
    {

        public DataEntity()
        {
            Referential = new List<DataReferenceField>();
        }

        public List<DataReferenceField> Referential { get; set; }

        public DateTime LastUpdateDate { get; set; }
        
        public string Id { get; set; }


        public DataReferenceField AddAttribute(DataReferenceField attribute)
        {
            Referential.Add(attribute);
            return attribute;
        }

        public Manifests.ManifestModelItem GetManifest()
        {

            return new Manifests.ManifestModelItem()
            {
                Kind = Manifests.DocumentKindEnum.DataReferential,
                Name = this.Name,
                LastUpdateDate = this.LastUpdateDate,
                Version = "1.0.0",
                Path = Getfilename(string.Empty),
                Id = this.Id,
            };

        }

        private string Getfilename(string _outPath)
        {
            var dir = !string.IsNullOrEmpty(_outPath)
                ? Path.Combine(_outPath, this.Name)
                : this.Name;

            string filename = this.Name + "." + ".dd.json";
            var file = Path.Combine(dir, filename);

            return file;

        }

        public void Save(string targetPath)
        {

            var _outPath = Path.Combine(targetPath, DataDeepConstants.DataReferentialFolder);

            var file = new FileInfo(Getfilename(_outPath));

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (File.Exists(file.FullName))
                File.Delete(file.FullName);

            var result = JsonConvert.SerializeObject(this, Formatting.Indented);

            file.FullName.Save(result);

            File.WriteAllText(file.FullName, result);

        }

        public static DataEntity Load(string file)
        {
            if (File.Exists(file))
            {
                var model = file.LoadContentFromFile().Deserialize<DataEntity>();
                return model;
            }

            return null;

        }

    }

}
