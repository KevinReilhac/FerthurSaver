using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FerthurSaver
{
    public class CategoryDict : Dictionary<string, Feature>
    {

    }

    [Serializable]
    public partial class SaveData
    {
        [NonSerializedAttribute] private Dictionary<string, CategoryDict> _dictSaveData;

        public SaveData()
        {
            _dictSaveData = new Dictionary<string, CategoryDict>();
        }

        public CategoryDict AddCategory(string category)
        {
            if (_dictSaveData.ContainsKey(category))
            {
                Debug.LogWarningFormat("{0} category already exist");
                return (_dictSaveData[category]);
            }

            _dictSaveData.Add(category, new CategoryDict());
            return (_dictSaveData[category]);
        }

        public bool TryAddFeature(string category, string key, Feature feature, bool createCategoryIfNotExist = false)
        {
            if (TryGetCategory(category, out CategoryDict categoryDict))
            {
                if (categoryDict.TryAdd(key, feature))
                    return (true);
            }
            else if (createCategoryIfNotExist)
            {
                AddCategory(category).Add(key, feature);
                return (true);
            }

            return (false);
        }

        public bool IsCategoryExist(string category)
        {
            return (_dictSaveData.ContainsKey(category));
        }

        public bool TryGetCategory(string category, out CategoryDict categoryDict)
        {
            return (_dictSaveData.TryGetValue(category, out categoryDict));
        }

        public bool TryGetFeature(string category, string key, out Feature feature)
        {
            if (TryGetCategory(category, out CategoryDict categoryDict))
            {
                return (categoryDict.TryGetValue(key, out feature));
            }

            feature = null;
            return (false);
        }
    }
}
