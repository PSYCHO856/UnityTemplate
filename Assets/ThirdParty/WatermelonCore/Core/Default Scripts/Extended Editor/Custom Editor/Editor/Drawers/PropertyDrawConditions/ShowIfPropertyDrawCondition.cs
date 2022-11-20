using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyDrawCondition(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawCondition : PropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            var showIfAttribute = PropertyUtility.GetAttribute<ShowIfAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            var conditionField = target.GetType().GetField(showIfAttribute.ConditionName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
                return (bool) conditionField.GetValue(target);

            var conditionMethod = target.GetType().GetMethod(showIfAttribute.ConditionName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
                return (bool) conditionMethod.Invoke(target, null);

            var warning = showIfAttribute.GetType().Name +
                          " needs a valid boolean condition field or method name to work";
            EditorGUILayout.HelpBox(warning, MessageType.Warning);
            Debug.LogWarning(warning, target);

            return true;
        }
    }
}