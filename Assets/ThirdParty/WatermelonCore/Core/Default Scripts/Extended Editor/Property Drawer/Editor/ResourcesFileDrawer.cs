using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(ResourcesFileAttribute))]
    public class ResourcesFileDrawer : UnityEditor.PropertyDrawer
    {
        private string[] m_FolderFiles;
        private bool m_Inited;

        private void Init(SerializedProperty property)
        {
            var resourcesFileAttribute = (ResourcesFileAttribute) attribute;

            m_FolderFiles = Resources.LoadAll(resourcesFileAttribute.path, resourcesFileAttribute.type)
                .Select(x => x.name).OrderBy(x => x).ToArray();

            m_Inited = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!m_Inited)
                Init(property);

            var propertyValue = property.stringValue;
            var m_SelectedFileId = 0;

            if (string.IsNullOrEmpty(propertyValue))
            {
                property.stringValue = null;
                m_SelectedFileId = -1;
            }
            else
            {
                var foundedKey = Array.FindIndex(m_FolderFiles, x => x == property.stringValue);

                if (foundedKey != -1)
                {
                    m_SelectedFileId = foundedKey;
                }
                else
                {
                    property.stringValue = "Null";
                    m_SelectedFileId = -1;
                }
            }

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Keyboard), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var amountRect = new Rect(position.x, position.y, position.width, position.height);

            m_SelectedFileId = EditorGUI.Popup(amountRect, m_SelectedFileId, m_FolderFiles);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();

            if (GUI.changed) property.stringValue = m_FolderFiles[m_SelectedFileId];
        }
    }
}