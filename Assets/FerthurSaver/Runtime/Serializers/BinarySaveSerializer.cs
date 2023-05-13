using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace FerthurSaver
{
    public class BinarySaveSerializer : ISaveSerializer
    {
        private BinaryFormatter _binaryFormatter = null;

        public BinarySaveSerializer()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public SaveData Deserialize(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                object obj = bf.Deserialize(ms);
                return (SaveData)obj;
            }
        }

        public byte[] Serialize(SaveData saveData)
        {
            if (saveData == null)
                return new byte[]{};

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, saveData);
                return ms.ToArray();
            }
        }
    }
}
