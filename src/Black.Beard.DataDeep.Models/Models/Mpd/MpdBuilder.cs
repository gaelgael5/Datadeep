using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.DataDeep.Models.Mpd
{

    public class MpdBuilder<T>
    {

        public MpdBuilder()
        {
            _resolvers = new Dictionary<string, Reader<T>>();
        }

        protected void Add(string key, Reader<T> reader)
        {
            _resolvers.Add(key, reader);
        }

        public void Resolve(string key, T item, StructureMpdBase parent) 
        {
            if (_resolvers.TryGetValue(key, out Reader<T> reader))
                reader.Resolve(item, parent);

            else
            {
                LocalDebug.Stop();
                throw new MissingMemberException(key);
            }

        }

        private Dictionary<string, Reader<T>> _resolvers;

    }

}
