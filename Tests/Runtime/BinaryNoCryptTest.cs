using UnityEngine;

namespace FerthurSaver.Tests
{
    internal class BinaryNoCryptTest : baseSaveTester
    {
        public override void Setup()
        {
            Save.DisplayDebug = false;
            Save.Initialize(Application.persistentDataPath, new BinarySaveSerializer(), null);
        }
    }
}