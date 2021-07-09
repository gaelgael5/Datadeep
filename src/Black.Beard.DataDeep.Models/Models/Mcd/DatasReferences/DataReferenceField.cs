using System;

namespace Bb.DataDeep.Models.Mcd.DataReferences
{

    public class DataReferenceField : Structure
    {

        public DataReferenceField()
        {

        }

        public string Label { get; set; }

        public string FunctionalKey { get; set; }

        public Version InterfaceVersion { get; set; }

    }

}
