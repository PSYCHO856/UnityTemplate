using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Watermelon
{
    [PropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            var dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            var fieldInfo = target.GetType().GetField(property.name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            var valuesFieldInfo = target.GetType().GetField(dropdownAttribute.ValuesFieldName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (valuesFieldInfo == null)
            {
                DrawWarningBox(string.Format("{0} cannot find a values field with name \"{1}\"",
                    dropdownAttribute.GetType().Name, dropdownAttribute.ValuesFieldName));
                EditorGUILayout.PropertyField(property, true);
            }
            else if (fieldInfo.FieldType == valuesFieldInfo.FieldType.GetElementType())
            {
                // Selected value
                var selectedValue = fieldInfo.GetValue(target);

                // Values and display options
                var valuesList = (IList) valuesFieldInfo.GetValue(target);
                var values = new object[valuesList.Count];
                var displayOptions = new string[valuesList.Count];

                for (var i = 0; i < values.Length; i++)
                {
                    var value = valuesList[i];
                    values[i] = value;
                    displayOptions[i] = value.ToString();
                }

                // Selected value index
                var selectedValueIndex = Array.IndexOf(values, selectedValue);
                if (selectedValueIndex < 0) selectedValueIndex = 0;

                // Draw the dropdown
                DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values, displayOptions);
            }
            else if (valuesFieldInfo.GetValue(target) is IDropdownList)
            {
                // Current value
                var selectedValue = fieldInfo.GetValue(target);

                // Current value index, values and display options
                var dropdown = (IDropdownList) valuesFieldInfo.GetValue(target);
                var dropdownEnumerator = dropdown.GetEnumerator();

                var index = -1;
                var selectedValueIndex = -1;
                var values = new List<object>();
                var displayOptions = new List<string>();

                while (dropdownEnumerator.MoveNext())
                {
                    index++;

                    var current = dropdownEnumerator.Current;
                    if (current.Value.Equals(selectedValue)) selectedValueIndex = index;

                    values.Add(current.Value);
                    displayOptions.Add(current.Key);
                }

                if (selectedValueIndex < 0) selectedValueIndex = 0;

                // Draw the dropdown
                DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values.ToArray(),
                    displayOptions.ToArray());
            }
            else
            {
                DrawWarningBox(typeof(DropdownAttribute).Name +
                               " works only when the type of the field is equal to the element type of the array");
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private void DrawDropdown(Object target, FieldInfo fieldInfo, string label, int selectedValueIndex,
            object[] values, string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            var newIndex = EditorGUILayout.Popup(label, selectedValueIndex, displayOptions);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Dropdown");
                fieldInfo.SetValue(target, values[newIndex]);
            }
        }

        private void DrawWarningBox(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
            Debug.LogWarning(message);
        }
    }
}