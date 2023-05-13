using System.Net.Http.Headers;
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
using SideRift.SaveSystem.TypeWriters;

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
            serializedType = valueType.AssemblyQualifiedName;
            if (TryGetValueAsText(out string valueAsText))
            {
                serializedValue = valueAsText;
            }
            else
            {
                byte[] bytesValue = BinaryUtilities.ToByteArray(value);
                serializedValue = BinaryUtilities.ByteArrayToString(bytesValue);
            }
        }

        public void OnAfterDeserialize()
        {
            valueType = Type.GetType(serializedType);

            if (TryGetValueFromText(serializedValue, out object readedValue))
            {
                value = readedValue;
            }
            else
            {
                byte[] bytesValue = BinaryUtilities.StringToByteArray(serializedValue);
                value = BinaryUtilities.FromByteArray<object>(bytesValue);
            }
        }

        private bool TryGetValueAsText(out string text)
        {
            if (CustomTypeWriters.TryGetCustomTypeWriter(valueType, out ACustomTypeWriter writer))
            {
                text = writer.ToText(value);
                return (true);
            }

            if (value is ISaveItem saveItemValue)
            {
                text = saveItemValue.ToSaveText();
                return (true);
            }

            text = null;
            return (false);
        }

        private bool TryGetValueFromText(string text, out object value)
        {
            if (CustomTypeWriters.TryGetCustomTypeWriter(valueType, out ACustomTypeWriter writer))
            {
                value = writer.FromText(text);
                return (true);
            }

            if (TestType<ISaveItem>())
            {
                value = valueType.GetMethod("FromSaveText").Invoke(null, new object[] {text});
                return (true);
            }

            value = null;
            return (false);
        }

        public bool TestType<T>()
        {
            return typeof(T).IsAssignableFrom(valueType);
        }

        public bool TestType<T>(out T typedValue)
        {
            if (typeof(T).IsAssignableFrom(valueType))
            {
                typedValue = (T)value;
                return (true);
            }

            typedValue = default(T);
            return (false);
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
            if (_feature.TestType<T>(out T typedValue))
            {
                return (typedValue);
            }

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