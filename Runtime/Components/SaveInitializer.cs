using FerthurSaver.Settings;
using UnityEngine;

namespace FerthurSaver.Components
{
    public class SaveInitializer : MonoBehaviour
    {
        public SaveSettings settings = null;

        private void Awake()
        {
            if (settings == null)
            {
                Debug.LogError("Save Initializer need settings");
                enabled = false;
                return;
            }

            if (!Save.IsInitialized)
                Initialize();
            else
                Destroy(gameObject);
        }

        public void Initialize()
        {
            ISaveSerializer serializer = null;
            ISaveEncryptor encryptor = null;

            //Serializer
            if (settings.serializer == Serializer.JSON)
                serializer = new JsonUtilitySaveSerializer(settings.jsonSerializerSettings.prettyPrint);
            else if (settings.serializer == Serializer.Binary)
                serializer = new BinarySaveSerializer();
            
            //Encryptor
            if (settings.encryption == Encryption.None)
                encryptor = null;
            else if (settings.encryption == Encryption.Aes)
                encryptor = new AesSaveEncryptor(settings.aesEncryptorSettings.IV, settings.aesEncryptorSettings.Key);
            
            Save.Initialize(settings.GetPath(), serializer, encryptor, settings.categories.ToArray());
        }
    }
}