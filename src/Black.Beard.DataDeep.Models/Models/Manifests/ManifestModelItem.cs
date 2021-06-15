using System;
using System.Diagnostics;

namespace Bb.DataDeep.Models.Manifests
{
    [DebuggerDisplay("{Name}")]
    public class ManifestModelItem
    {

        public string Id { get; set; }
        public string Name { get; set; }


        public string Application { get; set; }
        public string Version { get; set; }


        public DateTime LastUpdateDate { get; set; }

        public string Path { get; set; }

    }

}
