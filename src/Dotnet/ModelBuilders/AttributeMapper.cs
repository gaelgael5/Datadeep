using Bb;
using Bb.DataDeep.Models;
using System.Collections.Generic;

namespace DotnetParser
{

    public class AttributeMapper
    {

        public void Map(string name, List<List<string>> arguments, Structure parent)
        {

            string key = name;

            var index = name.LastIndexOf(".");
            if (index > -1)
            {
                key = name.Substring(index + 1);
            }

            switch (key)
            {

                case "UserSecretsIdAttribute":
                case "DisplayName":
                    break;


                case "NotNull":
                case "Required":
                    parent.AddMetadata(DataDeepConstants.Contraint, DataDeepConstants.Required, true);
                    break;

                case "FromQuery":
                case "FromBody":
                case "HttpPost":
                case "HttpGet":
                    //parent.AddMetadata(DataDeepConstants.Functional, DataDeepConstants.Service, "web method");
                    break;

                case "Area":
                case "Produces":
                case "Route":
                case "ApiController":
                case "ExternalApiRoute":
                case "ResponseCache":
                case "DisableRequestSizeLimit":
                    parent.AddMetadata(DataDeepConstants.Functional, DataDeepConstants.Service, "web service");
                    break;

                case "JsonProperty":
                    if (string.IsNullOrEmpty(parent.Description))
                        parent.Name = arguments[0][0];
                    break;

                case "Description":
                    if (string.IsNullOrEmpty(parent.Description))
                        parent.Description = arguments[0][0];
                    break;

                case "XmlElement":
                case "XmlAttribute":
                case "XmlRoot":
                case "EnumMember":
                case "DataContract":
                case "DataMember":
                case "IgnoreDataMember":
                    parent.AddMetadata(DataDeepConstants.Functional, DataDeepConstants.Service, "web service");
                    break;

                case "Serializable":
                case "Obsolete":
                case "Authorize":
                case "ExcludeFromCodeCoverage":
                case "EditorBrowsableAttribute":
                case "SuppressMessageAttribute":
                case "CompilerGeneratedAttribute":
                case "DebuggerNonUserCodeAttribute":
                case "GeneratedCodeAttribute":
                case "RelatedAssemblyAttribute":
                case "ProducesResponseType":
                case "TargetFrameworkAttribute":
                case "AssemblyCompanyAttribute":
                case "AssemblyConfigurationAttribute":
                case "AssemblyInformationalVersionAttribute":
                case "AssemblyProductAttribute":
                case "AssemblyTitleAttribute":
                case "AssemblyVersionAttribute":
                case "AssemblyFileVersionAttribute":
                case "Verb":
                case "Option":
                    break;

                default:
                    //LocalDebug.Stop();
                    break;
            }

        }

    }

}


// Microsoft.Extensions.Configuration.UserSecrets.UserSecretsIdAttribute("0709f13b-cfbc-4d13-bb96-798385da11ae")]