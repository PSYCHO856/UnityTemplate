using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Watermelon
{
    [PropertyDrawer(typeof(ReorderableListAttribute))]
    public class ReorderableListPropertyDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, ReorderableList> reorderableListsByPropertyName = new();

        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            if (property.isArray)
            {
                if (!reorderableListsByPropertyName.ContainsKey(property.name))
                {
                    var reorderableList =
                        new ReorderableList(property.serializedObject, property, true, true, true, true)
                        {
                            drawHeaderCallback = rect =>
                            {
                                EditorGUI.LabelField(rect,
                                    string.Format("{0}: {1}", property.displayName, property.arraySize),
                                    EditorStyles.label);
                            },

                            drawElementCallback = (rect, index, isActive, isFocused) =>
                            {
                                var element = property.GetArrayElementAtIndex(index);
                                rect.y += 2f;

                                EditorGUI.PropertyField(
                                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element);
                            }
                        };

                    reorderableListsByPropertyName[property.name] = reorderableList;
                }

                reorderableListsByPropertyName[property.name].DoLayoutList();
            }
            else
            {
                var warning = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, PropertyUtility.GetTargetObject(property));

                EditorDrawUtility.DrawPropertyField(property);
            }
        }

        public override void ClearCache()
        {
            reorderableListsByPropertyName.Clear();
        }
    }
}