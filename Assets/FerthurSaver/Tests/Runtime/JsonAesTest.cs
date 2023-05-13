using UnityEngine;

namespace FerthurSaver.Tests
{
    internal class JsonAesTest : baseSaveTester
    {
        public override void Setup()
        {
            Save.DisplayDebug = false;
            Save.Initialize(Application.persistentDataPath, new JsonUtilitySaveSerializer(), new AesSaveEncryptor());
        }
    }
}