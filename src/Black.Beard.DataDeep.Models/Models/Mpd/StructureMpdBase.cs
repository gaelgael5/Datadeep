using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Bb.DataDeep.Models.Mpd
{

    public class StructureMpdBase : Structure
    {

        public StructureMpdBase()
        {

            this.Description = string.Empty;
        }

        [JsonProperty(Order = 1)]
        public string Label { get; set; }

        public Version FromVersion { get; set; }

        public Version ToVersion { get; set; }

    }


}