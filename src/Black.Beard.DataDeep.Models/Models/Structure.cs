using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bb.DataDeep.Models
{

    [DebuggerDisplay("{Name}")]
    public class Structure
    {

        public Structure()
        {
            this.Metadatas = new List<Metadata>();
            this.Description = string.Empty;
        }

        [JsonProperty(Order = 0)] 
        public string Name { get; set; }

        [JsonProperty(Order = 2)]
        public string Description { get; set; }


        public Metadata AddMetadata(string category, string name, object value)
        {
            var item = Metadatas.FirstOrDefault(c => c.Category == category && Name == name && c.Value.Value == value);
            if (item == null)
            {
                var m = new Metadata() { Category = category, Name = name, Value = new JValue(value) };
                Metadatas.Add(m);
               return m;
            }
            return item;
        }

        public List<Metadata> Metadatas { get; set; }

       
    }


}