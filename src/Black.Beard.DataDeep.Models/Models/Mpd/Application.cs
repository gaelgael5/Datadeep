using System;
using System.Collections.Generic;

namespace Bb.DataDeep.Models.Mpd
{

    public class Application : StructureBase
    {

        public Application()
        {
            Packages = new List<Package>();
        }

        public Version Version { get; set; }

        public List<Package> Packages { get; set; }

    }


}
