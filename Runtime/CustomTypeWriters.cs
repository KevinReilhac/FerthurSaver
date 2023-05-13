using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerthurSaver.TypeWriters
{
    public static class CustomTypeWriters
    {
        private static Dictionary<Type, ACustomTypeWriter> _customWriters = null;

        public static void Initialize()
        {
            _customWriters = new Dictionary<Type, ACustomTypeWriter>();

            AddCustomWriter(new IntTypeWriter());
            AddCustomWriter(new FloatTypeWriter());
            AddCustomWriter(new DoubleTypeWriter());
            AddCustomWriter(new LongTypeWriter());
            AddCustomWriter(new BoolTypeWriter());
            AddCustomWriter(new StringTypeWriter());
            
            AddCustomWriter(new Vector3TypeWriter());
            AddCustomWriter(new Vector3IntTypeWriter());
            AddCustomWriter(new Vector2TypeWriter());
            AddCustomWriter(new Vector2IntTypeWriter());
        }

        private static void AddCustomWriter(ACustomTypeWriter customWriter)
        {
            _customWriters.Add(customWriter.Type, customWriter);
        }

        public static bool TryGetCustomTypeWriter<T>(out ACustomTypeWriter customTypeWriter)
        {
            if (_customWriters == null) Initialize();

            return (TryGetCustomTypeWriter(typeof(T), out customTypeWriter));
        }

        public static bool TryGetCustomTypeWriter(Type type, out ACustomTypeWriter customTypeWriter)
        {
            if (_customWriters == null) Initialize();

            return (_customWriters.TryGetValue(type, out customTypeWriter));
        }

        public static ACustomTypeWriter GetTypeWriter<T>()
        {
            if (_customWriters == null) Initialize();

            return (GetTypeWriter(typeof(T)));
        }

        public static ACustomTypeWriter GetTypeWriter(Type type)
        {
            if (_customWriters == null) Initialize();

            if (_customWriters.TryGetValue(type, out ACustomTypeWriter customWriter))
            {
                return (customWriter);
            }

            return (null);
        }
    }
}
