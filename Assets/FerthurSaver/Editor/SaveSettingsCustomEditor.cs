using UnityEngine;
using UnityEditor;

namespace FerthurSaver.Settings.Editors
{
    [CustomEditor(typeof(SaveSettings))]
    public class SaveSettingsCustomEditor : Editor
    {
        private SerializedProperty pathOrigin;
        private SerializedProperty path;
        private SerializedProperty serializer;
        private SerializedProperty encryption;
        private SerializedProperty categories;
        private SerializedProperty jsonSerializerSettings;
        private SerializedProperty binarySerializerSettings;
        private SerializedProperty aesEncryptorSettings;

        private void OnEnable()
        {
            pathOrigin = serializedObject.FindProperty("pathOrigin");
            path = serializedObject.FindProperty("path");
            serializer = serializedObject.FindProperty("serializer");
            encryption = serializedObject.FindProperty("encryption");
            categories = serializedObject.FindProperty("categories");
            jsonSerializerSettings = serializedObject.FindProperty("jsonSerializerSettings");
            binarySerializerSettings = serializedObject.FindProperty("binarySerializerSettings");
            aesEncryptorSettings = serializedObject.FindProperty("aesEncryptorSettings");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(path);
            EditorGUILayout.PropertyField(pathOrigin, new GUIContent(), GUILayout.Width(100f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializer);

            EditorGUI.indentLevel++;
            if (serializer.enumValueFlag == (int)Serializer.JSON)
                EditorGUILayout.PropertyField(jsonSerializerSettings);
            else if (serializer.enumValueFlag == (int)Serializer.Binary)
                EditorGUILayout.PropertyField(binarySerializerSettings);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(encryption);

            EditorGUI.indentLevel++;
            if (encryption.enumValueIndex == (int)Encryption.Aes)
            {
                EditorGUILayout.PropertyField(aesEncryptorSettings);
                GUI.enabled = false;
                GUILayout.Button(new GUIContent("Randomize Keys", "Not implemented for now"));
                GUI.enabled = true;
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}