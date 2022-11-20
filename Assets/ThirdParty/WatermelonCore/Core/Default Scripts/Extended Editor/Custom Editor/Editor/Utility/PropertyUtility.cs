using System;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Watermelon
{
    public static class PropertyUtility
    {
        public static T GetAttribute<T>(SerializedProperty property) where T : Attribute
        {
            var attributes = GetAttributes<T>(property);
            return attributes.Length > 0 ? attributes[0] : null;
        }

        public static T[] GetAttributes<T>(SerializedProperty property) where T : Attribute
        {
            var targetType = GetTargetObject(property).GetType();
            var fieldInfo = targetType.GetField(property.name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return (T[]) fieldInfo.GetCustomAttributes(typeof(T), true);
        }

        public static Object GetTargetObject(SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }
    }
}