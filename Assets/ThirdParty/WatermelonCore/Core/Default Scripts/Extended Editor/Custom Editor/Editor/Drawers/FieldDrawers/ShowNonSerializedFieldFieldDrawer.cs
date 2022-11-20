using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    [FieldDrawer(typeof(ShowNonSerializedFieldAttribute))]
    public class ShowNonSerializedFieldFieldDrawer : FieldDrawer
    {
        public override void DrawField(Object target, FieldInfo field)
        {
            var value = field.GetValue(target);

            if (value == null)
            {
                var warning = string.Format("{0} doesn't support {1} types",
                    typeof(ShowNonSerializedFieldFieldDrawer).Name, "Reference");
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, field.Name))
            {
                var warning = string.Format("{0} doesn't support {1} types",
                    typeof(ShowNonSerializedFieldFieldDrawer).Name, field.FieldType.Name);
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
        }
    }
}