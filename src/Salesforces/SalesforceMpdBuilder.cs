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


        public SalesforceMpdBuilder()
        {
            Add("CustomField", new CustomFieldReader(this));
            Add("ValidationRule", new ValidationRuleReader(this));
        }

        internal void Parse(string path, Library lib)
        {

            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (DirectoryInfo @object in dir.GetDirectories())
            {
                Entity entity = ParseObject(@object, lib);
            }

        }

        private Entity ParseObject(DirectoryInfo @object, Library parent)
        {

            var entity = parent.AddEntity(new Entity()
            {
                Name = @object.Name,
            });

            if (entity.Name.EndsWith("__c"))
                entity.AddMetadata(Constants.ComponentModel, Constants.IsCustom, true);

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
