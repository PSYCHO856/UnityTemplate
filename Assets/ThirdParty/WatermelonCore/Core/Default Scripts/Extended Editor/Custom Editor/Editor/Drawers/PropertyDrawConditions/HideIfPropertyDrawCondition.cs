using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [PropertyDrawCondition(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawCondition : PropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            var hideIfAttribute = PropertyUtility.GetAttribute<HideIfAttribute>(property);
            var target = PropertyUtility.GetTargetObject(property);

            var conditionField = target.GetType().GetField(hideIfAttribute.ConditionName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
                return !(bool) conditionField.GetValue(target);

            var conditionMethod = target.GetType().GetMethod(hideIfAttribute.ConditionName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
                return !(bool) conditionMethod.Invoke(target, null);

            var warning = hideIfAttribute.GetType().Name +
                          " needs a valid boolean condition field or method name to work";
            EditorGUILayout.HelpBox(warning, MessageType.Warning);
            Debug.LogWarning(warning, target);

            return true;
        }
    }
}