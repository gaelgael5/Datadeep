using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bb.DataDeep.Models.Mpd
{

    [DebuggerDisplay("{Name}")]
    public class StructureBase
    {

        public StructureBase()
        {
            this.Metadatas = new List<Metadata>();
            this.Description = string.Empty;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Label { get; set; }

        public Version Version { get; set; }


        public Metadata AddMetadata(string category, string name, object value)
        {
            var m = new Metadata() { Category = category, Name = name, Value = new JValue(value) };
            Metadatas.Add(m);
            return m;
        }

        public List<Metadata> Metadatas { get; set; }

    }


}