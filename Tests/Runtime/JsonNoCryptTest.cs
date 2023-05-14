using UnityEngine;

namespace FerthurSaver.Tests
{
    internal class JsonNoCryptTest : baseSaveTester
    {
        public override void Setup()
        {
            Save.DisplayDebug = false;
            Save.Initialize(Application.persistentDataPath, new JsonUtilitySaveSerializer(), null);
        }

    }
}