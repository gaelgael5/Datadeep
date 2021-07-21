using Bb;
using Bb.DataDeep.Models.Mpd;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Salesforces
{

    public class SalesforceMpdBuilder : DirectoryMpdBuilder<XElement>
    {
        private string _version;

        public SalesforceMpdBuilder()
        {
            Add("CustomField", new CustomFieldReader(this));
            Add("ValidationRule", new ValidationRuleReader(this));
            Add("GlobalValueSet", new ValidationGlobalValueSetsReader(this));

        }

        internal IEnumerable<Library> Parse(string path)
        {

            var file = new FileInfo(Path.Combine(path, "sfdx-project.json"));
            if (file.Exists)
            {

                JObject sfdx = (JObject)file.FullName.LoadContentFromFile().ConvertToJson();
                var _path = sfdx["packageDirectories"] as JArray;

                foreach (var item in _path)
                {

                    var sub = item["path"].Value<string>();

                   


                    var _file = new FileInfo(Path.Combine(path, @"manifest\package.xml"));
                    var doc = XDocument.Load(_file.FullName);
                    this._version = doc.Root.Element(XName.Get("version", "http://soap.sforce.com/2006/04/metadata"))?.Value;


                    Library lib = new Library()
                    {
                        Name = sub,
                        FromVersion = new Version(this._version),
                    };

                    GenerateListValues(path, sub, lib);
                    GenerateObjects(path, sub, lib);

                    yield return lib;

                }

            }

        }

        private void GenerateObjects(string path, string sub, Library lib)
        {
            var dir = new DirectoryInfo(Path.Combine(path, sub, @"main\default\objects"));
            foreach (DirectoryInfo @object in dir.GetDirectories())
                ParseDirectory(@object, lib);
        }

        private void GenerateListValues(string path, string sub, Library lib)
        {

            DirectoryInfo dir;

            dir = new DirectoryInfo(Path.Combine(path, sub, @"main\default\globalValueSets"));

            foreach (var item2 in dir.GetFiles("*.xml"))
            {

                var entity = lib.AddEntity(new Entity()
                {
                    Name = item2.Name.Split(".")[0],
                    Kind = EntityKindEnum.Enumeration,
                    FromVersion = new Version(_version)
                });

                Debug.WriteLine($"current file : {item2.FullName}");
                var doc = XDocument.Load(item2.FullName);
                Resolve(doc.Root.Name.LocalName, doc.Root, entity);
            }
        }

        private Entity ParseDirectory(DirectoryInfo @object, Library parent)
        {

            var entity = parent.AddEntity(new Entity()
            {
                Name = @object.Name,
                FromVersion = new Version(_version),
            });

            if (entity.Name.EndsWith("__c"))
                entity.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstants.IsCustom, true);

            foreach (var item in @object.GetDirectories())
            {
                switch (item.Name)
                {

                    case "listViews":
                    case "compactLayouts":
                        break;

                    case "validationRules":
                    case "fields":
                        Parse(item, entity);
                        break;

                    case "recordTypes":
                        break;

                    case "webLinks":
                        break;

                    case "businessProcesses":
                        break;

                    default:
                        LocalDebug.Stop();
                        break;

                }
            }


            return entity;

        }


        private void Parse(DirectoryInfo fieldDirectory, Entity entity)
        {

            foreach (var item in fieldDirectory.GetFiles("*.xml"))
            {
                Debug.WriteLine($"current file : {item.FullName}");
                var doc = XDocument.Load(item.FullName);
                Resolve(doc.Root.Name.LocalName, doc.Root, entity);
            }

        }


    }

}
