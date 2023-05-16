using UnityEngine;
using UnityEditor;
using FerthurSaver.Components;
using FerthurSaver.Settings;

namespace FerthurSaver.Editors
{
    [CustomEditor(typeof(SaveInitializer))]
    public class SaveInitializerCustomEditor : Editor
    {
        private SaveInitializer initializer = null;
        private Editor settingsEditor = null;

        private void OnEnable()
        {
            initializer = (SaveInitializer)target;

            if (initializer.settings != null)
                settingsEditor = Editor.CreateEditor(initializer.settings);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("settings"));

            EditorGUILayout.Space();
            if (initializer.settings == null)
            {
                if (GUILayout.Button("Create new settings"))
                {
                    initializer.settings = CreateSettingsFile();
                    settingsEditor = Editor.CreateEditor(initializer.settings);
                    EditorUtility.SetDirty(initializer);
                }
            }
            else
            {
                settingsEditor.OnInspectorGUI();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public SaveSettings CreateSettingsFile()
        {
            SaveSettings saveSettings = ScriptableObject.CreateInstance<SaveSettings>();
            string path = EditorUtility.SaveFilePanel("New save settings", Application.dataPath, "SaveSettings", "asset");

            if (path == null)
                return (null);
            AssetDatabase.CreateAsset(saveSettings, path.Replace(Application.dataPath, "Assets/"));
            AssetDatabase.Refresh();

            return (saveSettings);
        }
    }
}