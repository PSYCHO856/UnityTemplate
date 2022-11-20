using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [CustomPropertyDrawer(typeof(SmartArrayAttribute))]
    public class SmartArrayDrawer : UnityEditor.PropertyDrawer
    {
        private SerializedProperty m_BaseProperty;
        private bool m_Inited;

        private void Init(SerializedProperty property)
        {
            m_BaseProperty = property.serializedObject.FindProperty(property.GetPropertyPath());

            m_Inited = true;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!m_Inited)
                Init(property);

            if (GUI.Button(new Rect(position.x + position.width - 45, position.y, 14, 14),
                new GUIContent("-", "Remove"), EditorStyles.miniButtonLeft))
            {
                EditorApplication.delayCall += delegate
                {
                    GUI.FocusControl(null);

                    property.serializedObject.Update();

                    var index = property.GetPropertyArrayIndex();

                    m_BaseProperty.DeleteArrayElementAtIndex(index);

                    property.serializedObject.ApplyModifiedProperties();
                };

                return;
            }

            if (GUI.Button(new Rect(position.x + position.width - 31, position.y, 14, 14),
                new GUIContent("↑", "Move up"), EditorStyles.miniButtonMid))
            {
                GUI.FocusControl(null);

                var index = property.GetPropertyArrayIndex();

                if (index > 0) m_BaseProperty.MoveArrayElement(index, index - 1);
            }

            if (GUI.Button(new Rect(position.x + position.width - 17, position.y, 14, 14),
                new GUIContent("↓", "Move down"), EditorStyles.miniButtonRight))
            {
                GUI.FocusControl(null);

                var index = property.GetPropertyArrayIndex();
                var arraySize = m_BaseProperty.arraySize;

                if (index + 1 < arraySize) m_BaseProperty.MoveArrayElement(index, index + 1);
            }

            var width = position.width;

            if (property.propertyType == SerializedPropertyType.ObjectReference)
                width = position.width - 48;

            EditorGUI.PropertyField(new Rect(position.x, position.y, width, position.height), property, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}