using System;

namespace FerthurSaver.TypeWriters
{
    public abstract class ACustomTypeWriter
    {
        public abstract string ToText(object value);
        public abstract object FromText(string text);
        public abstract Type Type {get;}
    }
}
