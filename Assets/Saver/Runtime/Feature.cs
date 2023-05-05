using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace SideRift.SaveSystem
{
    [System.Serializable]
    public class Feature //: ISerializationCallbackReceiver
    {
        [SerializeField] protected object value;

        public bool TestType<T>()
        {
            return (value is T);
        }
    }

    public class Feature<T> : Feature
    {
        public Feature(T value)
        {
            this.value = value;
        }

        public Feature()
        {
        }

        public T GetValue()
        {
            if (value is T typedValue)
            {
                return (typedValue);
            }

            Debug.LogErrorFormat("{0} is the wrong type for this feature.", typeof(T).Name);
            return default(T);
        }

        public Feature<T> SetValue(T value)
        {
            this.value = value;
            return (this);
        }

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }
    }
}