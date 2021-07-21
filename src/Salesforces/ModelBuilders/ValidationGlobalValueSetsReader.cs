using Bb;
using Bb.DataDeep.Models.Mpd;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Salesforces
{


    internal class ValidationGlobalValueSetsReader : Reader<XElement>
    {


        public ValidationGlobalValueSetsReader(SalesforceMpdBuilder salesforceMpdBuilder)
        {
            this.salesforceMpdBuilder = salesforceMpdBuilder;
        }

        public override StructureMpdBase Resolve(XElement element, StructureMpdBase parent)
        {

            var entity = parent as Entity;

            var e = element.Element(XName.Get("errorDisplayField", @"http://soap.sforce.com/2006/04/metadata"));

            StructureMpdBase current = parent;
            if (e != null)
                current = (StructureMpdBase)entity.Attributes.FirstOrDefault(c => c.Name == e.Value) ?? entity;
            
            var result = current.AddMetadata(DataDeepConstants.Contraint, DataDeepConstants.Validation, string.Empty);

            var items = element.Elements();
            foreach (var item in items)
            {

                switch (item.Name.LocalName)
                {

                    case "customValue":
                        var a = new AttributeField()
                        {
                            Name = item.Element(XName.Get("fullName", "http://soap.sforce.com/2006/04/metadata")).Value,
                            Label = item.Element(XName.Get("label", "http://soap.sforce.com/2006/04/metadata")).Value,
                            Type = new TypeReference() { Name = "" },
                            FromVersion = new Version(entity.FromVersion.ToString()),
                        };

                        var isDefault = item.Element(XName.Get("default", "http://soap.sforce.com/2006/04/metadata")).Value.ToLower() == "true";
                        if (isDefault)
                            a.AddMetadata(DataDeepConstants.ComponentModel, DataDeepConstants.IsDefault, true);
                        entity.Attributes.Add(a);
                        break;

                    case "description":
                        entity.Description = item.Value;
                        break;

                    case "masterLabel":
                        entity.Label = item.Value;
                        break;

                    case "sorted":
                        break;

                    default:
                        LocalDebug.Stop();
                        break;
                }
            }

            return parent;

        }

        private SalesforceMpdBuilder salesforceMpdBuilder;

    }

}
