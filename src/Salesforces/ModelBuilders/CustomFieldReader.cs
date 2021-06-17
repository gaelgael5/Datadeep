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

                        object value = null;

                        if (bool.TryParse(item.Value, out bool i1))
                            value = i1;

                        else if (Int64.TryParse(item.Value, out Int64 i2))
                            value = i2;

                        else if (double.TryParse(item.Value, out double i3))
                            value = i3;

                        else if (DateTime.TryParse(item.Value, out DateTime i4))
                            value = i4;

                        else
                            value = item.Value;

                        result.AddMetadata(Constants.ComponentModel, Constants.DefaultValue, value);
                        break;

                    case "required":
                        if (bool.Parse(item.Value))
                            result.AddMetadata(Constants.Contraint, Constants.Required, true);
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
                        if (bool.Parse(item.Value))
                            result.AddMetadata(Constants.ComponentModel, Constants.IsUnique, true);
                        break;

                    case "precision":
                        var i8 = int.Parse(item.Value);
                        if (i8 > 0)
                            result.AddMetadata(Constants.ComponentModel, Constants.Precision, i8);
                        break;

                    case "scale":
                        var i7 = int.Parse(item.Value);
                        if (i7 > 0)
                            result.AddMetadata(Constants.ComponentModel, Constants.Scale, i7);
                        break;

                    case "length":
                        var i6 = int.Parse(item.Value);
                        if (i6 > 0)
                            result.AddMetadata(Constants.ComponentModel, Constants.maxLength, i6);
                        break;

                    case "caseSensitive":
                        if (bool.Parse(item.Value))
                            result.AddMetadata(Constants.ComponentModel, Constants.CaseSentitive, true);
                        break;

                    case "valueSet":
                        ManageValueSet(result, item);
                        break;

                    case "trackHistory":
                        if (bool.Parse(item.Value))
                            result.AddMetadata(Constants.ComponentModel, Constants.Tracked, true);
                        break;

                    case "trackTrending":
                        if (bool.Parse(item.Value))
                            result.AddMetadata(Constants.ComponentModel, Constants.TrackTrending, true);
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
                        if (bool.Parse(item2.Value))
                            result.AddMetadata(Constants.Contraint, "restrictedList", true);
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
                            f.Type.Name = Constants.Text;

                            switch (item3.Name.LocalName)
                            {

                                case "value":

                                    foreach (var item4 in item3.Elements())
                                    {

                                        bool i;

                                        switch (item4.Name.LocalName)
                                        {

                                            case "fullName":
                                                f.Name = item4.Value;
                                                break;

                                            case "label":
                                                f.Label = item4.Value;
                                                break;

                                            case "isActive":
                                                i = bool.Parse(item4.Value);
                                                if (i)
                                                    f.AddMetadata(Constants.ComponentModel, Constants.IsActive, true);
                                                break;

                                            case "default":
                                                i = bool.Parse(item4.Value);
                                                if (i)
                                                    f.AddMetadata(Constants.ComponentModel, Constants.IsDefault, true);
                                                break;


                                            default:
                                                LocalDebug.Stop();
                                                break;
                                        }

                                    }

                                    break;

                                case "sorted":
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
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.List);
                    break;

                case "MultiselectPicklist":
                    result.Type.IsList = true;
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.List);
                    break;

                case "Picklist":
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.List);
                    break;

                case "Checkbox":
                    result.Type.Name = Constants.Boolean;
                    result.AddMetadata("source-selection", Constants.Type, Constants.List);
                    break;

                case "Text":
                    result.Type.Name = Constants.Text;
                    break;

                case "Email":
                    result.Type.Name = Constants.Text;
                    result.AddMetadata(Constants.Contraint, Constants.Type, Constants.Email);
                    break;

                case "Number":
                    result.Type.Name = Constants.Number;
                    break;

                case "Percent":
                    result.Type.Name = Constants.Number;
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.Percent);
                    break;

                case "Location":
                    result.Type.Name = Constants.Geopraphy;
                    break;

                case "Time":
                    result.Type.Name = Constants.Time;
                    break;

                case "Date":
                    result.Type.Name = Constants.Date;
                    break;

                case "DateTime":
                    result.Type.Name = Constants.DateTime;
                    break;

                case "LongTextArea":
                    result.Type.Name = Constants.Text;
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.LongTextArea);
                    break;

                case "TextArea":
                    result.Type.Name = Constants.Text;
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.Textarea);
                    break;

                case "Lookup":
                    result.AddMetadata(Constants.Ihm, Constants.Type, Constants.Reference);
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
