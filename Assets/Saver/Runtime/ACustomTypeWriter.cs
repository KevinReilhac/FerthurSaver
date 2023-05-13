using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SideRift.SaveSystem.TypeWriters
{
    public abstract class ACustomTypeWriter
    {
        public abstract string ToText(object value);
        public abstract object FromText(string text);
        public abstract Type Type {get;}
    }
}
