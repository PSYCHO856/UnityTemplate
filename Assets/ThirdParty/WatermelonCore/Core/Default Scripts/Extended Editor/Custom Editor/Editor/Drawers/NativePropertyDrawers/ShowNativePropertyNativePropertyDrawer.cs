using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [NativePropertyDrawer(typeof(ShowNativePropertyAttribute))]
    public class ShowNativePropertyNativePropertyDrawer : NativePropertyDrawer
    {
        public override void DrawNativeProperty(Object target, PropertyInfo property)
        {
            var value = property.GetValue(target, null);

            if (value == null)
            {
                var warning = string.Format("{0} doesn't support {1} types",
                    typeof(ShowNativePropertyNativePropertyDrawer).Name, "Reference");
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, property.Name))
            {
                var warning = string.Format("{0} doesn't support {1} types",
                    typeof(ShowNativePropertyNativePropertyDrawer).Name, property.PropertyType.Name);
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
        }
    }
}