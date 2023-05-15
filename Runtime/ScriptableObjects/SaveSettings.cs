using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerthurSaver.Settings
{
    public enum PathOrigin
    {
        DataPath,
        PersistantDataPath
    }

    public enum Serializer
    {
        JSON,
        Binary,
    }

    public enum Encryption
    {
        None,
        Aes
    }

    [CreateAssetMenu(fileName = "SaveSettings", menuName = "FerthurSaver/Settings")]
    public class SaveSettings : ScriptableObject
    {
        [SerializeField] private PathOrigin pathOrigin;
        [SerializeField] private string path;

        public Serializer serializer;
        public Encryption encryption;
        public List<string> categories;

        public JsonSerializerSettings jsonSerializerSettings;
        public BinarySerializerSettings binarySerializerSettings;
        public AesEncryptorSettings aesEncryptorSettings;

        public string GetPath()
        {
            if (pathOrigin == PathOrigin.DataPath)
                return (Path.Combine(Application.dataPath, path));
            return (Path.Combine(Application.persistentDataPath, path));
        }
    }
}
