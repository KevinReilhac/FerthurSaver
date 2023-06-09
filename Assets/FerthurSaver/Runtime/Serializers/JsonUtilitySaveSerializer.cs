using System.Text;
using UnityEngine;

namespace FerthurSaver
{
    /// <summary>
    /// Serialize your save to json file
    /// </summary>
    public class JsonUtilitySaveSerializer : ISaveSerializer
    {
        private  bool _prettyPrint = false;

        public JsonUtilitySaveSerializer(bool prettyPrint = false)
        {
            _prettyPrint = prettyPrint;
        }

        public SaveData Deserialize(byte[] bytes)
        {
            string json = System.Text.Encoding.UTF8.GetString(bytes);
            return (JsonUtility.FromJson<SaveData>(json));
        }

        public byte[] Serialize(SaveData saveData)
        {
            string json = JsonUtility.ToJson(saveData, _prettyPrint);
            return (Encoding.UTF8.GetBytes(json));
        }
    }
}