using Bb;
using Bb.DataDeep.Models.Mpd;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Salesforces
{


    internal class ValidationRuleReader : Reader<XElement>
    {


        public ValidationRuleReader(SalesforceMpdBuilder salesforceMpdBuilder)
        {
            this.salesforceMpdBuilder = salesforceMpdBuilder;
        }

        public override StructureBase Resolve(XElement element, StructureBase parent)
        {

            var entity = parent as Entity;

            var e = element.Element(XName.Get("errorDisplayField", @"http://soap.sforce.com/2006/04/metadata"));

            StructureBase current = parent;
            if (e != null)
                current = (StructureBase)entity.Attributes.FirstOrDefault(c => c.Name == e.Value) ?? entity;
            
            var result = current.AddMetadata(Constants.Contraint, Constants.Validation, string.Empty);

            var items = element.Elements();
            foreach (var item in items)
            {

                switch (item.Name.LocalName)
                {

                    case "fullName":
                        result.Name = item.Value;
                        break;

                    case "active":
                        result.AddInfos(Constants.IsActive, item.Value);
                        break;

                    case "errorMessage":
                    case "description":
                        result.AddInfos(item.Name.LocalName, item.Value);
                        break;

                    case "errorConditionFormula":
                    case "errorDisplayField":
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
