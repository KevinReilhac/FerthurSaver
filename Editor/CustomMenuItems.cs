using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FerthurSaver.Components;

namespace FerthurSaver.Editors
{
    public class CustomMenuItems : Editor
    {
        [MenuItem("GameObject/FerthurSaver/Save Initializer", false, 100)]
        public static void CreateSaveInitializer()
        {
            GameObject gameObject = new GameObject("Save Initializer");
            gameObject.AddComponent<SaveInitializer>();
            Selection.activeGameObject = gameObject;
        }
    }
}