using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bb.DataDeep.Models.Mpd
{

    public class StructureMpdBase : Structure
    {

        public StructureMpdBase()
        {

            this.Description = string.Empty;
        }


        public string Label { get; set; }

        public Version Version { get; set; }


    }


}