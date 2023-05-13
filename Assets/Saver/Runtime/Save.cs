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

namespace SideRift.SaveSystem
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

        public static event FeatureUpdatedEvent OnFeatureUpdated;
        public static event Action onWriteStart;
        public static event Action onWriteComplete;
        public static event Action onReadStart;
        public static event Action onReadComplete;

        public static void Initialize(string[] categories, string filePath, ISaveSerializer saveSerializer, ISaveEncryptor saveEncryptor = null)
        {
            _saveData = new SaveData();
            _saveData.AddCategory(DEFAULT_CATEGORY);
            _saveSerializer = saveSerializer;
            _saveEncryptor = saveEncryptor;
            _filePath = filePath;
            _isInitialized = true;
        }

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
                return (new Feature<T>(result));
            }
            else
            {
                if (createIfNotExist)
                {
                    //Add new feature in dict
                    AddFeature<T>(key, defaultValue, category);
                }
                else
                {
                    Debug.LogErrorFormat("{0}/{1} Feature not exist");
                    return new Feature<T>(defaultValue);
                }
            }

            //Try to return result with good type
            if (result.TestType<T>())
                return (new Feature<T>(result));

            Debug.LogErrorFormat("{0} item has wrong type.", key);
            return new Feature<T>(defaultValue);
        }

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
            else
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

        /// <summary>
        /// Write save to a file
        /// </summary>
        /// <param name="saveName"> save file name </param>
        /// <returns></returns>
        public static async Task WriteSave(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            onWriteStart?.Invoke();
            byte[] serializedSave = _saveSerializer.Serialize(_saveData);
            if (_saveEncryptor != null)
                serializedSave = _saveEncryptor.Encrypt(serializedSave);
            await File.WriteAllBytesAsync(fullPath, serializedSave);
            onWriteComplete?.Invoke();
            Debug.LogFormat("Write save at {0}", fullPath);
        }

        /// <summary>
        /// Read save to a file
        /// </summary>
        /// <param name="saveName"> save file name </param>
        public static async Task ReadSave(string saveName)
        {
            string fullPath = GetFullFilePath(saveName);

            onReadStart?.Invoke();
            byte[] serializedSave = await File.ReadAllBytesAsync(fullPath);
            if (_saveEncryptor != null)
                serializedSave = _saveEncryptor.Decript(serializedSave);
            _saveData = _saveSerializer.Deserialize(serializedSave);
            onReadComplete?.Invoke();
            Debug.LogFormat("Save loaded from {0}", fullPath);
        }

        private static string GetFullFilePath(string saveName)
        {
            return (Path.Combine(_filePath, string.Format("save_{0}{1}", saveName, EXTENTION)));
        }
    }
}