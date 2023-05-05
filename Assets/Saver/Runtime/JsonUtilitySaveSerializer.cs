using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideRift.SaveSystem
{
    public class JsonUtilitySaveSerializer : ISaveSerializer
    {
        public async Task<SaveData> Read(string fullFilePath)
        {
            string jsonData = await File.ReadAllTextAsync(fullFilePath);
            return (JsonUtility.FromJson<SaveData>(jsonData));
        }

        public async Task Write(SaveData saveData, string fullFilePath)
        {
            string jsonData = JsonUtility.ToJson(saveData);
            await File.WriteAllTextAsync(fullFilePath, jsonData, Encoding.ASCII);
        }
    }
}