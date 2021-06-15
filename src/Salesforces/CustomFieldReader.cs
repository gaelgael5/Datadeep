using Bb;
using Bb.DataDeep.Models.Mpd;
using System;
using System.Xml.Linq;

namespace Salesforces
{


    internal class CustomFieldReader : Reader<XElement>
    {


        public CustomFieldReader(SalesforceMpdBuilder salesforceMpdBuilder)
        {
            this.salesforceMpdBuilder = salesforceMpdBuilder;
        }

        public override StructureBase Resolve(XElement element, StructureBase parent)
        {

            var entity = parent as Entity;

            var result = entity.AddAttribute(new AttributeField());

            var items = element.Elements();
            foreach (var item in items)
            {
                switch (item.Name.LocalName)
                {

                    case "fullName":
                        result.Name = item.Value;
                        break;

                    case "inlineHelpText":
                    case "description":
                        result.Description += (". " + item.Value).TrimStart(' ', '.');
                        break;

                    case "label":
                        result.Label = item.Value;
                        break;

                    case "defaultValue":
                        result.AddMetadata(Constants.ComponentModel, Constants.DefaultValue, item.Value);
                        break;

                    case "required":
                        result.AddMetadata(Constants.Contraint, Constants.Required, bool.Parse(item.Value));
                        break;

                    case "externalId":
                        // result. = item.Value;
                        break;

                    #region MasterDetail          
                    case "relationshipLabel":
                        break;
                    case "relationshipName":
                        break;
                    case "relationshipOrder":
                        break;
                    case "reparentableMasterDetail":
                        break;
                    case "writeRequiresMasterRead":
                        break;
                    #endregion MasterDetail          

                    case "unique":
                        result.AddMetadata(Constants.ComponentModel, Constants.IsUnique, bool.Parse(item.Value));
                        break;

                    case "precision":
                        result.AddMetadata(Constants.ComponentModel, Constants.Precision, int.Parse(item.Value));
                        break;

                    case "scale":
                        result.AddMetadata(Constants.ComponentModel, Constants.Scale, int.Parse(item.Value));
                        break;
                    case "length":
                        result.AddMetadata(Constants.ComponentModel, Constants.maxLength, int.Parse(item.Value));
                        break;

                    case "caseSensitive":
                        result.AddMetadata(Constants.ComponentModel, Constants.CaseSentitive, bool.Parse(item.Value));
                        break;

                    case "valueSet":
                        ManageValueSet(result, item);
                        break;

                    case "trackHistory":
                        result.AddMetadata(Constants.ComponentModel, Constants.Tracked, bool.Parse(item.Value));
                        break;

                    case "trackTrending":
                        result.AddMetadata(Constants.ComponentModel, Constants.TrackTrending, bool.Parse(item.Value));
                        break;

                    case "referenceTo":
                        result.Type.Name = item.Value;
                        break;

                    case "type":
                        ResolveType(item.Value, result);
                        break;


                    case "lookupFilter":
                    //ParseLookupFiler(result, item);
                    //break;

                    case "fieldManageability":
                    case "displayLocationInDecimal":
                    case "deleteConstraint":
                    case "visibleLines":
                    case "trackFeedHistory":
                    case "formula":
                    case "formulaTreatBlanksAs":
                        break;

                    default:
                        LocalDebug.Stop();
                        break;
                }
            }

            return result;

        }

        private static void ManageValueSet(AttributeField result, XElement item)
        {

            foreach (var item2 in item.Elements())
            {

                switch (item2.Name.LocalName)
                {

                    case "restricted":
                        result.AddMetadata(Constants.Contraint, "restrictedList", bool.Parse(item2.Value));
                        break;

                    case "valueSetName":
                        result.Type.Name = item2.Value;
                        break;

                    case "valueSettings":
                    case "controllingField":

                        break;

                    case "valueSetDefinition":

                        string subType = "enum_" + result.Name;
                        result.Type.Name = subType;
                        var package = result.GetParent().GetParent();

                        Entity e = package.AddEntity(new Entity() 
                        {
                            Name = subType,
                            Kind = EntityKindEnum.Enumeration 
                        });

                        foreach (var item3 in item2.Elements())
                        {

                            var f = e.AddAttribute(new AttributeField());

                            switch (item3.Name.LocalName)
                            {

                                case "value":
                                case "sorted":
                                    break;

                                case "fullName":
                                    f.Name = item3.Value;
                                    break;

                                case "label":
                                    f.Label = item3.Value;
                                    break;

                                case "default":
                                    var i = bool.Parse(item3.Value);
                                    if (i)
                                        f.AddMetadata(Constants.ComponentModel, Constants.IsDefault, true);
                                    break;

                                default:
                                    LocalDebug.Stop();
                                    break;

                            }


                        }
                        break;

                    default:
                        LocalDebug.Stop();
                        break;
                }
            }
        }
         

        private void ResolveType(string value, AttributeField result)
        {

            switch (value)
            {

                case "MasterDetail":
                    result.Type.IsList = true;
                    result.AddMetadata(Constants.Ihm, Constants.Type, "list");
                    break;

                case "MultiselectPicklist":
                    result.Type.IsList = true;
                    result.AddMetadata(Constants.Ihm, Constants.Type, "list");
                    break;

                case "Picklist":
                    result.AddMetadata(Constants.Ihm, Constants.Type, "list");
                    break;

                case "Checkbox":
                    result.Type.Name = "boolean";
                    result.AddMetadata("source-selection", Constants.Type, "list");
                    break;

                case "Text":
                    result.Type.Name = "text";
                    break;

                case "Email":
                    result.Type.Name = "text";
                    result.AddMetadata(Constants.Contraint, Constants.Type, "email");
                    break;

                case "Number":
                    result.Type.Name = "number";
                    break;

                case "Percent":
                    result.Type.Name = "number";
                    result.AddMetadata(Constants.Ihm, Constants.Type, "percent");
                    break;

                case "Location":
                    result.Type.Name = "geopraphy";
                    break;

                case "Time":
                    result.Type.Name = "time";
                    break;

                case "Date":
                    result.Type.Name = "date";
                    break;

                case "DateTime":
                    result.Type.Name = "datetime";
                    break;

                case "LongTextArea":
                    result.Type.Name = "text";
                    result.AddMetadata(Constants.Ihm, Constants.Type, "LongTextArea");
                    break;

                case "TextArea":
                    result.Type.Name = "text";
                    result.AddMetadata(Constants.Ihm, Constants.Type, "textarea");
                    break;

                case "Lookup":
                    result.AddMetadata(Constants.Ihm, Constants.Type, "reference");
                    break;

                case "Html":
                    break;

                case "Url":
                    break;

                case "Hierarchy":
                    break;

                default:
                    LocalDebug.Stop();
                    break;
            }


        }

        private SalesforceMpdBuilder salesforceMpdBuilder;

    }

}
