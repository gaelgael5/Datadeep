
using System;
using System.Collections.Generic;

namespace Bb.DataDeep.Models.Mpd
{
    public class TypeReferenceRepository
    {

        static TypeReferenceRepository()
        {
            _instance = new Lazy<TypeReferenceRepository>();
        }

        private TypeReferenceRepository()
        {
            _references = new List<TypeReference>();
        }

        public static TypeReferenceRepository Instance { get => _instance.Value; }


        internal void AddReference(TypeReference typeReference)
        {
            _references.Add(typeReference);
        }

        public IEnumerable<TypeReference> References { get => _references; }

        private static readonly Lazy<TypeReferenceRepository> _instance;

        private List<TypeReference> _references;

    }

}
