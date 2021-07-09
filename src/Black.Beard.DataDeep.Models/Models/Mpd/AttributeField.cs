using System;

namespace Bb.DataDeep.Models.Mpd
{

    public class AttributeField : StructureMpdBase
    {

        public AttributeField()
        {

            this.Type = new TypeReference();

        }
               
        public TypeReference Type { get; set; }

        public void SetParent(Entity entity)
        {
            this._parent = entity;
        }

        public Entity GetParent() => this._parent;

        private Entity _parent;
    
    }

}
