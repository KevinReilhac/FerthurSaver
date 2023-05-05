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
    public class Feature : ISerializationCallbackReceiver
    {
        public object value;
        public Type valueType = null;

        [SerializeField] private string serializedValue;
        [SerializeField] private string serializedType;

        public Type GetValueType()
        {
            return this.valueType;
        }

        public void OnBeforeSerialize()
        {
            byte[] bytesValue = BinaryUtilities.ToByteArray(value);
            serializedValue = BinaryUtilities.ByteArrayToString(bytesValue);
            serializedType = valueType.AssemblyQualifiedName;
        }

        public void OnAfterDeserialize()
        {
            byte[] bytesValue = BinaryUtilities.StringToByteArray(serializedValue);
            value = BinaryUtilities.FromByteArray<object>(bytesValue);
            valueType = Type.GetType(serializedType);
        }


        public bool TestType<T>()
        {
            return valueType == typeof(T);
        }
    }

    public class Feature<T>
    {
        private Feature _feature;
        public Feature(Feature feature)
        {
            _feature = feature;
        }

        public Feature(T value)
        {
            _feature = new Feature();
            _feature.value = value;
            _feature.valueType = typeof(T);
        }

        public Feature()
        {
            _feature = new Feature();
        }

        public T GetValue()
        {
            if (_feature.value is T typedValue)
            {
                return (typedValue);
            }

            Debug.LogErrorFormat("{0} is the wrong type for this feature.", typeof(T).Name);
            return default(T);
        }

        public Feature<T> SetValue(T value)
        {
            _feature.value = value;
            return (this);
        }

        public T Value
        {
            get => GetValue();
            set => SetValue(value);
        }

        public Feature GetFeature() => _feature;
        public static implicit operator Feature(Feature<T> d) => d.GetFeature();
    }
}