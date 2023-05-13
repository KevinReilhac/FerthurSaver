using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace FerthurSaver
{
    public partial class SaveData : ISerializationCallbackReceiver
    {
        [Serializable]
        private class SerializedCategoryItem
        {
            public SerializedCategoryItem(string categoryName, List<SerializedFeatureItem> featureList)
            {
                this.CategoryName = categoryName;
                this.Features = featureList;
            }

            public string CategoryName;
            public List<SerializedFeatureItem> Features;
        }

        [Serializable]
        private class SerializedFeatureItem
        {
            public SerializedFeatureItem(string key, Feature feature)
            {
                this.Key = key;
                this.Feature = feature;
            }

            public string Key;
            public Feature Feature;
        }

        [SerializeField] private List<SerializedCategoryItem> serializedCategories = new List<SerializedCategoryItem>();

        public void OnBeforeSerialize()
        {
            serializedCategories.Clear();
            foreach (var category in _dictSaveData)
            {
                List<SerializedFeatureItem> featureItems = new List<SerializedFeatureItem>();
                foreach (var feature in category.Value)
                {
                    featureItems.Add(new SerializedFeatureItem(feature.Key, feature.Value));
                }
                serializedCategories.Add(new SerializedCategoryItem(category.Key, featureItems));
            }
        }

        public void OnAfterDeserialize()
        {
            _dictSaveData = new Dictionary<string, CategoryDict>();

            foreach (SerializedCategoryItem categoryItem in serializedCategories)
            {
                foreach (SerializedFeatureItem featureItem in categoryItem.Features)
                {
                    TryAddFeature(categoryItem.CategoryName, featureItem.Key, featureItem.Feature, createCategoryIfNotExist: true);
                }
            }
        }
    }
}