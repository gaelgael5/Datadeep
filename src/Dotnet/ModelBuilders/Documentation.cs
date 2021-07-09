using System.Collections.Generic;

namespace DotnetParser
{

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref=""
    public class Documentation
    {

        public string Summary { get; set; }

        public string Returns { get; set; }

        public List<DocumentationItem> Params { get; set; }

        public string Remarks { get; set; }
        
        public List<DocumentationItem> Permissions { get; set; }
        
        public List<DocumentationItem> Exceptions { get; set; }

        public List<DocumentationItem> SeeAlsos { get;  set; }

    }

}
