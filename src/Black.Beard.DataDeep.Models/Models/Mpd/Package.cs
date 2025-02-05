﻿using Bb.DataDeep.Models.Manifests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bb.DataDeep.Models.Mpd
{

    public class Package : StructureMpdBase
    {

        public Package()
        {
            Libraries = new List<Library>();
        }

        public DateTime LastUpdateDate { get; set; }

        public string Id { get; set; }

        public string Application { get; set; }

        public List<Library> Libraries { get; set; }


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
                Kind = Manifests.DocumentKindEnum.Mpd,
                Name = this.Name,
                LastUpdateDate = this.LastUpdateDate,
                Version = this.FromVersion?.ToString(),
                Path = Getfilename(string.Empty),
                Application = this.Application,
                Id = this.Id,
            };

        }

        public void Save(string targetPath)
        {

            var _outPath = Path.Combine(targetPath, DataDeepConstants.MpdFolder);

            var file = new FileInfo(Getfilename(_outPath));

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (File.Exists(file.FullName))
                File.Delete(file.FullName);

            var result = JsonConvert.SerializeObject(this, Formatting.Indented, new Newtonsoft.Json.Converters.StringEnumConverter());

            file.FullName.Save(result);

            File.WriteAllText(file.FullName, result);

        }

        private string Getfilename(string _outPath)
        {
            var dir = !string.IsNullOrEmpty(_outPath)
                ? Path.Combine(_outPath, this.Name)
                : this.Name;

            string filename = this.Name;
            if(this.FromVersion != null)
                filename += "." + this.FromVersion.ToString();

            filename += ".dd.json";
            var file = Path.Combine(dir, filename);

            return file;

        }

        public static Package Load(ManifestModelItem manifest, ManifestModel parent)
        {

            if (manifest.Kind == DocumentKindEnum.Mpd)
            {
                var o = Path.Combine(parent.Path, "Mpd", manifest.Path);
                var package = Package.Load(o);
                return package;
            }

            return null;
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
