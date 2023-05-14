using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics.SymbolStore;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace FerthurSaver
{
    public delegate void FeatureUpdatedEvent(string key, string value, Feature feature);

    public static class Save
    {
        public const string DEFAULT_CATEGORY = "default";
        private const string NOT_INITILIZED_MESSAGE = "The saver is not initialized.";
        private const string EXTENTION = ".save";

        private static SaveData _saveData;
        private static ISaveSerializer _saveSerializer;
        private static ISaveEncryptor _saveEncryptor;
        private static bool _isInitialized;
        private static string _filePath;

        public static bool DisplayDebug = true;

        public static event FeatureUpdatedEvent OnFeatureUpdated;
        public static event Action onWriteAsyncStart;
        public static event Action onWriteAsyncComplete;
        public static event Action onReadAsyncStart;
        public static event Action onReadAsyncComplete;

        /// <summary>
        /// Initialize Save system
        /// </summary>
        /// <param name="filePath"> Save files path </param>
        /// <param name="saveSerializer"> Type of serializer JSON/Binary etc. </param>
        /// <param name="saveEncryptor"> Type of encryptor, null for no encryption </param>
        /// <param name="categories">Features custom categories</param>
        public static void Initialize(string filePath, ISaveSerializer saveSerializer, ISaveEncryptor saveEncryptor = null, params string[] categories)
        {
            _saveData = new SaveData();
            _saveData.AddCategory(DEFAULT_CATEGORY);
            _saveSerializer = saveSerializer;
            _saveEncryptor = saveEncryptor;
            _filePath = filePath;
            _isInitialized = true;
            DebugLogFormat("Initialize with {0} and {1}", saveEncryptor != null ? saveEncryptor.GetType().Name : "null", saveSerializer != null ? saveSerializer.GetType().Name : "null");
        }

        #region Features Controlls
        /// <summary>
        /// Get a feature
        /// </summary>
        /// <param name="key">key of the item</param>
        /// <param name="category">item category ("default" by default)</param>
        /// <param name="defaultValue">value returned if the feature not exist</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns></returns>
        public static Feature<T> Get<T>(string key, string category = DEFAULT_CATEGORY, T defaultValue = default(T), bool createIfNotExist = true)
        {
            if (!_isInitialized)
            {
                Debug.LogError(NOT_INITILIZED_MESSAGE);
                return (new Feature<T>(defaultValue));
            }

            //Check if this category exist or return default value 
            if (!_saveData.TryGetCategory(category, out var categoryDict))
            {
                Debug.LogErrorFormat("{0} category not exist", category);
                return new Feature<T>(defaultValue);
            }

            //Check if this key exist or return default value 
            if (categoryDict.TryGetValue(key, out var result))
            {
                if (result.TestType<T>())
                    return (new Feature<T>(result));
                Debug.LogErrorFormat("{0} item has wrong type.", key);
                return new Feature<T>(defaultValue);
            }
            else
            {
                if (createIfNotExist)
                {
                    //Add new feature in dict
                    return AddFeature<T>(key, defaultValue, category);
                }
                else
                {
                    Debug.LogErrorFormat("{0}/{1} Feature not exist");
                    return new Feature<T>(defaultValue);
                }
            }

        }

        /// <summary>
        /// Set a feature
        /// </summary>
        /// <param name="key">the key of the feature to set</param>
        /// <param name="value">value to set</param>
        /// <param name="category">Feature category</param>
        /// <param name="createIfNotExist">Create the Feature if it not exist (true by default)</param>
        /// <typeparam name="T">Value Type</typeparam>
        /// <returns> Defined feature </returns>
        public static Feature<T> Set<T>(string key, T value, string category = DEFAULT_CATEGORY, bool createIfNotExist = true)
        {
            if (!_isInitialized)
            {
                Debug.LogError(NOT_INITILIZED_MESSAGE);
                return (new Feature<T>());
            }

            //Check if this category exist or return default value 
            if (!_saveData.TryGetCategory(category, out var categoryDict))
            {
                Debug.LogErrorFormat("{0} category not exist", category);
                return new Feature<T>();
            }

            //Check if this key exist
            if (categoryDict.TryGetValue(key, out Feature result))
                return CheckResultType<T>(key, value, category, result);
            else
                return CheckCreateIfNotExist(key, value, category, createIfNotExist);
        }

        private static Feature<T> CheckCreateIfNotExist<T>(string key, T value, string category, bool createIfNotExist)
        {
            if (createIfNotExist)
            {
                return AddFeature<T>(key, value, category);
            }
            else
            {
                Debug.LogErrorFormat("{0} key not exist.", key);
                return (new Feature<T>());
            }
        }

        private static Feature<T> CheckResultType<T>(string key, T value, string category, Feature result)
        {
            //Check if the value has the same type than the Feature at this key
            if (result.TestType<T>())
            {
                (result as Feature<T>).Value = value;
                OnFeatureUpdated?.Invoke(key, category, result);
                return (result as Feature<T>);
            }
            else
            {
                Debug.LogErrorFormat("Type mismatch on set {0}/{1} value", category, key);
                return (new Feature<T>());
            }
        }

        /// <summary>
        /// Add feature in save
        /// </summary>
        /// <param name="key">The key of the feature to set</param>
        /// <param name="value">default feature value</param>
        /// <param name="category">Feature category</param>
        /// <typeparam name="T">Value Type</typeparam>
        /// <returns>Created feature</returns>
        public static Feature<T> AddFeature<T>(string key, T value, string category = "default")
        {
            if (!_isInitialized)
            {
                Debug.LogError(NOT_INITILIZED_MESSAGE);
                return (new Feature<T>());
            }

            //Check if this category exist or return default value 
            if (!_saveData.TryGetCategory(category, out var categoryDict))
            {
                Debug.LogErrorFormat("{0} category not exist.", category);
                return new Feature<T>();
            }

            if (categoryDict.ContainsKey(key))
            {
                Debug.LogErrorFormat("{0}/{1} Feature already exist", category, key);
                return (new Feature<T>());
            }

            Feature<T> newFeature = new Feature<T>(value);
            categoryDict.Add(key, newFeature);
            OnFeatureUpdated?.Invoke(key, category, newFeature);
            return (newFeature);
        }
        #endregion //Features controlls

        #region Categories
        /// <summary>
        /// Get all features of a type in a category
        /// </summary>
        /// <param name="category">Category to get</param>
        /// <typeparam name="T">Features value type</typeparam>
        /// <returns>The list of all features of a category (null if category not exist)</returns>
        public static List<Feature<T>> GetCategory<T>(string category)
        {
            if (!_isInitialized)
            {
                Debug.LogError(NOT_INITILIZED_MESSAGE);
                return (null);
            }

            if (!_saveData.TryGetCategory(category, out var categoryDict))
            {
                List<Feature<T>> categoryFeatures = new List<Feature<T>>();

                foreach (Feature feature in categoryDict.Values)
                {
                    if (feature.TestType<T>())
                        categoryFeatures.Add(feature as Feature<T>);
                }
                return (categoryFeatures);
            }
            else
            {
                Debug.LogErrorFormat("{0} category not exist.", category);
                return (null);
            }
        }

        /// <summary>
        /// Get all features in a category
        /// </summary>
        /// <param name="category">Category to get</param>
        /// <returns>The list of all features of a category (null if category not exist)</returns>
        public static List<Feature> GetCategory(string category)
        {
            if (!_isInitialized)
            {
                Debug.LogError(NOT_INITILIZED_MESSAGE);
                return (null);
            }

            if (!_saveData.TryGetCategory(category, out var categoryDict))
            {
                List<Feature> categoryFeatures = new List<Feature>();

                foreach (Feature feature in categoryDict.Values)
                    categoryFeatures.Add(feature);
                return (categoryFeatures);
            }
            else
            {
                Debug.LogErrorFormat("{0} category not exist.", category);
                return (null);
            }
        }
        #endregion //Controlls

        #region Read/Write
        /// <summary>
        /// Write save to a file
        /// </summary>
        /// <param name="saveName"> save file name </param>
        public static async Task WriteSaveAsync(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            onWriteAsyncStart?.Invoke();
            await Task.Run(async () =>
            {
                byte[] serializedSave = _saveSerializer.Serialize(_saveData);
                if (_saveEncryptor != null)
                    serializedSave = _saveEncryptor.Encrypt(serializedSave);
                await File.WriteAllBytesAsync(fullPath, serializedSave);
                onWriteAsyncComplete?.Invoke();
            });
            DebugLogFormat("Write save at {0}", fullPath);
        }

        /// <summary>
        /// Read save to a file
        /// </summary>
        /// <param name="saveName"> save file name </param>
        public static async Task ReadSaveAsync(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            onReadAsyncStart?.Invoke();
            await Task.Run(async () =>
            {
                byte[] serializedSave = await File.ReadAllBytesAsync(fullPath);
                if (_saveEncryptor != null)
                    serializedSave = _saveEncryptor.Decript(serializedSave);
                _saveData = _saveSerializer.Deserialize(serializedSave);
                onReadAsyncComplete?.Invoke();

            });
            DebugLogFormat("Save loaded from {0}", fullPath);
        }

        /// <summary>
        /// Write save to a file
        /// </summary>
        /// <param name="saveName"> save file name </param>
        public static void WriteSave(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            onWriteAsyncStart?.Invoke();
            byte[] serializedSave = _saveSerializer.Serialize(_saveData);
            if (_saveEncryptor != null)
                serializedSave = _saveEncryptor.Encrypt(serializedSave);
            File.WriteAllBytes(fullPath, serializedSave);
            onWriteAsyncComplete?.Invoke();
            DebugLogFormat("Write save at {0}", fullPath);
        }

        /// <summary>
        /// Read save to a file
        /// </summary>
        /// <param name="saveName"> save file name </param>
        public static void ReadSave(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            onReadAsyncStart?.Invoke();
            byte[] serializedSave = File.ReadAllBytes(fullPath);
            if (_saveEncryptor != null)
                serializedSave = _saveEncryptor.Decript(serializedSave);
            _saveData = _saveSerializer.Deserialize(serializedSave);
            onReadAsyncComplete?.Invoke();
            DebugLogFormat("Save loaded from {0}", fullPath);
        }
        #endregion //Read/Write

        #region DebugWrite
        private static void DebugLog(string message)
        {
            if (!DisplayDebug)
                return;

            StringBuilder builder = new StringBuilder();

            builder.Append("☭ FerthurSaver ☭ : ");
            builder.Append(message);
            Debug.Log(builder.ToString());
        }

        private static void DebugLogFormat(string message, params object[] obj)
        {
            if (!DisplayDebug)
                return;

            DebugLog(string.Format(message, obj));
        }
        #endregion //DebugWrite

        #region Clear Save
        /// <summary>
        /// Delete a save file
        /// </summary>
        /// <param name="saveName"> Save to delete </param>
        public static void DestroySave(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            File.Delete(fullPath);
        }

        /// <summary>
        /// Clear all features from save
        /// </summary>
        public static void ClearSave()
        {
            _saveData.Clear();
            _saveData.AddCategory(DEFAULT_CATEGORY);
        }
        #endregion //Clear Save

        #region Helpers
        private static string GetFullFilePath(string saveName)
        {
            return (Path.Combine(_filePath, string.Format("save_{0}{1}", saveName, EXTENTION)));
        }
        #endregion
    }
}