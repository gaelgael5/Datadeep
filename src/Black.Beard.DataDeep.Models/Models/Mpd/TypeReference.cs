
using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Bb.DataDeep.Models.Mpd
{

    [DebuggerDisplay("{Name}")]
    public class TypeReference 
    {

        public TypeReference()
        {
        }

        public string Name { get; set; }

        public bool IsList { get; set; }
    
    
    }

}
