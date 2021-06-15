using System;
using System.Collections.Generic;

namespace Bb.DataDeep.Models.Mpd
{

    public class Library : StructureBase
    {

        public Library()
        {
            this.Entities = new List<Entity>();
        }

        public List<Entity> Entities { get; set; }

        public Entity AddEntity(Entity entity)
        {
            Entities.Add(entity);
            entity.SetParent(this);
            return entity;
        }

        public void SetParent(Package package)
        {
            this._parent = package;
        }

        public Package GetParent() => this._parent;

        private Package _parent;


    }


}
