using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Bb.DataDeep.Models.Mpd
{
    public class Metadata
    {

        public Metadata()
        {
            this.Infos = new List<MetadataExtension>();
        }

        public string Category { get; set; }

        public string Name { get; set; }

        public JValue Value { get; set; }
        
        public List<MetadataExtension> Infos { get; set; }
               
        public void AddInfos(string name, object value)
        {
            var m = new MetadataExtension() { Name = name, Value = new JValue(value) };
            Infos.Add(m);
        }

    }


    public class MetadataExtension
    {

        public string Name { get; set; }

        public JValue Value { get; set; }

    }

}