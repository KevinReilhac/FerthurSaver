using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FerthurSaver
{
    /// <summary>
    /// Serialize your save to binary file
    /// </summary>
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
                SaveData saveData = obj as SaveData;
                saveData.OnAfterDeserialize();
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
                saveData.OnBeforeSerialize();
                bf.Serialize(ms, saveData);
                return ms.ToArray();
            }
        }
    }
}
