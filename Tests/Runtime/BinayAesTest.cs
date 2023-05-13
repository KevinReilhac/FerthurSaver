using UnityEngine;

namespace FerthurSaver.Tests
{
    internal class BinayAesTest : baseSaveTester
    {
        public override void Setup()
        {
            Save.DisplayDebug = false;
            Save.Initialize(Application.persistentDataPath, new BinarySaveSerializer(), new AesSaveEncryptor());
        }
    }
}