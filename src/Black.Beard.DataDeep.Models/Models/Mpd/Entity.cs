﻿using System;
using System.Collections.Generic;

namespace Bb.DataDeep.Models.Mpd
{

    public class Entity : StructureMpdBase
    {

        public Entity()
        {
            Attributes = new List<AttributeField>();
        }


        public EntityKindEnum Kind { get; set; }

        public string FamilyName { get; set; }


        public List<AttributeField> Attributes { get; set; }

        public AttributeField AddAttribute(AttributeField attribute)
        {
            Attributes.Add(attribute);
            attribute.SetParent(this);
            return attribute;
        }

        public void SetParent(Library library)
        {
            this._parent = library;
        }

        public Library GetParent() => this._parent;

        private Library _parent;

    }

    public enum EntityKindEnum
    {
        Object,
        Contract,
        Table,
        Enumeration,
    }


}
