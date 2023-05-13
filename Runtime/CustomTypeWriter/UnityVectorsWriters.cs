using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerthurSaver.TypeWriters
{
    public class Vector3TypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            string[] textValues = text.Split(',');
            float[] values = new float[3];

            for (int i = 0; i < 3; i++)
            {
                if (i < textValues.Length)
                    values[i] = float.Parse(textValues[i]);
                else
                    values[i] = 0f;
            }

            return (new Vector3(values[0], values[1], values[3]));
        }

        public override string ToText(object value)
        {
            Vector3 typedValue = (Vector3)value;
            return (string.Format("{0},{1},{2}", typedValue.x, typedValue.y, typedValue.z));
        }

        public override Type Type => typeof(Vector3);
    }

    public class Vector3IntTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            string[] textValues = text.Split(',');
            int[] values = new int[3];

            for (int i = 0; i < 3; i++)
            {
                if (i < textValues.Length)
                    values[i] = int.Parse(textValues[i]);
                else
                    values[i] = 0;
            }

            return (new Vector3Int(values[0], values[1], values[3]));
        }

        public override string ToText(object value)
        {
            Vector3Int typedValue = (Vector3Int)value;
            return (string.Format("{0},{1},{2}", typedValue.x, typedValue.y, typedValue.z));
        }

        public override Type Type => typeof(Vector3Int);
    }

    public class Vector2TypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            string[] textValues = text.Split(',');
            float[] values = new float[2];

            for (int i = 0; i < 2; i++)
            {
                if (i < textValues.Length)
                    values[i] = float.Parse(textValues[i]);
                else
                    values[i] = 0;
            }

            return (new Vector2(values[0], values[1]));
        }

        public override string ToText(object value)
        {
            Vector2 typedValue = (Vector2)value;
            return (string.Format("{0},{1}", typedValue.x, typedValue.y));
        }

        public override Type Type => typeof(Vector2);
    }

    public class Vector2IntTypeWriter : ACustomTypeWriter
    {
        public override object FromText(string text)
        {
            string[] textValues = text.Split(',');
            int[] values = new int[2];

            for (int i = 0; i < 2; i++)
            {
                if (i < textValues.Length)
                    values[i] = int.Parse(textValues[i]);
                else
                    values[i] = 0;
            }

            return (new Vector2Int(values[0], values[1]));
        }

        public override string ToText(object value)
        {
            Vector2Int typedValue = (Vector2Int)value;
            return (string.Format("{0},{1}", typedValue.x, typedValue.y));
        }

        public override Type Type => typeof(Vector2Int);
    }
}