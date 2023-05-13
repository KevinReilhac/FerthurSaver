using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerthurSaver.TypeWriters
{
    public class IntTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            return (int.Parse(text));
        }

        public override string ToText(object value)
        {
            return (((int)value).ToString());
        }

        public override Type Type => typeof(int);

    }

    public class LongTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            return (long.Parse(text));
        }

        public override string ToText(object value)
        {
            return (((long)value).ToString());
        }

        public override Type Type => typeof(long);
    }

    public class FloatTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            return (float.Parse(text));
        }

        public override string ToText(object value)
        {
            return (((float)value).ToString());
        }

        public override Type Type => typeof(float);
    }

    public class DoubleTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            return (double.Parse(text));
        }

        public override string ToText(object value)
        {
            return (((double)value).ToString());
        }

        public override Type Type => typeof(double);
    }

    public class BoolTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            return (bool.Parse(text));
        }

        public override string ToText(object value)
        {
            return (((bool)value).ToString());
        }

        public override Type Type => typeof(bool);
    }

        public class StringTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            return (text);
        }

        public override string ToText(object value)
        {
            return (value as string);
        }

        public override Type Type => typeof(string);
    }
}